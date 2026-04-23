namespace DoQCI.Services;

using DoQCI.Configuration;
using DoQCI.Helpers;
using DoQCI.Models.Requests;
using DoQCI.Models.Responses;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Options;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

public class PdfService : IPdfService
{

    private readonly StorageOptions _storage;
    private readonly HttpClient _httpClient;
    private readonly PythonServiceOptions _python;
    public string JobsFolder => Path.Combine(_storage.RootPath, "temp", "jobs");
    public string DownloadsFolder => Path.Combine(_storage.RootPath, "temp", "downloads");

    public PdfService(IOptions<StorageOptions> storageOptions,
                        IOptions<PythonServiceOptions> pythonOptions,
                        IHttpClientFactory httpClientFactory)
    {
        _storage = storageOptions.Value;
        _python = pythonOptions.Value;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<FileInfoResponse> UploadAsync(IFormFile file, string jobId, int index)
    {
        var fileName = $"file_{index}.pdf";

        var filesFolder = Path.Combine(JobsFolder, jobId, "files");

        Directory.CreateDirectory(filesFolder); 

        var filePath = Path.Combine(filesFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var pageCount = await CountPages(filePath);

        return new FileInfoResponse
        {
            FileIndex = index,
            FileName = fileName,
            PagesCount = pageCount
        };
    }
    public async Task<List<PageInfoResponse>> GenerateThumbnails(string jobId, int index)
    {
        var url = $"{_python.BaseUrl}/generate-thumbnails";

        var body = new
        {
            jobId = jobId,
            fileIndex = index
        };

        var response = await _httpClient.PostAsJsonAsync(url, body);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Python thumbnail service failed");

        var result = await response.Content.ReadFromJsonAsync<List<PageInfoResponse>>();

        return result ?? new List<PageInfoResponse>();
    }

    public async Task<int> CountPages(string filePath)
    {
        var doc = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
        return await Task.FromResult(doc.PageCount);

    }

    //public async Task<FileUploadResponse> ReorderAsync(ReorderPdfRequest reorderRequest)
    //{
    //    var inputPath = Path.Combine(FileHelper.UploadsFolder, reorderRequest.FileName);

    //    if (!File.Exists(inputPath))
    //        throw new Exception("File not found");

    //    var inputDocument = PdfReader.Open(inputPath, PdfDocumentOpenMode.Import);
    //    var outputDocument = new PdfDocument();

    //    foreach (var pageNumber in reorderRequest.PageOrder)
    //    {
    //        if (pageNumber < 1 || pageNumber > inputDocument.PageCount)
    //            continue;

    //        outputDocument.AddPage(inputDocument.Pages[pageNumber - 1]);
    //    }
        

    //    var outputName = FileHelper.GenerateDownloadFileName(reorderRequest.FileName);

    //    var outputPath = Path.Combine(FileHelper.DownloadsFolder, outputName);

    //    outputDocument.Save(outputPath);

    //    return new FileUploadResponse
    //    {
    //        FileName = outputName,
    //        Path = outputPath
    //    };
    //}


    //public async Task<MergeUploadResponse> UploadMergeAsync(MergeUploadRequest request)
    //{
    //    var mergeId = Guid.NewGuid().ToString();

    //    var mergeFolder = Path.Combine(FileHelper.MergeFolder, mergeId);

    //    if (!Directory.Exists(mergeFolder))
    //        Directory.CreateDirectory(mergeFolder);

    //    var response = new MergeUploadResponse
    //    {
    //        MergeId = mergeId
    //    };

    //    int index = 0;

    //    foreach (var file in request.Files)
    //    {
    //        var fileName = $"file_{index}.pdf";
    //        var filePath = Path.Combine(mergeFolder, fileName);

    //        using (var stream = new FileStream(filePath, FileMode.Create))
    //        {
    //            await file.CopyToAsync(stream);
    //        }

    //        var doc = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);

    //        response.Files.Add(new FileInfoResponse
    //        {
    //            FileName = fileName,
    //            PageCount = doc.PageCount
    //        });

    //        index++;
    //    }

    //    return response;
    //}
    public async Task<FileDownloadResponse> MergeAsync(MergeRequest mergeRequest)
    {
        if (mergeRequest.Pages == null || mergeRequest.Pages.Count == 0)
            throw new Exception("No pages to merge");

        if (string.IsNullOrEmpty(mergeRequest.MergeId))
            throw new Exception("MergeId is required");

        var mergeFolder = Path.Combine(FileHelper.MergeFolder, mergeRequest.MergeId);

        if (!Directory.Exists(mergeFolder))
            throw new Exception("Merge folder not found");

        var outputDocument = new PdfDocument();

        var openedDocuments = new Dictionary<string, PdfDocument>();

        foreach (var pageRequest in mergeRequest.Pages)
        {
            if (!openedDocuments.TryGetValue(pageRequest.FileIndex, out var doc))
            {
                var fileName = $"file_{pageRequest.FileIndex}.pdf";
                var filePath = Path.Combine(mergeFolder, fileName);

                if (!File.Exists(filePath))
                    throw new Exception($"File {fileName} not found");

                doc = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
                openedDocuments.Add(pageRequest.FileIndex, doc);
            }

            if (pageRequest.Page < 1 || pageRequest.Page > doc.PageCount)
                continue;

            outputDocument.AddPage(doc.Pages[pageRequest.Page - 1]);
        }

        var outputName = FileHelper.GenerateDownloadFileName(mergeRequest.MergeId);
        var outputPath = Path.Combine(FileHelper.DownloadsFolder, outputName);

        outputDocument.Save(outputPath);

        return new FileDownloadResponse
        {
            FileName = outputName,
            Path = outputPath
        };
    }



}