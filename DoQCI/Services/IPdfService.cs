using DoQCI.Models.Requests;
using DoQCI.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace DoQCI.Services;

public interface IPdfService
{
    Task<FileUploadResponse> UploadAsync(IFormFile file);
    Task<FileUploadResponse> ReorderAsync(ReorderPdfRequest reorderRequest);
    Task<int> CountPages(string filePath);
}


