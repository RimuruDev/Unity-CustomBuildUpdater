**[🇷🇺 Read this in Russian](RU_README.md)**

# Unity Custom Build Updater

Unity Custom Build Updater is a Unity package designed to streamline the build process for your Unity projects. It automates versioning, archiving, and build path management, making it easier to manage and distribute builds. This package is particularly useful for WebGL builds.

## Features

- Automatic version incrementing based on build type (Major, Feature, Bugfix, Build)
- Customizable build paths with default and custom options
- Automatic archiving of builds into zip files
- Simple initialization and configuration through a Unity Editor window

## Installation

To install the Unity Custom Build Updater package, follow these steps:

1. Open your Unity project.
2. Go to `Window > Package Manager`.
3. Click on the `+` button in the top left corner.
4. Select `Add package from git URL...`.
5. Paste the following URL: `https://github.com/RimuruDev/Unity-CustomBuildUpdater.git`
6. Click `Add`.

## Initialization

After installing the package, you need to initialize the build configuration. Follow these steps:

1. Go to `RimuruDev Tools > Initialize BuildConfig` in the top menu.
2. This will create a `BuildConfig` asset in `Assets/Resources/Editor/BuildConfig.asset`.

## Configuration

### BuildConfig

The `BuildConfig` asset contains the following settings:

- **Company Name**: The name of your company.
- **Product Name**: The name of your product.
- **Initial Version**: The starting version of your build (e.g., `1.0.0.0`).
- **Build Path Type**: The type of build path (`Default` or `Custom`).
- **Custom Build Path**: The custom build path if `Build Path Type` is set to `Custom`.
- **Archive Build**: Whether to archive the build into a zip file.
- **Version Type**: The type of version increment (`Major`, `Feature`, `Bugfix`, `Build`).
- **Version Pattern**: The pattern for the version naming (e.g., `com.{company}.{product}.v{version}`).

### Example

```csharp
using UnityEngine;

namespace RimuruDev.Unity_CustomBuildUpdater.CustomBuildUpdater.Editor
{
    [CreateAssetMenu(fileName = "BuildConfig", menuName = "Configs/Build/BuildConfig", order = 1)]
    public class BuildConfig : ScriptableObject
    {
        public string companyName = "AbyssMoth";
        public string productName = "SuperGame";
        public string initialVersion = "1.0.0.0";
        public BuildPathType buildPathType = BuildPathType.Default;
        public string customBuildPath = "Builds";
        public bool archiveBuild = true;
        public VersionType versionType = VersionType.Build;
        public string versionPattern = "com.{company}.{product}.v{version}";
    }
}
```

## Usage

### Updating the Version

To update the current version in the configuration:

1. Open the `BuildConfig` asset.
2. Click on the `Update Current Version` button.

### Building the Project

To build the project using the configured settings:

1. Go to `File > Build Settings`.
2. Configure your build settings as needed (make sure to select WebGL if testing WebGL builds).
3. Click on `Build` and select the folder for your build.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.
