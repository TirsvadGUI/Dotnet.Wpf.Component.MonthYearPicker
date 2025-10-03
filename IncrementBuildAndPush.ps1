param(
    # Path to the project file; adjust this default value if needed.  
    [string]$ProjectFilePath = "$PSScriptRoot/src/TirsvadGUI.Wpf/TirsvadGUI.Wpf.csproj",
    # Path to the NuGet API key for authentication.  
    [string]$NuGetApiKey = "$env:NugetTirsvadCLI",  # Replace with your actual API key or set it in the environment variable.
    # NuGet source URL (default is nuget.org).  
    [string]$NuGetSource = "https://api.nuget.org/v3/index.json",
    # Path to the certificate file (PFX format) for signing
    [string]$CertificatePath = "$PSScriptRoot/../../../cert/NugetCertTirsvad/Tirsvad.pfx",
    # Password for the certificate file
    [string]$CertificatePassword = "$env:CertTirsvadPassword", # Replace with your actual password or set it in the environment variable.
    # Is this a NuGet package?
    [switch]$IsNuGetPackage = $true,
    # Selfsigned nuget should be off as Nuget.org donnot accept selfsigned packages
    [switch]$SelfSignedNuGet = $true,
    # Path to signtool.exe
    [string]$SignToolPath = "C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64\signtool.exe"
)

# set variables
$IncrementBuildUrl = "https://github.com/TirsvadCLI/PS.IncreaseBuildNumberVSProject/releases/download/1.0.0/IncrementBuild.zip"
$IncrementBuildDestinationPath = "$PSScriptRoot/IncrementBuild.zip"
$IncrementBuildUnzipPath = "$PSScriptRoot/IncrementBuild"
$IncrementBuildScriptPath = "$IncrementBuildUnzipPath/IncrementBuild.ps1"

function ExitWaitForKey {
    param(
        [string]$ErrorMessage = ""
    )

    if ($ErrorMessage) {
        Write-Host "ERROR: $ErrorMessage" -ForegroundColor Red
    }

    Write-Host "Press any key to continue..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

    exit 1
}

function DownloadAndProcessZip {
    param(
        [string]$DownloadUrl,
        [string]$DestinationPath,
        [string]$UnzipPath
    )

    # Download the zip file
    Write-Output "Downloading zip file from $DownloadUrl..."
    try {
        Invoke-WebRequest -Uri $DownloadUrl -OutFile $DestinationPath -ErrorAction Stop
        Write-Output "Download completed: $DestinationPath"
    } catch {
        ExitWaitForKey -ErrorMessage "Failed to download the zip file. Error: $_"
    }

    # Unzip the file
    Write-Output "Unzipping the file to $UnzipPath..."
    try {
        Expand-Archive -Path $DestinationPath -DestinationPath $UnzipPath -Force
        Write-Output "Unzipping completed."
    } catch {
        ExitWaitForKey -ErrorMessage "Failed to unzip the file. Error: $_"
    }
}

function DeleteZipAndFolder {
    param(
        [string]$DestinationPath,
        [string]$UnzipPath
    )
    # Delete the zip file
    Write-Output "Deleting the zip file: $DestinationPath..."
    try {
        Remove-Item -Path $DestinationPath -Force
        Write-Output "Zip file deleted."
    } catch {
        ExitWaitForKey -ErrorMessage "Failed to delete the zip file. Error: $_"
    }
    # Delete unzipped files
    Write-Output "Deleting unzipped files in $UnzipPath..."
    try {
        Remove-Item -Path $UnzipPath -Recurse -Force
        Write-Output "Unzipped files deleted."
    } catch {
        ExitWaitForKey -ErrorMessage "Failed to delete unzipped files. Error: $_"
    }
}

# Ensure the script is running as an administrator.
if (-not (New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Output "Elevating script to run as administrator..."
    $arguments = @('-File', $MyInvocation.MyCommand.Source)
    if ($args) {
        $arguments += $args
    }
    Start-Process -FilePath 'powershell' -ArgumentList $arguments -Verb RunAs
    exit
}

# Download and process the zip file if needed
DownloadAndProcessZip -DownloadUrl $IncrementBuildUrl -DestinationPath $IncrementBuildDestinationPath -UnzipPath $IncrementBuildUnzipPath
# Run the IncrementBuild.ps1 script
Write-Output "Running the IncrementBuild.ps1 script from $UnzipPath..."
if (Test-Path $IncrementBuildScriptPath) {
    try {
        & $IncrementBuildScriptPath -ProjectFilePath $ProjectFilePath
        Write-Output "IncrementBuild.ps1 executed successfully."
    } catch {
        ExitWaitForKey -ErrorMessage "Failed to execute IncrementBuild.ps1. Error: $_"
    }
} else {
    ExitWaitForKey -ErrorMessage "IncrementBuild.ps1 not found in $UnzipPath."
}
# Delete zip and folder if needed
DeleteZipAndFolder -DestinationPath $IncrementBuildDestinationPath -UnzipPath $IncrementBuildUnzipPath

# Build the project in Release mode.
Write-Output "Building the project in Release mode..."
& dotnet build $ProjectFilePath -c Release
if ($LASTEXITCODE -ne 0) {
    ExitWaitForKey -ErrorMessage "Build failed. Please check the output for errors."
}
Write-Output "Build succeeded in Release mode."

if ($IsNuGetPackage) {
    if ($SelfSignedNuGet) {
        # Selfigned Nuget is not supported by nuget.org
        Write-Output "Self-signed NuGet packages are not supported by nuget.org."
    } else {
        # Pack the project to create a NuGet package.
        Write-Output "Packing the project to create a NuGet package..."
        & dotnet pack $ProjectFilePath -c Release --no-build
        if ($LASTEXITCODE -ne 0) {
            ExitWaitForKey -ErrorMessage "Packing failed. Please check the output for errors."
        }
        Write-Output "NuGet package created successfully."

        # Find the generated .nupkg file.
        $projectDirectory = Split-Path -Path $ProjectFilePath -Parent
        $packagePath = Get-ChildItem -Path "$projectDirectory\bin\Release" -Filter *.nupkg | Select-Object -ExpandProperty FullName
        if (-not $packagePath) {
            ExitWaitForKey -ErrorMessage "NuGet package not found in the expected directory."
        }
        Write-Output "Found NuGet package: $packagePath"

        # Sign the NuGet package with the certificate.
        Write-Output "Signing the NuGet package with the certificate..."
        & dotnet nuget sign $packagePath `
            --certificate-path $CertificatePath `
            --certificate-password $CertificatePassword `
            --timestamper "http://timestamp.digicert.com"
        if ($LASTEXITCODE -ne 0) {
            ExitWaitForKey -ErrorMessage "Failed to sign the NuGet package. Please check the output for errors."
        }
        Write-Output "NuGet package signed successfully."

        # Push the NuGet package to the specified source.
        Write-Output "Pushing the NuGet package to $NuGetSource..."
        & dotnet nuget push $packagePath --api-key $NuGetApiKey --source $NuGetSource
        if ($LASTEXITCODE -ne 0) {
            ExitWaitForKey -ErrorMessage "Failed to push the NuGet package. Please check the output for errors."
        }
        Write-Output "NuGet package uploaded successfully to $NuGetSource."
    }
} else {
    # Find the generated .exe file.
    $projectDirectory = Split-Path -Path $ProjectFilePath -Parent
    Write-Output "Project directory: $projectDirectory"
    Write-Output "Locating the .exe file in the Release directory..."
    # Find the first PropertyGroup element that contains a TargetFramework element.
    $propertyGroup = $projXml.Project.PropertyGroup | Where-Object { $_.TargetFramework }
    if ($propertyGroup) {
        $exePath = Get-ChildItem -Path "$projectDirectory\bin\Release" -Filter *.exe | Select-Object -ExpandProperty FullName
    } else {
        # If no TargetFramework is found, assume the default path.
        $exePath = Get-ChildItem -Path "$projectDirectory\bin\Release\$propertyGroup.TargetFramework" -Filter *.exe | Select-Object -ExpandProperty FullName
    }
    $exePath = Get-ChildItem -Path "$projectDirectory\bin\Release\net9.0" -Filter *.exe | Select-Object -ExpandProperty FullName
    if (-not $exePath) {
        ExitWaitForKey -ErrorMessage "Executable file not found in the expected directory."
    }
    Write-Output "Found executable: $exePath"

    # Check if signtool.exe is available
    #$SignToolPath = Get-Command signtool.exe -ErrorAction SilentlyContinue
    if (-not $SignToolPath) {
        ExitWaitForKey -ErrorMessage "signtool.exe not found. Ensure it is installed and available in the system's PATH."
    }

    # Sign the .exe file with the certificate.
    Write-Output "Signing the executable with the certificate..."
    $signOutput = & $SignToolPath sign /fd sha256 /f $CertificatePath /p $CertificatePassword /t http://timestamp.digicert.com $exePath 2>&1
    Write-Output "SignTool Output: $signOutput"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to sign the executable. Output:"
        Write-Output $signOutput
        ExitWaitForKey -ErrorMessage "Failed to sign the executable. Please check the output for errors."
    }
    Write-Output "Executable signed successfully."
}

# not implemented yet
function UploadToGitHub {
    param(
        [string]$PackagePath,
        [string]$GitHubRepoUrl,
        [string]$GitHubToken
    )

    if (-not (Test-Path $PackagePath)) {
        ExitWaitForKey -ErrorMessage "NuGet package not found at path: $PackagePath"
    }

    Write-Output "Uploading NuGet package to GitHub..."

    # Prepare the GitHub API URL for uploading the release asset
    $uploadUrl = "$GitHubRepoUrl/releases/assets?name=$(Split-Path -Leaf $PackagePath)"

    # Use Invoke-RestMethod to upload the package
    try {
        $response = Invoke-RestMethod -Uri $uploadUrl `
            -Headers @{ Authorization = "Bearer $GitHubToken" } `
            -Method Post `
            -ContentType "application/octet-stream" `
            -InFile $PackagePath

        Write-Output "NuGet package uploaded successfully to GitHub."
    } catch {
        ExitWaitForKey -ErrorMessage "Failed to upload NuGet package to GitHub. Error: $_"
    }
}

# Example usage of the function
#$GitHubRepoUrl = "https://api.github.com/repos/YourUsername/YourRepo"
#$GitHubToken = $env:GitHubToken # Replace with your GitHub token or set it in the environment variable
#UploadToGitHub -PackagePath $packagePath -GitHubRepoUrl $GitHubRepoUrl -GitHubToken $GitHubToken

Start-Sleep -Seconds 3000
