using SV20T1080033.DomainModels;

namespace SV20T1080033.Web.Models
{
    public class PaginationSearchOrder : PaginationSearchBaseResult
    {
        /// <summary>
        /// Dữ liệu đơn hàng
        /// </summary>
        public List<Order> Data { get; set; }
    }
}
