using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class ObjectionPermitRequest
    {
        public int Id { get; set; }

        /// <summary>
        /// The related case id.
        /// </summary>
        public int? CaseId { get; set; }

        /// <summary>
        /// The related case.
        /// </summary>
        public Case Case { get; set; }

        /// <summary>
        /// The base request entity.
        /// </summary>
        public Request Request { get; set; }

        /// <summary>
        /// The opinion reason.
        /// </summary>
        public string Note { get; set; }


        public SuggestedOpinon SuggestedOpinon { get; set; }

        /// <summary>
        /// The reject reason or accept reason.
        /// </summary>
        public string ReplyNote { get; set; }

        /// <summary>
        /// The replay date.
        /// </summary>
        public DateTime? ReplyDate { get; set; }

    }
}
