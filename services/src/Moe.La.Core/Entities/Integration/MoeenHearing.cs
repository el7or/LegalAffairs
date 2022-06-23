using System;

namespace Moe.La.Core.Entities
{
    public class MoeenHearing
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public MoeenCase Case { get; set; }

        public string Court { get; set; }

        public string CircleNumber { get; set; }

        public int? HearingNumber { get; set; }

        public DateTime HearingDate { get; set; }

        public string HearingTime { get; set; }

        public string HearingDesc { get; set; }


        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }
    }
}
