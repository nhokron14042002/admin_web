using SV20T1080033.DomainModels;

namespace SV20T1080033.Web.Models
{
    public class PaginationSearchShipper : PaginationSearchBaseResult
    {
        public IList<Shipper> Data { get; set; }
    }
}
