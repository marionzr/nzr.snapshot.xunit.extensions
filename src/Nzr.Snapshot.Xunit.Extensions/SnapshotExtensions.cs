using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Snapshooter;
using Snapshooter.Xunit;
using Xunit;

namespace Nzr.Snapshot.Xunit.Extensions;

/// <summary>
/// Provides extension methods for integrating Snapshooter with xUnit tests,
/// enabling custom folder organization and field exclusion for snapshots.
/// This is particularly useful for managing complex test cases with structured
/// snapshot storage and customization.
/// </summary>
public static class SnapshotExtensions
{
    private static readonly Regex _snapshotNameCleanupRegex = new("[^A-Za-z0-9=.]+", RegexOptions.Compiled);

    /// <summary>
    /// Matches the snapshot of the given <paramref name="result"/> object with optional label, folder, and field exclusions.
    /// </summary>
    /// <param name="result">The object whose snapshot is being matched.</param>
    /// <param name="snapshotName">An optional name to distinguish the snapshot.</param>
    /// <param name="matchOptions">An optional function to configure the snapshot matching options.</param>
    public static void Match(this object result, string? snapshotName = null, Func<MatchOptions, MatchOptions>? matchOptions = null)
    {
        snapshotName = snapshotName != null ? _snapshotNameCleanupRegex.Replace(snapshotName, "_").TrimEnd('_') : null;

        var testMethod = GetCurrentTestMethod();
        var snapshotFolderAttribute = testMethod.GetCustomAttribute<SnapshotFolderAttribute>();

        var snapshotFullName = string.IsNullOrWhiteSpace(snapshotName) ?
            Snapshooter.Xunit.Snapshot.FullName() : Snapshooter.Xunit.Snapshot.FullName(snapshotName);

        snapshotFullName = ApplySnapshotFolder(snapshotFullName, testMethod, snapshotFolderAttribute);

        result.MatchSnapshot(snapshotFullName, matchOptions);
    }

    private static SnapshotFullName ApplySnapshotFolder(SnapshotFullName snapshotFullName, MethodBase testMethod, SnapshotFolderAttribute? snapshotFolderAttribute)
    {
        if (snapshotFolderAttribute == null || string.IsNullOrWhiteSpace(snapshotFolderAttribute.Name))
        {
            return snapshotFullName;
        }

        var folderPath = $"{snapshotFullName.FolderPath}{Path.DirectorySeparatorChar}{snapshotFolderAttribute.Name}";

        if (snapshotFolderAttribute.ExcludeClassName)
        {
            var filenameWithoutClass = GetFileNameWithoutClassName(testMethod, snapshotFullName.Filename);
            return new SnapshotFullName(filenameWithoutClass, folderPath);
        }

        return new SnapshotFullName(snapshotFullName.Filename, folderPath);
    }

    /// <summary>
    /// Finds the currently executing xUnit test method in the call stack.
    /// </summary>
    /// <returns>The MethodBase of the current test method, or null if not found.</returns>
    private static MethodBase GetCurrentTestMethod()
    {
        var stackFrames = new StackTrace(true).GetFrames()!;

        foreach (var stackFrame in stackFrames)
        {
            var method = stackFrame.GetMethod();

            if (IsXunitTestMethod(method))
            {
                return method!;
            }

            var asyncMethod = EvaluateAsynchronousMethodBase(method);
            if (IsXunitTestMethod(asyncMethod))
            {
                return asyncMethod!;
            }
        }

        throw new InvalidOperationException("Could not find the current xUnit test method.");
    }

    /// <summary>
    /// Checks if the given method is an xUnit test method (Fact or Theory).
    /// </summary>
    /// <param name="method">The method to check.</param>
    /// <returns>True if the method is an xUnit test method; otherwise, false.</returns>
    private static bool IsXunitTestMethod(MemberInfo? method)
    {
        return method != null &&
               (method.GetCustomAttributes(typeof(FactAttribute), true).Length != 0 ||
                method.GetCustomAttributes(typeof(TheoryAttribute), true).Length != 0);
    }

    /// <summary>
    /// Evaluates the asynchronous method base for state machine methods.
    /// </summary>
    /// <param name="method">The method to evaluate.</param>
    /// <returns>The actual method base, if found.</returns>
    private static MethodInfo? EvaluateAsynchronousMethodBase(MethodBase? method)
    {
        if (method?.DeclaringType == null)
        {
            return null;
        }

        var declaringType = method.DeclaringType;
        var parentType = declaringType.DeclaringType;

        if (parentType == null)
        {
            return null;
        }

        var methods = parentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).ToList();
        var methodInfo = methods.Find(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == declaringType);

        return methodInfo;
    }

    /// <summary>
    /// Removes the class name prefix from the snapshot file name if present.
    /// </summary>
    /// <param name="testMethod">The current test method.</param>
    /// <param name="filename">The original snapshot file name.</param>
    /// <returns>The snapshot file name without the class name prefix.</returns>
    private static string GetFileNameWithoutClassName(MethodBase testMethod, string filename)
    {
        var className = testMethod.DeclaringType!.Name;

        if (!string.IsNullOrWhiteSpace(className) && filename.StartsWith(className))
        {
            return filename.Substring(className.Length + 1); // Remove class name and trailing dot
        }

        return filename;
    }
}
