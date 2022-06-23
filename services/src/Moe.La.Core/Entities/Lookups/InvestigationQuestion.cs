using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class InvestigationQuestion
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        public string Question { get; set; }

        public InvestigationQuestionStatuses Status { get; set; }
    }
}
