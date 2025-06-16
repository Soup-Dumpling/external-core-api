using System.Collections.Generic;

namespace External.Product.Core.Models
{
    public class PagedResult<T> where T : class
    {
        public PagedResult() { }
        public PagedResult(IEnumerable<T> items, int count) 
        {
            Result = items;
            Count = count;
        }
        public IEnumerable<T> Result { get; set; }
        public int Count { get; set; }
    }
}
