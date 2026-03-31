using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Settings;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace LearnmateSolution.Services;

public sealed class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _client;
    private readonly string _bucket;

    public MinioFileStorageService(IOptions<MinioSettings> options)
    {
        var s = options.Value;
        _bucket = s.BucketName;
        _client = new MinioClient()
            .WithEndpoint(s.Endpoint)
            .WithCredentials(s.AccessKey, s.SecretKey)
            .WithSSL(s.UseSSL)
            .Build();
    }

    public async Task<string> UploadAsync(
        string objectPath,
        Stream content,
        string contentType,
        CancellationToken ct = default
    )
    {
        await EnsureBucketAsync(ct);

        // Minio SDK requires a seekable stream with known length
        Stream uploadStream = content.CanSeek ? content : await ToMemoryStreamAsync(content, ct);

        var args = new PutObjectArgs()
            .WithBucket(_bucket)
            .WithObject(objectPath)
            .WithStreamData(uploadStream)
            .WithObjectSize(uploadStream.Length)
            .WithContentType(contentType);

        await _client.PutObjectAsync(args, ct);
        return objectPath;
    }

    /// <summary>MinIO has no image pipeline — upload as raw and return presigned URL.</summary>
    public async Task<string> UploadImageAsync(
        string folder,
        string publicId,
        Stream content,
        string contentType,
        CancellationToken ct = default)
    {
        var objectPath = $"{folder}/{publicId}";
        await UploadAsync(objectPath, content, contentType, ct);
        return await GetUrlAsync(objectPath, ct: ct);
    }

    public async Task DeleteAsync(string objectPath, CancellationToken ct = default)
    {
        var args = new RemoveObjectArgs().WithBucket(_bucket).WithObject(objectPath);

        await _client.RemoveObjectAsync(args, ct);
    }

    public async Task<string> GetUrlAsync(
        string objectPath,
        int expirySeconds = 604800,
        CancellationToken ct = default
    )
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(_bucket)
            .WithObject(objectPath)
            .WithExpiry(expirySeconds);

        return await _client.PresignedGetObjectAsync(args);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task EnsureBucketAsync(CancellationToken ct)
    {
        var exists = await _client.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucket),
            ct
        );

        if (!exists)
            await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucket), ct);
    }

    private static async Task<MemoryStream> ToMemoryStreamAsync(Stream source, CancellationToken ct)
    {
        var ms = new MemoryStream();
        await source.CopyToAsync(ms, ct);
        ms.Position = 0;
        return ms;
    }
}
