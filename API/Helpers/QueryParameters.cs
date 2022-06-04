namespace API.Helpers
{
    public class QueryParameters
    {
        public int MaxPageSize { get; } = 20;

        private int _pageSize = 20;
        public int PageSize 
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value > MaxPageSize || value <= 0)
                {
                    _pageSize = MaxPageSize;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }

        public int PageNumber { get; set; } = 1;
    }
}