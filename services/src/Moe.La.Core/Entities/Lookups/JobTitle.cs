using System;

namespace Moe.La.Core.Entities
{
    public class JobTitle
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
    }
}
