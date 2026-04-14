namespace DoQCI.Models.Requests;

public class ReorderPdfRequest
{
    public string FileName { get; set; } = String.Empty;

    public List<int> PageOrder { get; set; } = new();
}
