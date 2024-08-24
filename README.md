
# Nzr.Snapshot.Xunit.Extensions

[![License](https://img.shields.io/github/license/marionzr/nzr.snapshot.xunit.extensions)](https://github.com/marionzr/nzr.snapshot.xunit.extensions/blob/main/docs/LICENSE.txt)

`Nzr.Snapshot.Xunit.Extensions` is a simple extension library for integrating [Snapshooter](https://github.com/TakeScoop/Snapshooter) with [xUnit](https://xunit.net/). It enables custom folder organization for snapshots based on test attributes and allows for flexible snapshot management in unit tests.

## Features

- **Custom Folder Organization**: Automatically organize snapshots into custom folders based on the `SnapshotFolder` attribute applied to xUnit test methods.
- **Class Name Exclusion**: Optionally exclude the class name from the snapshot file path.


## Installation

You can install `Nzr.Snapshot.Xunit.Extensions` via NuGet Package Manager:

```bash
dotnet add package Nzr.Snapshot.Xunit.Extensions
```

Or, via the [NuGet package page](https://www.nuget.org/packages/Nzr.Snapshot.Xunit.Extensions).

## Usage

To use this package, simply include it in your xUnit test project and apply the extension methods to match the snapshots of objects during tests.

### Example Usage

```csharp
using FluentAssertions;
using Snapshooter;
using Nzr.Snapshot.Xunit.Extensions;

public class SnapshotTests
{
    [Fact]
    public void Match_Without_SnapshotFolderAttribute_Should_Snapshot_Folder_In_The_Same_Folder()
    {
        // Arrange
        var currentResult = new { City = "Nova Lima", CreatedAt = DateTimeOffset.Now };

        // Assert
        currentResult.Should().Match();
    }

    [SnapshotFolder("SnapshotExtensions")]
    [Fact]
    public void Match_With_SnapshotFolderAttribute_Should_Snapshot_Folder_In_Specified_Folder()
    {
        // Arrange
        var currentResult = new { City = "Lisbon", CreatedAt = DateTimeOffset.Now };

        // Assert
        currentResult.Should().Match();
    }

    [SnapshotFolder("Shared", excludeClassName: false)]
    [Fact]
    public void Match_With_SnapshotFolderAttribute_Keeping_ClassName_Should_Snapshot_Folder_In_Specified_Folder()
    {
        // Arrange
        var currentResult = new { City = "Berlin", CreatedAt = DateTimeOffset.Now };

        // Assert
        currentResult.Should().Match();
    }
}
```

### SnapshotFolderAttribute

The `SnapshotFolderAttribute` is used to define custom folder paths for your snapshots.
You can optionally specify whether to exclude the class name from the snapshot file name.

#### Parameters:
- `Name`: Specifies the folder name for snapshots.
- `ExcludeClassName`: Optional boolean to exclude the class name from the snapshot filename.

```csharp
[SnapshotFolder("FolderName", excludeClassName: true)]
```

## License

This project is licensed under the MIT License - see the [LICENSE.txt](docs/LICENSE.txt) file for details.

## Contributing

We welcome contributions! If you'd like to contribute to this project, please fork the repository and submit a pull request. Please ensure that your code passes all tests and includes relevant documentation for new features or changes.

### Steps for Contribution

1. Give a start for this repository!
2. Fork the repository.
3. Clone the forked repository to your local machine.
4. Create a new branch.
5. Implement your changes.
6. Run tests to ensure everything works.
7. Commit your changes and create a pull request.

## Acknowledgements

- [Snapshooter](https://github.com/Snapshooter/Snapshooter) for snapshot testing.
- [xUnit](https://xunit.net/) for providing a great testing framework.
- The open-source community for their contributions and feedback.

---

For more information on how to use this project, please refer to the [documentation](https://github.com/marionzr/nzr.snapshot.xunit.extensions).
