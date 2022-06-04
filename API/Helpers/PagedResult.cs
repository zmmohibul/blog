using System.Collections.Generic;

namespace API.Helpers
{
    public class PagedResult<T>
    {
        public int Count { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
        
        public List<T> Data { get; set; }
    }
}