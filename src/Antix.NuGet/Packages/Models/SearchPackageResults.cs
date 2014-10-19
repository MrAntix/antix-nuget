namespace Antix.NuGet.Packages.Models
{
    public class SearchPackageResults
    {
        readonly SearchPackageCriteria _criteria;
        readonly int _pageIndex;
        readonly IPackageMetadata[] _items;

        public SearchPackageResults(
            SearchPackageCriteria criteria,
            int pageIndex,
            IPackageMetadata[] items)
        {
            _criteria = criteria;
            _pageIndex = pageIndex;
            _items = items;
        }

        public SearchPackageCriteria Criteria
        {
            get { return _criteria; }
        }

        public IPackageMetadata[] Items
        {
            get { return _items; }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
        }
    }
}