namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        // Used for searching
        public string SearchTerm { get; set; }
        // Used for data sorting
        public string OrderBy { get; set; }
        // Used for data shaping
        public string Fields { get; set; }

    }
}