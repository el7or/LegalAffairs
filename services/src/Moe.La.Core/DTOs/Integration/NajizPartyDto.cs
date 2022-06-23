using System;

namespace Moe.La.Core.Dtos
{
    public class NajizPartyDto
    {
        public string Name { get; set; }

        public int CaseId { get; set; }

        public NajizCaseDto Case { get; set; }

        public string PartyType { get; set; }

        public string IdentityType { get; set; }

        public string IdentityValue { get; set; }

        public string IdentitySource { get; set; }

        public DateTime? IdentityStartDate { get; set; }

        public DateTime? IdentityExpireDate { get; set; }

        public string Nationality { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public string Mobile { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string BuidlingNumber { get; set; }

        public string PostalCode { get; set; }

        public string AddressDetails { get; set; }

        public string TelephoneNumber { get; set; }

        public string AttorneyNumber { get; set; }

        public DateTime? AttorneyDate { get; set; }

        public int? AttorneyPartyId { get; set; }

        public string AttorneyPartyName { get; set; }

        public int PartyId { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }


        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? CreatedOn { get; set; }
    }
}
