using FluentAssertions;
using Snapshooter;

namespace Nzr.Snapshot.Xunit.Extensions.Tests;

/// <summary>
/// Contains tests for the <see cref="SnapshotExtensions"/> class, verifying its behavior
/// with and without the <see cref="SnapshotFolderAttribute"/>. The tests ensure that
/// snapshots are stored in the appropriate folder based on the usage of the attribute.
/// </summary>
public class SnapshotExtensionsTests
{
    /// <summary>
    /// Verifies that when the <see cref="SnapshotFolderAttribute"/> is not applied,
    /// snapshots are stored in the default folder. This demonstrates the default behavior
    /// of the <see cref="SnapshotExtensions.Match"/> method.
    /// </summary>
    /// <param name="snapshotName">The custom snapshot name, if provided.</param>
    /// <param name="matchOptions">Optional match options to customize the snapshot comparison.</param>
    [Theory]
    [MemberData(nameof(MatchTestData))]
    public void Match_Without_SnapshotFolderAttribute_Should_Snapshot_Folder_In_The_Same_Folder(
        string? snapshotName, Func<MatchOptions, MatchOptions>? matchOptions)
    {
        // Arrange
        var currentResult = new { City = "Nova Lima", CreatedAt = DateTimeOffset.Now };

        // Assert
        currentResult.Should().Match(snapshotName, matchOptions: matchOptions);
    }

    /// <summary>
    /// Verifies that when the <see cref="SnapshotFolderAttribute"/> is applied with a folder name,
    /// snapshots are stored in the specified folder. This demonstrates the customization capability
    /// provided by the <see cref="SnapshotFolderAttribute"/>.
    /// </summary>
    /// <param name="snapshotName">The custom snapshot name, if provided.</param>
    /// <param name="matchOptions">Optional match options to customize the snapshot comparison.</param>
    [SnapshotFolder("SnapshotExtensions")]
    [Theory]
    [MemberData(nameof(MatchTestData))]
    public void Match_With_SnapshotFolderAttribute_Should_Snapshot_Folder_In_Specified_Folder(
        string? snapshotName, Func<MatchOptions, MatchOptions>? matchOptions)
    {
        // Arrange
        var currentResult = new { City = "Lisbon", CreatedAt = DateTimeOffset.Now };

        // Assert
        currentResult.Should().Match(snapshotName, matchOptions: matchOptions);
    }

    /// <summary>
    /// Verifies that when the <see cref="SnapshotFolderAttribute"/> is applied with a folder name
    /// and the <c>excludeClassName</c> parameter set to <c>false</c>, snapshots are stored
    /// in the specified folder, and the class name is included in the snapshot file name.
    /// </summary>
    /// <param name="snapshotName">The custom snapshot name, if provided.</param>
    /// <param name="matchOptions">Optional match options to customize the snapshot comparison.</param>
    [SnapshotFolder("Shared", excludeClassName: false)]
    [Theory]
    [MemberData(nameof(MatchTestData))]
    public void Match_With_SnapshotFolderAttribute_Keeping_ClassName_Should_Snapshot_Folder_In_Specified_Folder(
        string? snapshotName, Func<MatchOptions, MatchOptions>? matchOptions)
    {
        // Arrange
        var currentResult = new { City = "Berlin", CreatedAt = DateTimeOffset.Now };

        // Assert
        currentResult.Should().Match(snapshotName, matchOptions: matchOptions);
    }

    /// <summary>
    /// Provides test data for the snapshot tests, including different combinations
    /// of snapshot names and match options.
    /// </summary>
    public static TheoryData<string?, Func<MatchOptions, MatchOptions>?> MatchTestData =>
        new()
        {
            { null, o => o.IgnoreField("CreatedAt") },
            { "custom_file_name1", o => o.IgnoreField("CreatedAt") },
            { "custom file name2", o => o.IgnoreField("CreatedAt") },
        };
}
