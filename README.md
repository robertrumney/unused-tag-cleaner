# Unused Tag Cleaner

UnusedTagCleaner is a Unity Editor tool designed to help streamline your project by identifying and removing unused tags from the Tag Manager across all scenes in your Unity project. This tool is ideal for maintaining a clean and efficient project structure, especially useful in larger projects with multiple contributors.

## Features

- **Scan All Scenes**: Quickly scan all enabled scenes in the project to determine which tags are actually in use.
- **Identify Unused Tags**: Lists all tags that are not used in any of the scanned scenes.
- **Remove Unused Tags**: Provides an option to delete all unused tags with a single click, keeping your Tag Manager clean and relevant.

## Installation

1. Clone this repository or download the latest release.
2. Copy the `UnusedTagCleaner.cs` script into the `Assets/Editor` folder within your Unity project. If the folder doesn't exist, create it.

## Usage

1. Open your Unity project.
2. Navigate to `Tools > Unused Tags Remover` in the Unity menu bar to open the UnusedTagCleaner window.
3. Click on `Scan for Unused Tags` to start the scanning process. The tool will analyze all enabled scenes and compile a list of unused tags.
4. Review the list of unused tags displayed in the tool window.
5. If you decide to clean up, click on `Remove All Unused Tags` to delete them from the project.

## Contribution

Contributions to UnusedTagCleaner are welcome. Please feel free to fork the repository, make changes, and submit a pull request with your improvements.

## License

Distributed under the MIT License. See `LICENSE` file for more information.

## Support

If you encounter any issues or have suggestions for improvements, please open an issue in this GitHub repository
