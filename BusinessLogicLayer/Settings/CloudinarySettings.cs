namespace BusinessLogicLayer.Settings;

public record CloudinarySettings
{
    public string CloudName { get; init; } = "";
    public string ApiKey { get; init; } = "";
    public string ApiSecret { get; init; } = "";
}
