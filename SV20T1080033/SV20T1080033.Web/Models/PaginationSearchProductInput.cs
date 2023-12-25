

namespace SV20T1080033.Web.Models
{
    /// <summary>
    /// Lưu trữ thông tin để tìm kiếm mặt hàng
    /// Lọc mặt hàng theo loại, nhà cung cấp
    /// Lọc theo giá
    /// </summary>
    public class PaginationSearchProductInput : PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public int MinPrice { get; set; } 
        public int MaxPrice { get; set; } 
    }
}
