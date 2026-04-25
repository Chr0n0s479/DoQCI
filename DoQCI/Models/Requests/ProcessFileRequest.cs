using System.Net;

namespace DoQCI.Models.Requests
{
    public class ProcessFileRequest
    {
        public string JobId { get; set; } = string.Empty;
        public ProcessOptions Options { get; set; } = new();
        public List<Pages> Pages { get; set; } = new();

    }

    public class ProcessOptions
    {
        public bool compress { get; set; } = false;
        public bool ocr { get; set; } = false;
        public bool enhance { get; set; } = false;

    }
    public class Pages
    {
        public int FileIndex { get; set; }
        public int PageNumber { get; set; }
    }
}
