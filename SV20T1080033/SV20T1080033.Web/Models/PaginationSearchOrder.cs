using SV20T1080033.DomainModels;

namespace SV20T1080033.Web.Models
{
    public class PaginationSearchOrder : PaginationSearchBaseResult
    {
        public IList<Customer> Data { get; set; }
    }
}
