using System.Net;
using System.Security.Cryptography;
using System.Text;
using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Settings;
using Microsoft.Extensions.Options;

namespace BusinessLogicLayer.Services;

/// <summary>
/// VNPay payment service — tạo URL và xác thực chữ ký theo đúng spec VNPay 2.1.0
/// Tài liệu: https://sandbox.vnpayment.vn/apis/docs/thanh-toan-pay/pay.html
/// </summary>
public class VnPayService(IOptions<VnPaySettings> options) : IVnPayService
{
    private readonly VnPaySettings _settings = options.Value;

    public string CreatePaymentUrl(long paymentId, string vnpTxnRef, decimal amount, string orderInfo, string clientIp)
    {
        var now = DateTime.UtcNow.AddHours(7); // VNPay dùng multiple giờ Việt Nam (UTC+7)

        var queryParams = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            ["vnp_Version"]    = "2.1.0",
            ["vnp_Command"]    = "pay",
            ["vnp_TmnCode"]    = _settings.TmnCode,
            ["vnp_Amount"]     = ((long)(amount * 100)).ToString(), // Amount * 100 (VND, no decimal)
            ["vnp_CurrCode"]   = "VND",
            ["vnp_TxnRef"]     = vnpTxnRef,
            ["vnp_OrderInfo"]  = orderInfo,
            ["vnp_OrderType"]  = "other",
            ["vnp_Locale"]     = "vn",
            ["vnp_ReturnUrl"]  = _settings.ReturnUrl,
            ["vnp_IpAddr"]     = clientIp,
            ["vnp_CreateDate"] = now.ToString("yyyyMMddHHmmss"),
            ["vnp_ExpireDate"] = now.AddMinutes(15).ToString("yyyyMMddHHmmss"),
        };

        var hashData = new StringBuilder();
        var query    = new StringBuilder();

        foreach (var kv in queryParams)
        {
            if (string.IsNullOrEmpty(kv.Value)) continue;

            // Build hash data: key=UrlEncode(value)
            hashData.Append(kv.Key);
            hashData.Append('=');
            hashData.Append(WebUtility.UrlEncode(kv.Value));

            // Build query: UrlEncode(key)=UrlEncode(value)
            query.Append(WebUtility.UrlEncode(kv.Key));
            query.Append('=');
            query.Append(WebUtility.UrlEncode(kv.Value));

            hashData.Append('&');
            query.Append('&');
        }

        // Remove last '&'
        if (hashData.Length > 0) hashData.Remove(hashData.Length - 1, 1);
        if (query.Length > 0)    query.Remove(query.Length - 1, 1);

        var secureHash = HmacSha512(_settings.HashSecret, hashData.ToString());

        // Tạo URL thanh toán
        var paymentUrl = $"{_settings.PaymentUrl}?{query}&vnp_SecureHash={secureHash}";
        return paymentUrl;
    }

    public bool ValidateSignature(IEnumerable<KeyValuePair<string, string>> queryParams, string receivedHash)
    {
        // Lấy tất cả param trừ vnp_SecureHash và vnp_SecureHashType
        var sorted = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (var kv in queryParams)
        {
            if (kv.Key == "vnp_SecureHash" || kv.Key == "vnp_SecureHashType")
                continue;
            sorted[kv.Key] = kv.Value;
        }

        var hashData = new StringBuilder();
        foreach (var kv in sorted)
        {
            if (string.IsNullOrEmpty(kv.Value)) continue;

            hashData.Append(kv.Key);
            hashData.Append('=');
            hashData.Append(WebUtility.UrlEncode(kv.Value));
            hashData.Append('&');
        }

        if (hashData.Length > 0) hashData.Remove(hashData.Length - 1, 1);

        var expectedHash = HmacSha512(_settings.HashSecret, hashData.ToString());
        return string.Equals(expectedHash, receivedHash, StringComparison.OrdinalIgnoreCase);
    }

    public string? GetParam(IEnumerable<KeyValuePair<string, string>> queryParams, string key) =>
        queryParams.FirstOrDefault(kv => kv.Key == key).Value;

    private static string HmacSha512(string key, string data)
    {
        var keyBytes  = Encoding.UTF8.GetBytes(key);
        var dataBytes = Encoding.UTF8.GetBytes(data);
        using var hmac = new HMACSHA512(keyBytes);
        var hash = hmac.ComputeHash(dataBytes);
        return Convert.ToHexString(hash).ToLower();
    }
}
