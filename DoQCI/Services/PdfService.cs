namespace DoQCI.Services;

using DoQCI.Helpers;
using DoQCI.Models.Requests;
using DoQCI.Models.Responses;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

public class PdfService : IPdfService
{
    public async Task<FileUploadResponse> UploadAsync(IFormFile file)
    {
        FileHelper.EnsureTempFolders();

        var fileName = FileHelper.GenerateUploadFileName(file.FileName);
        var filePath = Path.Combine(FileHelper.UploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
   
        var pageCount = await CountPages(filePath);

        return new FileUploadResponse
        {
            FileName = fileName,
            Path = filePath,
            PageCount = pageCount
        };
    }

    public async Task<int> CountPages(string filePath)
    {
        var doc = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
        return await Task.FromResult(doc.PageCount);

    }

    public async Task<FileUploadResponse> ReorderAsync(ReorderPdfRequest reorderRequest)
    {
        var inputPath = Path.Combine(FileHelper.UploadsFolder, reorderRequest.FileName);

        if (!File.Exists(inputPath))
            throw new Exception("File not found");

        var inputDocument = PdfReader.Open(inputPath, PdfDocumentOpenMode.Import);
        var outputDocument = new PdfDocument();

        foreach (var pageNumber in reorderRequest.PageOrder)
        {
            if (pageNumber < 1 || pageNumber > inputDocument.PageCount)
                continue;

            outputDocument.AddPage(inputDocument.Pages[pageNumber - 1]);
        }
        

        var outputName = FileHelper.GenerateDownloadFileName(reorderRequest.FileName);

        var outputPath = Path.Combine(FileHelper.DownloadsFolder, outputName);

        outputDocument.Save(outputPath);

        return new FileUploadResponse
        {
            FileName = outputName,
            Path = outputPath
        };
    }


    public async Task<MergeUploadResponse> UploadMergeAsync(MergeUploadRequest request)
    {
        var mergeId = Guid.NewGuid().ToString();

        var mergeFolder = Path.Combine(FileHelper.MergeFolder, mergeId);

        if (!Directory.Exists(mergeFolder))
            Directory.CreateDirectory(mergeFolder);

        var response = new MergeUploadResponse
        {
            MergeId = mergeId
        };

        int index = 0;

        foreach (var file in request.Files)
        {
            var fileName = $"file_{index}.pdf";
            var filePath = Path.Combine(mergeFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var doc = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);

            response.Files.Add(new FileInfoResponse
            {
                FileName = fileName,
                PageCount = doc.PageCount
            });

            index++;
        }

        return response;
    }
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