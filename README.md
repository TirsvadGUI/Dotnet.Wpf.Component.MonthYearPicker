[![NuGet Downloads][nuget-shield]][nuget-url][![Contributors][contributors-shield]][contributors-url][![Forks][forks-shield]][forks-url][![Stargazers][stars-shield]][stars-url][![Issues][issues-shield]][issues-url][![License][license-shield]][license-url][![LinkedIn][linkedin-shield]][linkedin-url]

# ![Logo][Logo] Month year picker
A modern, reusable WPF user control for selecting a month and year, featuring a combo box for months and a numeric up/down for years.

## Table of Contents
- [Description](#description)
- [✨ Features](#-features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
    - [1. Install via NuGet](#1-install-via-nuget)
    - [2. Clone the Repository](#2-clone-the-repository)
- [🛠️ Usage](#️-usage)
- [Example of code](#example-of-code)
- [📦 Project Structure](#-project-structure)
- [Contributing](#contributing)
- [Bug / Issue Reporting](#bug--issue-reporting)
- [License](#license)
- [Contact](#contact)
- [Acknowledgments](#acknowledgments)

## Description
A modern, reusable WPF user control for selecting a month and year, featuring a combo box for months and a numeric up/down for years.

## ✨ Features
- Easy integration into any WPF application
- Customizable appearance and localization support
- Keyboard and mouse navigation
- Data binding support for MVVM architecture

## Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- Visual Studio 2022 or newer (recommended)

### Installation

#### 1. Install via NuGet

```powershell
dotnet add package TirsvadGUI.Wpf.Component.MonthYearPickerCb
```

#### 2. Clone the Repository
![Repo size][repos-size-shield]

1. **Clone the repository:**

    ```bash
    git clone https://github.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker.git 
    cd Dotnet.Wpf.Component.MonthYearPicker
    ```

1. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

1. **Build the project:**

    ```bash
    dotnet build
    ```


## 🛠️ Usage
Add the control to your XAML:
```xml
<Window 
...
xmlns:controls="clr-namespace:TirsvadGUI.UI.Components;assembly=TirsvadGUI.UI"
...
>
    <Grid>
        ...
        <!-- Example with property bindings --> 
        <controls:MonthYearComboboxPickerUC 
            SelectedDate="{Binding SelectedDate}" <!-- The currently selected date (DateTime) --> 
            BeginsAtDate="{Binding MinDate}" <!-- The earliest selectable date (DateTime) -->
            EndsAtDate="{Binding MaxDate}" <!-- The latest selectable date (DateTime) -->
            FirstDayOfMonth="{Binding IsFirstDay}" /> <!-- true: select first day, false: last day of month -->
        ...
    </Grid>
</Window>
```


**Note:**  
- Replace `TirsvadGUI.UI.Components` and assembly name as needed.
- For advanced usage and customization, see the [example code][example-url].

---

## Example of code
See example [here][example-url]

## 📦 Project Structure
```plaintext
Dotnet.Wpf.Component.MonthYearPicker/
├── 📄 docs/                             # Documentation files
│   └── 📄 doxygen/                      # Doxygen output
├── 🖼️ images/                           # Images used in documentation
├── 📂 src/                              # Source code
│   └── 📦 TirsvadGUI.Wpf/               # Main WPF project
│       └── 📦 Components/               # UI Components
├── 📂 examples/                         # Examples on how to use the project
└── 📂 tests/                            # Test projects
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

<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker?style=for-the-badge
[contributors-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker?style=for-the-badge
[forks-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/network/members
[stars-shield]: https://img.shields.io/github/stars/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker?style=for-the-badge
[stars-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/stargazers
[issues-shield]: https://img.shields.io/github/issues/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker?style=for-the-badge
[issues-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/issues
[license-shield]: https://img.shields.io/github/license/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker?style=for-the-badge
[license-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/blob/main/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/jens-tirsvad-nielsen-13b795b9/
[githubIssue-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/issues/
[repos-size-shield]: https://img.shields.io/github/repo-size/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker?style=for-the-badg

[logo]: https://raw.githubusercontent.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/main/images/logo/32x32/logo.png

<!-- If there is example code -->
[example-url]: https://raw.githubusercontent.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/main/examples/Example.SimpleUse

<!-- If this is a Nuget package -->
[nuget-shield]: https://img.shields.io/nuget/dt/TirsvadGUI.Wpf.Component.MonthYearPickerCb?style=for-the-badge
[nuget-url]: https://www.nuget.org/packages/TirsvadGUI.Wpf.Component.MonthYearPickerCb/

<!-- If this is a downloadable package from github -->
[downloads-shield]: https://img.shields.io/github/downloads/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/total?style=for-the-badge
[downloads-url]: https://github.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/releases

<!-- If there is screenshots -->
<!-- [screenshot1]: https://raw.githubusercontent.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/main/images/small/Screenshot1.png
[screenshot1-url]: https://raw.githubusercontent.com/TirsvadGUI/Dotnet.Wpf.Component.MonthYearPicker/main/images/Screenshot1.png -->