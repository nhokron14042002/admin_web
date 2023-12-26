namespace SV20T1080033.Web.Models
{
    /// <summary>
    /// Lưu trữ thông tin để tìm kiếm hóa đơn
    /// </summary>
    public class PaginationSearchOrderInput : PaginationSearchInput
    {
        /// <summary>
        /// Tình trạng đơn hàng
        /// </summary>
        public int Status { get; set; }
    }
}
