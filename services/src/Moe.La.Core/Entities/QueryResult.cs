using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class QueryResult<T>
    {
        public int TotalItems { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
