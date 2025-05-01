using FastEndpoints;
using Minio;
using Minio.DataModel.Args;

namespace vegetarian.Features.File.Upload;

public class FileUploadRequest
{
    public IFormFile File { get; set; } = default!;
}

public class UploadEndpoint : Endpoint<FileUploadRequest>
{
    private readonly IMinioClient _minio;

    public UploadEndpoint(IMinioClient minio)
    {
        _minio = minio;
    }

    public override void Configure()
    {
        Post("/files/upload");
        AllowAnonymous();
        AllowFileUploads();
    }

    public override async Task HandleAsync(FileUploadRequest req, CancellationToken ct)
    {
        var bucketName = "vegetarian";

        var exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct);
        if (!exists)
            await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), ct);

        var file = req.File;
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        await using var stream = file.OpenReadStream();

        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType), ct);

        await SendAsync(new { message = "Uploaded", fileName }, cancellation: ct);
    }
}