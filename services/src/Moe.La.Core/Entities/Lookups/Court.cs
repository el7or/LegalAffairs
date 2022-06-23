using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class Court
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

        public LitigationTypes? LitigationType { get; set; }

        public CourtCategories CourtCategory { get; set; }

        public string Code { get; set; }
    }
}
