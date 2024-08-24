namespace Nzr.Snapshot.Xunit.Extensions;

/// <summary>
/// Specifies metadata for organizing snapshots by placing them into designated folders.
/// This attribute allows you to group snapshots based on a specific folder name and control
/// whether the class name is included in the snapshot file naming convention.
/// </summary>

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class SnapshotFolderAttribute : Attribute
{
    /// <summary>
    /// Creates a new instance of the <see cref="SnapshotFolderAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the folder where snapshots will be saved. This value is required and should uniquely identify the folder.</param>
    /// <param name="excludeClassName">Indicates whether the class name should be excluded from snapshot file names.
    /// If <c>true</c>, only the method name and other metadata will determine the file name.</param>
    public SnapshotFolderAttribute(string name, bool excludeClassName = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        ExcludeClassName = excludeClassName;
    }

    /// <summary>
    /// The name of the snapshot folder.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Indicates whether the class name should be excluded from the snapshot file name.
    /// </summary>
    public bool ExcludeClassName { get; }

    /// <summary>
    /// Returns a formatted string representation of the attribute for debugging purposes.
    /// </summary>
    public override string ToString()
    {
        return $"SnapshotFolder: {Name}, ExcludeClassName: {ExcludeClassName}";
    }

}
