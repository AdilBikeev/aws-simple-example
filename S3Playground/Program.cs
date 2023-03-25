// See https://aka.ms/new-console-template for more information

using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

const string bucketName = "fooawsbucket";
const string key = "files/foo.txt";
const string contentType = "text/plain";

var client = new AmazonS3Client(RegionEndpoint.USEast1);

// Put
// await using var stream = new FileStream("./foo.txt", FileMode.Open, FileAccess.Read);
// client = new AmazonS3Client(RegionEndpoint.USEast1);
// await client.PutObjectAsync(new PutObjectRequest()
// {
//     BucketName = bucketName,
//     Key = key,
//     ContentType = contentType,
//     InputStream = stream
// });

// Get
var response = await client.GetObjectAsync(new GetObjectRequest()
{
    BucketName = bucketName,
    Key = key,
});
using var memoryStream = new MemoryStream();
response.ResponseStream.CopyTo(memoryStream);

var txt = Encoding.Default.GetString(memoryStream.ToArray());
Console.Write(txt);
