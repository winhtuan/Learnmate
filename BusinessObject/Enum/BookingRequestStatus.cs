namespace BusinessObject.Enum;

public enum BookingRequestStatus
{
    PENDING,            // Student vừa book, chờ teacher duyệt
    AWAITING_PAYMENT,   // Teacher đã duyệt, student cần thanh toán
    PAYMENT_SUCCESS,    // Thanh toán thành công, đang xử lý join class
    BOOKING_SUCCESS,    // Đã thanh toán + đã join class thành công
    DECLINED,           // Teacher từ chối
    CANCELLED,          // Student hoặc hệ thống huỷ
    EXPIRED,            // Hết thời gian thanh toán
}
