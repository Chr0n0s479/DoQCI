namespace DoQCI.Models.Responses;

public class FileUploadResponse
{
    public string FileName { get; set; } = "";
    public string Path { get; set; } = "";
    public int PageCount { get; set; } = 0;
}
