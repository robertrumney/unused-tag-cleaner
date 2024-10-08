# Tag Usage Finder

The "Tag Usage Finder" is a Unity Editor tool designed to provide a detailed analysis of specific tag usage within a Unity project. It serves as a supplementary tool alongside Unused Tag Cleaner to ensure that no leftover tags are being used without your knowledge, helping to maintain clean and optimized project management.

## Features

- **Specific Tag Search**: Allows users to enter a specific tag name and find all occurrences of its usage across both scenes and scripts.
- **Comprehensive Analysis**: Scans all enabled scenes and C# script files for the entered tag, providing a thorough review of where and how each tag is used.
- **Scrollable Results Display**: Outputs the search results in a scrollable list within the Unity Editor window, making it easy to navigate through even extensive lists of tag usage.
- **Supplementary Usage**: Ideal for use as a bonus pass to analyze tags that may be leftover from previous versions of the project or from merged branches, ensuring all tags are accounted for and utilized properly.

## Installation

1. Clone this repository or download the latest release.
2. Copy the `TagUsageFinder.cs` script into the `Assets/Editor` folder within your Unity project. If the folder doesn't exist, create it.

## Usage

1. Open your Unity project.
2. Navigate to `Tools > Tag Usage Finder` from the Unity menu bar to open the Tag Usage Finder window.
3. Enter the tag name you wish to investigate in the "Tag Name:" text field.
4. Click on "Find Tag Usage" to initiate the scanning process.
5. Review the results displayed in the scrollable list within the window. Locations will include both scene paths and script files where the tag is used.

## Support

If you encounter any issues or have suggestions for improvements, please open an issue in this GitHub repository.
