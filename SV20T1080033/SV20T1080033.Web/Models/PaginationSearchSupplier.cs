using SV20T1080033.DomainModels;

namespace SV20T1080033.Web.Models
{
	public class PaginationSearchSupplier : PaginationSearchBaseResult
	{
		public IList<Supplier> Data { get; set; }
	}
}
