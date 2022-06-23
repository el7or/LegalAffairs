using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class Party : BaseEntity<int>
    {
        /// <summary>
        /// الاسم.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// نوع الطرف.
        /// </summary> 
        public PartyTypes? PartyType { get; set; }

        /// <summary>
        ///  نوع الهوية.
        /// </summary>
        public int? IdentityTypeId { get; set; }

        public IdentityType IdentityType { get; set; }

        /// <summary>
        /// رقم الهوية.
        /// </summary>
        public string IdentityValue { get; set; }

        /// <summary>
        /// مصدر الهوية.
        /// </summary>
        public string IdentitySource { get; set; }

        /// <summary>
        /// تاريخ اصدار الهوية.
        /// </summary>
        public DateTime? IdentityStartDate { get; set; }

        /// <summary>
        /// تاريخ انتهاء الهوية.
        /// </summary>
        public DateTime? IdentityExpireDate { get; set; }

        /// <summary>
        /// الجنسية.
        /// </summary>
        public int? NationalityId { get; set; }
        public Country Nationality { get; set; }

        public DateTime? BirthDate { get; set; }

        public Genders? Gender { get; set; }

        public string Mobile { get; set; }

        public int? ProvinceId { get; set; }

        public Province Province { get; set; }

        public int? CityId { get; set; }

        public City City { get; set; }

        public string Region { get; set; }

        public int? DistrictId { get; set; }
        public District District { get; set; }

        public string Street { get; set; }

        public string BuidlingNumber { get; set; }

        public string PostalCode { get; set; }

        public string AddressDetails { get; set; }

        public string TelephoneNumber { get; set; }

        public string CommertialRegistrationNumber { get; set; }

        public string Email { get; set; }

        //public string AttorneyNumber { get; set; }

        //public DateTime? AttorneyDate { get; set; }

        //public int? AttorneyPartyId { get; set; }

        //public string AttorneyPartyName { get; set; }

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
