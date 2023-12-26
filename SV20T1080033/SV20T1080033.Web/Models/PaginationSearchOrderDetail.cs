using SV20T1080033.DomainModels;

namespace SV20T1080033.Web.Models
{
    /// <summary>
    /// Xử lí chi tiết đơn hàng
    /// </summary>
    public class PaginationSearchOrderDetail
    {
        /// <summary>
        /// Lấy ra dữ liệu của đơn hàng
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Lấy ra thông tin chi tiết của đơn hàng
        /// </summary>
        public List<DomainModels.OrderDetail> OrderDetails { get; set; }
    }
}
