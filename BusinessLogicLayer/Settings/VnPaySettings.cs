namespace BusinessLogicLayer.Settings;

public class VnPaySettings
{
    /// <summary>vnp_TmnCode - Mã website trên VNPay</summary>
    public string TmnCode { get; set; } = string.Empty;

    /// <summary>vnp_HashSecret - Chuỗi bí mật tạo checksum</summary>
    public string HashSecret { get; set; } = string.Empty;

    /// <summary>URL thanh toán VNPay (sandbox hoặc production)</summary>
    public string PaymentUrl { get; set; } = string.Empty;

    /// <summary>URL IPN (server-to-server callback từ VNPay)</summary>
    public string IpnUrl { get; set; } = string.Empty;

    /// <summary>URL trả về sau khi user thanh toán xong</summary>
    public string ReturnUrl { get; set; } = string.Empty;
}
