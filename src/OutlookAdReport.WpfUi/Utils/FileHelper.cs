using System.IO;

namespace OutlookAdReport.WpfUi.Utils;

/// <summary> A file helper.</summary>
public static class FileHelper
{
    /// <summary> Queries if a given file exists.</summary>
    /// <param name="fileInfo">            Information describing the file. </param>
    /// <param name="millisecondsTimeout"> The milliseconds timeout. </param>
    /// <returns> True if it succeeds, false if it fails.</returns>
    public static bool FileExists(this FileInfo fileInfo, int millisecondsTimeout)
    {
        var task = new Task<bool>(() => fileInfo.Exists);
        task.Start();
        return task.Wait(millisecondsTimeout) && task.Result;
    }
}