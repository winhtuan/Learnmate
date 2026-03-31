using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace LearnmateSolution.Services;

public sealed class CloudinaryFileStorageService : IFileStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryFileStorageService(IOptions<CloudinarySettings> options)
    {
        var s = options.Value;
        var account = new Account(s.CloudName, s.ApiKey, s.ApiSecret);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadAsync(
        string objectPath,
        Stream content,
        string contentType,
        CancellationToken ct = default
    )
    {
        var fileName = Path.GetFileName(objectPath);
        var uploadParams = new RawUploadParams
        {
            File = new FileDescription(fileName, content),
            PublicId = objectPath,
            Overwrite = true
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.PublicId;
    }

    public async Task DeleteAsync(string objectPath, CancellationToken ct = default)
    {
        var deleteParams = new DeletionParams(objectPath)
        {
            ResourceType = ResourceType.Raw
        };
        await _cloudinary.DestroyAsync(deleteParams);
    }

    public Task<string> GetUrlAsync(
        string objectPath,
        int expirySeconds = 604800,
        CancellationToken ct = default
    )
    {
        var url = _cloudinary.Api.Url.Secure(true).ResourceType("raw").BuildUrl(objectPath);
        return Task.FromResult(url);
    }
}
