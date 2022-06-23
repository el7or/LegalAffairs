using System;

namespace Moe.La.Core.Entities
{
    public class MoeenParty
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CaseId { get; set; }

        public MoeenCase Case { get; set; }

        public int? PartyTypeId { get; set; }

        public PartyType PartyType { get; set; }

        public int? IdentityTypeId { get; set; }

        public IdentityType IdentityType { get; set; }

        public string IdentityValue { get; set; }

        public string IdentitySource { get; set; }

        public DateTime? IdentityStartDate { get; set; }

        public DateTime? IdentityExpireDate { get; set; }

        public string Nationality { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public string Mobile { get; set; }

        public int? CityId { get; set; }

        public City City { get; set; }

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

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who updated it.
        /// </summary>
        public AppUser UpdatedByUser { get; set; }

        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
