namespace BusinessLogicLayer.Services.Interfaces;

public interface IFileStorageService
{
    /// <summary>
    /// Upload a file to storage. Returns the object path (e.g. "materials/1/report.pdf").
    /// </summary>
    Task<string> UploadAsync(
        string objectPath,
        Stream content,
        string contentType,
        CancellationToken ct = default
    );

    /// <summary>
    /// Delete a file from storage.
    /// </summary>
    Task DeleteAsync(string objectPath, CancellationToken ct = default);

    /// <summary>
    /// Returns a pre-signed URL to access the file. Default expiry: 7 days.
    /// </summary>
    Task<string> GetUrlAsync(
        string objectPath,
        int expirySeconds = 604800,
        CancellationToken ct = default
    );
}
