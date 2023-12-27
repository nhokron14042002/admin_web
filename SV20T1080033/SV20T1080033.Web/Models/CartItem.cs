namespace SV20T1080033.Web.Models
{
	public class CartItem
	{
		public string ProductId { get; set; } = "";
		public string ProductName { get; set; } = "";
		public string Unit { get; set; } = "";
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Total => Quantity * Price;
	}
}
