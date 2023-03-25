using System.Net;
using Amazon.S3;
using Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers;

[ApiController]
public class CustomerImageController : ControllerBase
{
    private readonly ICustomerImageService _customerImageService;

    public CustomerImageController(ICustomerImageService customerImageService)
    {
        _customerImageService = customerImageService;
    }

    [HttpPost("customers/{id:guid}/image")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, Guid id)
    {
        var response = await _customerImageService.UploadImageAsync(id, file);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpGet("customers/{id:guid}/image")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var response = await _customerImageService.GetImageAsync(id);
            return File(response.ResponseStream, response.Headers.ContentType);
        }
        catch (AmazonS3Exception e)
        {
            return NotFound();
        }
    }
}