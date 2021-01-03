using System.ComponentModel;

namespace Iam.Core.ApiModels
{
    public class PageableRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; } = "Id";
        public ListSortDirection SortDirection { get; set; } = ListSortDirection.Ascending;
    }
}
