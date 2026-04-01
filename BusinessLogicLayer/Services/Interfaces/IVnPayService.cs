namespace BusinessLogicLayer.Services.Interfaces;

public interface IVnPayService
{
    /// <summary>Tạo URL thanh toán VNPay để redirect student sang cổng thanh toán</summary>
    string CreatePaymentUrl(long paymentId, string vnpTxnRef, decimal amount, string orderInfo, string clientIp);

    /// <summary>Xác thực chữ ký (secure hash) từ VNPay callback</summary>
    bool ValidateSignature(IEnumerable<KeyValuePair<string, string>> queryParams, string receivedHash);

    /// <summary>Lấy giá trị từ VNPay response params</summary>
    string? GetParam(IEnumerable<KeyValuePair<string, string>> queryParams, string key);
}
