using System.Net;
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
    public async Task<IActionResult> Create([FromForm] IFormFile file, Guid id)
    {
        var response = await _customerImageService.UploadImageAsync(id, file);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}