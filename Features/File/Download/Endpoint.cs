using FastEndpoints;
using Minio;
using Minio.DataModel.Args;

namespace vegetarian.Features.File.Download
{
    public class DownloadEndpoint(IMinioClient minio) : EndpointWithoutRequest
    {
        private readonly IMinioClient _minio = minio;

        public override void Configure()
        {
            Get("/files/download/{file}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var bucketName = "vegetarian";
            var fileName = Route<string>("file");

            var memoryStream = new MemoryStream();

            await _minio.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                }), ct);

            memoryStream.Position = 0;
            await SendStreamAsync(memoryStream, fileName, cancellation: ct);
        }
    }
}