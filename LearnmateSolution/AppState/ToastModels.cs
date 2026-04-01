using System;

namespace LearnmateSolution.AppState;

public enum ToastType
{
    Success,
    Error,
    Warning,
    Info
}

public class ToastMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Message { get; set; } = string.Empty;
    public ToastType Type { get; set; } = ToastType.Info;
    public DateTime CreatedAt { get; } = DateTime.Now;
    public bool IsFadingOut { get; set; } = false;
}
