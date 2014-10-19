namespace Antix.NuGet.Packages.Models
{
    public class SearchPackageCriteria
    {
        public string Text { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}