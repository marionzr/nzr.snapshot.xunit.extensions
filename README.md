
# Nzr.Snapshot.Xunit.Extensions

![NuGet Version](https://img.shields.io/nuget/v/Nzr.Snapshot.Xunit.Extensions?link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FNzr.ToolBox.Core.Single)
![NuGet Downloads](https://img.shields.io/nuget/dt/Nzr.Snapshot.Xunit.Extensions?logoColor=red)
![GitHub last commit](https://img.shields.io/github/last-commit/marionzr/nzr.snapshot.xunit.extensions)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/marionzr/nzr.snapshot.xunit.extensions/ci.yaml)
![GitHub License](https://img.shields.io/github/license/marionzr/nzr.snapshot.xunit.extensions)

`Nzr.Snapshot.Xunit.Extensions` is a simple extension library for integrating [Snapshooter](https://github.com/TakeScoop/Snapshooter) with [xUnit](https://xunit.net/). It enables custom folder organization for snapshots based on test attributes and allows for flexible snapshot management in unit tests.

## Features

- **Custom Folder Organization**: Automatically organize snapshots into custom folders based on the `SnapshotFolder` attribute applied to xUnit test methods.
- **Class Name Exclusion**: Optionally exclude the class name from the snapshot file path.


## Installation

You can install `Nzr.Snapshot.Xunit.Extensions` via NuGet Package Manager:

```
Install-Package Nzr.Snapshot.Xunit.Extensions
```

```bash
dotnet add package Nzr.Snapshot.Xunit.Extensions
```

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

## License
Nzr.Snapshot.Xunit.Extensions is licensed under the Apache License, Version 2.0, January 2004. You may obtain a copy of the License at:

```
http://www.apache.org/licenses/LICENSE-2.0
```

# Disclaimer

This project is provided "as-is" without any warranty or guarantee of its functionality. The author assumes no responsibility or liability for any issues, damages, or consequences arising from the use of this code, whether direct or indirect. By using this project, you agree that you are solely responsible for any risks associated with its use, and you will not hold the author accountable for any loss, injury, or legal ramifications that may occur.

Please ensure that you understand the code and test it thoroughly before using it in any production environment.
