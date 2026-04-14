namespace DoQCI.Helpers;

public static class FileHelper
{
    public static string TempFolder =>
        Path.Combine(Directory.GetCurrentDirectory(), "Storage", "temp");

    public static string UploadsFolder =>
        Path.Combine(TempFolder, "uploads");

    public static string DownloadsFolder =>
        Path.Combine(TempFolder, "downloads");
    public static string MergeFolder =>
        Path.Combine(TempFolder, "merge");

    public static void EnsureTempFolders()
    {
        if (!Directory.Exists(TempFolder))
            Directory.CreateDirectory(TempFolder);

        if (!Directory.Exists(UploadsFolder))
            Directory.CreateDirectory(UploadsFolder);

        if (!Directory.Exists(DownloadsFolder))
            Directory.CreateDirectory(DownloadsFolder);

        if (!Directory.Exists(MergeFolder))
            Directory.CreateDirectory(MergeFolder);
    }

    public static string GenerateUploadFileName(string originalName)
    {
        var extension = Path.GetExtension(originalName);
        return $"{Guid.NewGuid()}{extension}";
    }

    public static string GenerateDownloadFileName(string originalName)
    {
        var dateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
        var guidChunk = originalName.Substring(0, 8);

        return $"DoQCI_{dateTimeStamp}_{guidChunk}.pdf";
    }
}