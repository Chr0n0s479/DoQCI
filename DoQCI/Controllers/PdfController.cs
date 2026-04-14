namespace DoQCI.Controllers;

using DoQCI.Services;
using Microsoft.AspNetCore.Mvc;
using DoQCI.Models.Requests;

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
        if (request.File == null || request.File.Length == 0)
            return BadRequest("File is required");

        var result = await _pdfService.UploadAsync(request.File);

        return Ok(result);
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

        var result = await _pdfService.ReorderAsync(reorderDto);

        return Ok(result);
    }



}
