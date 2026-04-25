using DoQCI.Models.Requests;
using DoQCI.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace DoQCI.Services;

public interface IPdfService
{
    Task<FileInfoResponse> UploadAsync(IFormFile file, string jobId, int index);
    //Task<FileInfoResponse> ReorderAsync(ReorderPdfRequest reorderRequest);
    Task<int> CountPages(string filePath);
    //Task<MergeUploadResponse> UploadMergeAsync(MergeUploadRequest request);
    Task<FileDownloadResponse> MergeAsync(MergeRequest mergeRequest);
    Task<List<PageInfoResponse>> GenerateThumbnails(string jobId, int index);
    Task<FileDownloadResponse> ProcessAsync(ProcessFileRequest fileRequest);
}


