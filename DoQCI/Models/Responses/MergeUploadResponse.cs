namespace DoQCI.Models.Responses;


public class MergeUploadResponse
{
    public string MergeId { get; set; } = String.Empty;

    public List<FileInfoResponse> Files { get; set; } = new();
}

public class FileInfoResponse
{
    public string FileName { get; set; } = String.Empty;

    public int PageCount { get; set; }
}
