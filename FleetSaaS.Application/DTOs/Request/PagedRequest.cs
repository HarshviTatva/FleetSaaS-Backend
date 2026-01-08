namespace FleetSaaS.Application.DTOs.Request
{
    public class PagedRequest
    {
        private const int MaxPageSize = 100;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Search { get; set; }

        public string? SortBy { get; set; }

        public string SortDirection { get; set; } = "asc";

        public int? Status { get; set; } = 0;

        public string? Date { get; set; }

        public bool? ShowCompletedRecords { get; set; }
    }
}
