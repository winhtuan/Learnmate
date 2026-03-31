namespace BusinessLogicLayer.Services.Interfaces;

public interface IFileStorageService
{
    /// <summary>Upload a file (PDF/doc/etc.) to storage. Returns the object path.</summary>
    Task<string> UploadAsync(
        string objectPath,
        Stream content,
        string contentType,
        CancellationToken ct = default);

    /// <summary>
    /// Upload an image (avatar/thumbnail) to Cloudinary's image pipeline.
    /// Returns the permanent CDN URL (e.g. https://res.cloudinary.com/.../avatar.jpg).
    /// </summary>
    Task<string> UploadImageAsync(
        string folder,
        string publicId,
        Stream content,
        string contentType,
        CancellationToken ct = default);

    /// <summary>Delete a file from storage.</summary>
    Task DeleteAsync(string objectPath, CancellationToken ct = default);

    /// <summary>Returns a pre-signed URL. Default expiry: 7 days.</summary>
    Task<string> GetUrlAsync(
        string objectPath,
        int expirySeconds = 604800,
        CancellationToken ct = default);
}
