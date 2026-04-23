namespace DoQCI.Models.Requests;


public class FileUploadRequest
{
    public List<IFormFile> Files { get; set; } = new();
}