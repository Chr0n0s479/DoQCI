namespace DoQCI.Controllers;

using DoQCI.Helpers;
using DoQCI.Models.Requests;
using DoQCI.Models.Responses;
using DoQCI.Services;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf.IO;

[ApiController]
[Route("api/pdf")]
public class PdfController : ControllerBase
{
    private readonly IPdfService _pdfService;

    public PdfController(IPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(15728640)]
    public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
    {
        if (request.Files == null || request.Files.Count == 0)
            return BadRequest("Files are required");

        var results = new List<FileInfoResponse>();

        var jobId = Guid.NewGuid().ToString();

        int index = 0;
        foreach (var file in request.Files)
        {
            var result = await _pdfService.UploadAsync(file, jobId, index);
            result.Pages = await _pdfService.GenerateThumbnails(jobId, index);
            results.Add(result);
            index++;
        }

        return Ok(new UploadJobResponse(){
            JobId = jobId,
            Files = results
        });

    }

    [HttpPost("reorder")]
    public async Task<IActionResult> Reorder([FromBody] ReorderPdfRequest request)
    {
        if (string.IsNullOrEmpty(request.FileName))
            return BadRequest("FileName required");

        if (request.PageOrder == null || request.PageOrder.Count == 0)
            return BadRequest("Page order required");

        var reorderDto = new ReorderPdfRequest
        {
            FileName = request.FileName,
            PageOrder = request.PageOrder
        };

        //var result = await _pdfService.ReorderAsync(reorderDto);
        var result = new object();
        return Ok(result);
    }

    [HttpPost("upload-merge")]
    [RequestSizeLimit(15728640)]
    public async Task<IActionResult> UploadMerge([FromForm] MergeUploadRequest request)
    {
        if (request.Files == null || request.Files.Count < 2)
            return BadRequest("At least two files are required");

        //var response = await _pdfService.UploadMergeAsync(request);

        var response = new object();
        return Ok(response);
    }

    [HttpPost("merge")]
    public async Task<IActionResult> MergeAsync([FromBody] MergeRequest request)
    {
        if (string.IsNullOrEmpty(request.MergeId))
            return BadRequest("MergeId is required");

        if (request.Pages == null || request.Pages.Count == 0)
            return BadRequest("Pages are required");

        var mergeFolder = Path.Combine(FileHelper.MergeFolder, request.MergeId);

        if (!Directory.Exists(mergeFolder))
            return BadRequest("Invalid MergeId");

        var mergeDto = new MergeRequest
        {
            MergeId = request.MergeId,
            Pages = request.Pages
        };
        var response = await _pdfService.MergeAsync(mergeDto);
        return Ok(response);
    }


}
