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



}