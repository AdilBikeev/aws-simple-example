using Amazon.S3;
using Amazon.S3.Model;

namespace Customers.Api.Services;

public class CustomerImageService : ICustomerImageService
{
    const string bucketName = "fooawsbucket";
    private readonly IAmazonS3 _amazonS3;

    public CustomerImageService(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }
    
    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
    {
        var response = await _amazonS3.PutObjectAsync(new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = $"images/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
            Metadata =
            {
                ["x-amz-originalname"] = file.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(file.FileName),
            }
        });

        return response;
    }

    public async Task<GetObjectResponse> GetImageAsync(Guid id)
    {
        var response = await _amazonS3.GetObjectAsync(new GetObjectRequest()
        {
            BucketName = bucketName,
            Key = $"images/{id}"
        });

        return response;
    }
}