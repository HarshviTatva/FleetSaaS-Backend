
namespace FleetSaaS.Application.DTOs.Response
{
    public class PagedResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
