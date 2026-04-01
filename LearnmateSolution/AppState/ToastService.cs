using System;

namespace LearnmateSolution.AppState;

public class ToastService
{
    public List<ToastMessage> Toasts { get; } = new();
    public event Action? OnChanged;

    public void Show(string message, ToastType type = ToastType.Info, int durationMs = 2000)
    {
        var toast = new ToastMessage { Message = message, Type = type };
        Toasts.Add(toast);
        NotifyChanged();

        // Auto remove after duration
        Task.Delay(durationMs).ContinueWith(_ => RemoveToast(toast.Id));
    }

    public void ShowSuccess(string message, int durationMs = 2000) => Show(message, ToastType.Success, durationMs);
    public void ShowError(string message, int durationMs = 2000) => Show(message, ToastType.Error, durationMs);
    public void ShowWarning(string message, int durationMs = 2000) => Show(message, ToastType.Warning, durationMs);
    public void ShowInfo(string message, int durationMs = 2000) => Show(message, ToastType.Info, durationMs);

    public void RemoveToast(Guid id)
    {
        var toast = Toasts.FirstOrDefault(t => t.Id == id);
        if (toast != null)
        {
            // Optional: add a fade-out state if you want smoother removal handled by CSS
            toast.IsFadingOut = true;
            NotifyChanged();
            
            // Actually remove after a tiny delay for the animation
            Task.Delay(300).ContinueWith(_ => {
                Toasts.RemoveAll(t => t.Id == id);
                NotifyChanged();
            });
        }
    }

    private void NotifyChanged() => OnChanged?.Invoke();
}
