using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class Province
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
