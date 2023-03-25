using Amazon.S3.Model;

namespace Customers.Api.Services;

public interface ICustomerImageService
{
    public Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file);
    public Task<GetObjectResponse> GetImageAsync(Guid id);
}