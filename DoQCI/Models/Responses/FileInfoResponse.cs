namespace DoQCI.Models.Responses;

public class FileInfoResponse
{
    public int FileIndex { get; set; }

    public string FileName { get; set; } 

    public int PagesCount { get; set; }

    public List<PageInfoResponse> Pages { get; set; } = new();
}