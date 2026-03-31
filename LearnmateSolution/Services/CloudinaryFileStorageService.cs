using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace LearnmateSolution.Services;

/// <summary>
/// IFileStorageService implementation using Cloudinary.
/// Supports PDF, Word, images, and other file types via Cloudinary's "raw" upload.
/// </summary>
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
        CancellationToken ct = default)
    {
        bool isImage = contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
                    || objectPath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                    || objectPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                    || objectPath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase);

        if (isImage)
        {
            var imgParams = new ImageUploadParams
            {
                File = new FileDescription(objectPath, content),
                PublicId = objectPath.Contains('.') ? objectPath.Substring(0, objectPath.LastIndexOf('.')) : objectPath, 
                Overwrite = true
            };
            var imgResult = await _cloudinary.UploadAsync(imgParams);
            if (imgResult.Error != null) throw new InvalidOperationException($"Cloudinary upload failed: {imgResult.Error.Message}");
            return imgResult.SecureUrl.ToString();
        }
        else
        {
            var rawParams = new RawUploadParams
            {
                File = new FileDescription(objectPath, content),
                PublicId = objectPath,
                Overwrite = true
            };
            var rawResult = await _cloudinary.UploadAsync(rawParams);
            if (rawResult.Error != null) throw new InvalidOperationException($"Cloudinary upload failed: {rawResult.Error.Message}");
            return objectPath;
        }
    }

    public async Task<string> UploadImageAsync(
        string folder,
        string publicId,
        Stream content,
        string contentType,
        CancellationToken ct = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File        = new FileDescription(publicId, content),
            PublicId    = publicId,
            Folder      = folder,
            Overwrite   = true,
            // Auto-crop & format for avatars: square 400×400, WebP output
            Transformation = new Transformation()
                .Width(400).Height(400).Crop("fill").Gravity("face")
                .FetchFormat("auto").Quality("auto")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new InvalidOperationException($"Cloudinary image upload failed: {result.Error.Message}");

        // SecureUrl = permanent HTTPS CDN URL — no presign needed
        return result.SecureUrl.ToString();
    }

    public async Task DeleteAsync(string objectPath, CancellationToken ct = default)
    {
        // For raw files, the publicId to delete MUST include the extension
        var deleteParams = new DeletionParams(objectPath)
        {
            ResourceType = ResourceType.Raw
        };

        await _cloudinary.DestroyAsync(deleteParams);
    }

    public Task<string> GetUrlAsync(
        string objectPath,
        int expirySeconds = 604800,
        CancellationToken ct = default)
    {
        // If the objectPath is already a full http/https URL, return it directly
        if (objectPath.StartsWith("http://") || objectPath.StartsWith("https://"))
        {
            return Task.FromResult(objectPath);
        }

        bool isImage = objectPath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                    || objectPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                    || objectPath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                    || objectPath.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)
                    || objectPath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase);

        var resourceType = isImage ? "image" : "raw";

        var url = _cloudinary.Api.UrlImgUp
            .ResourceType(resourceType)
            .BuildUrl(objectPath);

        return Task.FromResult(url);
    }
}
