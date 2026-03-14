namespace BusinessLogicLayer.Settings;

public record MinioSettings
{
    public string Endpoint { get; init; } = "localhost:9000";
    public string AccessKey { get; init; } = "";
    public string SecretKey { get; init; } = "";
    public string BucketName { get; init; } = "learnmate";
    public bool UseSSL { get; init; } = false;
}
