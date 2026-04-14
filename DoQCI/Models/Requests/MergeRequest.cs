namespace DoQCI.Models.Requests
{
    public class MergeRequest
    {
        public string MergeId { get; set; } = string.Empty;
        public List<FilePage> Pages { get; set; } = new();
    }

    public class FilePage
    {
        public string FileIndex { get; set; } = string.Empty;
        public int Page { get; set; }
    }
}
