namespace SV20T1080033.Web.Models
{
    /// <summary>
    /// Lưu dữ liệu dùng để tìm kiếm và phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchValue { get; set; } = "";

		public int customerID { get; set; }
		public int shipperID { get; set; }
		public int employeeID { get; set; }
	}
}
