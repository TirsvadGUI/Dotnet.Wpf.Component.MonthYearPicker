[![NuGet Downloads][nuget-shield]][nuget-url][![Contributors][contributors-shield]][contributors-url][![Forks][forks-shield]][forks-url][![Stargazers][stars-shield]][stars-url][![Issues][issues-shield]][issues-url][![License][license-shield]][license-url][![LinkedIn][linkedin-shield]][linkedin-url]

# ![Logo][Logo] Dotnet.Template.NugetPackage.NugetPackage
A template for nuget packages projects

<!-- ![Screenshot1][screenshot1-url] -->

## Description
This is a template for creating .NET nugets projects. It includes a basic structure and some common files to get started quickly.

## Features

## Getting Started

### Prerequisites
- Dotnet 9.0 or later

### Installation
The TirsvadCLI.Template can be installed in several ways:

#### Fork the projects
In github forks this project to your account and adjust it for your need.

#### Clone the repo (In the project have something simerly to this)
![Repo size][repos-size-shield]

1. **Clone the repository:**

    ```bash
    git clone https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage.git 
    cd Dotnet.Template.NugetPackage
    ```

1. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

1. **Update database (if needed):**

    ```bash
    dotnet ef database update --project src/Template.Infrastructure
    ```

1. **Build the project:**

    ```bash
    dotnet build
    ```

1. **Run the API:**

    ```bash
    dotnet run --project src/Template
    ```

#### NuGet Package

```Powershell
dotnet add package NugetPackageName
```


## Usage

### Notes

Change TirsvadCLI/Dotnet.Template.NugetPackage with the name of your project.
Change NugetPackageName with the name of your nuget package.

Add Doxygen to the project and add a script to generate the documentation.

In project file for library, add the following lines:
```xml
  <PropertyGroup>
    <VersionPrefix>0.1.0</VersionPrefix>
    <PackageId>$(AssemblyName)</PackageId>
    <Title></Title>
    <Authors>Jens Tirsvad Nielsen</Authors>
    <Company>TirsvadCLI</Company>
    <PackageIcon>logo.png</PackageIcon>
    <GeneratePackageOnBuild>True</GeneratePackageOnBuild>
    <RepositoryUrl>https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage</RepositoryUrl>
    <PackageTags>Console</PackageTags>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <PackageLicenseFile>LICENSE.txt</PackageLicenseFile>
    <Description></Description>
  </PropertyGroup>
  <PropertyGroup>
    <IncludeSymbols>true</IncludeSymbols>
    <SymbolPackageFormat>snupkg</SymbolPackageFormat>
  </PropertyGroup>
  <ItemGroup>
    <None Include="..\..\image\logo\64x64\logo.png">
      <Pack>True</Pack>
      <PackagePath>\</PackagePath>
    </None>
    <None Include="..\..\README.md">
      <Pack>True</Pack>
      <PackagePath>\</PackagePath>
    </None>
    <None Include="..\..\LICENSE">
      <Pack>True</Pack>
      <PackagePath>\</PackagePath>
    </None>
  </ItemGroup>
```

#### Increment version number script file

run the command with parameters or change the top of the script file for your need

change these lines
```powershell
param(
    # Path to the project file; adjust this default value if needed.  
    [string]$ProjectFilePath = "$PSScriptRoot/src/Example/Example.csproj",
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
```

and then run .\IncrementBuildAndPush.ps1


## Example of code
See example [here][example-url]

## 📂 Folder Structure
```plaintext
Dotnet.Template.NugetPackage/
├── 📄 docs/                         # Documentation files
│   └── 📄 doxygen/                  # Doxygen output
├── 🖼️ images/                       # Images used in documentation
├── 📂 src/                          # Source code for the library
│   ├── 📦 Template/                 # Main project
│   ├── 📦 Template.Application/     # Application layer
│   │   ├── 📦 Models/               # Data transfer objects (DTOs)
│   │   └── 📦 Services/             # Application services
│   ├── 📦 Template.Infrastructure/  # Infrastructure project
│   │   ├── 📦 Data/                 # Data access layer
│   │   └── 📦 Services/             # Infrastructure services
│   └── 📦 Template.Domain/          # Domain project
│       └── 📦Entities/              # Domain entities
└── 📂 tests/                        # Test projects
    ├── 📦 Template.Tests/           # Unit tests for the main project
    └── 📦 Template.IntegrationTests/ # Integration tests
```

Under folder src/** and tests/** is an example of how you can structure your project.
This is just an example and you can change it as you like.
Folders do not exist, but is just an example of how you can structure your project.

## Contributing
Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

See [CONTRIBUTING.md](CONTRIBUTING.md)

## Bug / Issue Reporting  
If you encounter a bug or have an issue to report, please follow these steps:  

1. **Go to the Issues Page**  
  Navigate to the [GitHub Issues page][githubIssue-url].  

2. **Click "New Issue"**  
  Click the green **"New Issue"** button to create a new issue.  

3. **Provide Details**  
  - **Title**: Write a concise and descriptive title for the issue.  
  - **Description**: Include the following details:  
    - Steps to reproduce the issue.  
    - Expected behavior.  
    - Actual behavior.  
    - Environment details (e.g., OS, .NET version, etc.).  
  - **Attachments**: Add screenshots, logs, or any other relevant files if applicable.  

4. **Submit the Issue**  
  Once all details are filled in, click **"Submit new issue"** to report it.  

## License
Distributed under the AGPL-3.0 [License][license-url].

## Contact
Jens Tirsvad Nielsen - [LinkedIn][linkedin-url]

## Acknowledgments
- [dotnet](https://dotnet.microsoft.com/)

<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/TirsvadCLI/Dotnet.Template.NugetPackage?style=for-the-badge
[contributors-url]: https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/TirsvadCLI/Dotnet.Template.NugetPackage?style=for-the-badge
[forks-url]: https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage/network/members
[stars-shield]: https://img.shields.io/github/stars/TirsvadCLI/Dotnet.Template.NugetPackage?style=for-the-badge
[stars-url]: https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage/stargazers
[issues-shield]: https://img.shields.io/github/issues/TirsvadCLI/Dotnet.Template.NugetPackage?style=for-the-badge
[issues-url]: https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage/issues
[license-shield]: https://img.shields.io/github/license/TirsvadCLI/Dotnet.Template.NugetPackage?style=for-the-badge
[license-url]: https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage/blob/main/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/jens-tirsvad-nielsen-13b795b9/
[githubIssue-url]: https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage/issues/
[repos-size-shield]: https://img.shields.io/github/repo-size/TirsvadCLI/Dotnet.Template.NugetPackage?style=for-the-badg

[logo]: https://raw.githubusercontent.com/TirsvadCLI/Dotnet.Template.NugetPackage/main/images/logo/32x32/logo.png

<!-- If there is example code -->
[example-url]: https://raw.githubusercontent.com/TirsvadCLI/Dotnet.Template.NugetPackage/main/src/Example/Example.cs

<!-- If this is a Nuget package -->
[nuget-shield]: https://img.shields.io/nuget/dt/NugetPackageName?style=for-the-badge
[nuget-url]: https://www.nuget.org/packages/NugetPackageName/

<!-- If this is a downloadable package from github -->
[downloads-shield]: https://img.shields.io/github/downloads/TirsvadCLI/Dotnet.Template.NugetPackage/total?style=for-the-badge
[downloads-url]: https://github.com/TirsvadCLI/Dotnet.Template.NugetPackage/releases

<!-- If there is screenshots -->
<!-- [screenshot1]: https://raw.githubusercontent.com/TirsvadCLI/Dotnet.Template.NugetPackage/main/images/small/Screenshot1.png
[screenshot1-url]: https://raw.githubusercontent.com/TirsvadCLI/Dotnet.Template.NugetPackage/main/images/Screenshot1.png -->