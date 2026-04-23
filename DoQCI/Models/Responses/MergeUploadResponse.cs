namespace DoQCI.Models.Responses;


public class MergeUploadResponse
{
    public string MergeId { get; set; } = String.Empty;

    public List<FileInfoResponse> Files { get; set; } = new();
}

