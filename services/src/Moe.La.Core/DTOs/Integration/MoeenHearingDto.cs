using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class MoeenHearingDto
    {
        public int CaseId { get; set; }

        public MoeenCaseDto Case { get; set; }

        public string Court { get; set; }

        public string CircleNumber { get; set; }

        public int? HearingNumber { get; set; }

        public DateTime HearingDate { get; set; }

        public string HearingTime { get; set; }

        public string HearingDesc { get; set; }

        public HearingStatuses Status { get; set; }

        public HearingTypes Type { get; set; }


        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

    }
}
