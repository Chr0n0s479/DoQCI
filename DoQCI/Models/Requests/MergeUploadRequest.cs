namespace DoQCI.Models.Requests;

public class MergeUploadRequest
{
    public List<IFormFile> Files { get; set; } = new();
}
