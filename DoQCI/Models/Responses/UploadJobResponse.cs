namespace DoQCI.Models.Responses
{
    public class UploadJobResponse
    {
        public string JobId { get; set; } = String.Empty;
        public List<FileInfoResponse> Files { get; set; } = new();
    }
}
