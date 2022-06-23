using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class PartyListItemDto : BaseDto<int>
    {
        public string Name { get; set; }

        public string PartyTypeName { get; set; }

        public PartyTypes PartyTypeId { get; set; }

        public string IdentityType { get; set; }

        public string IdentityValue { get; set; }

        public string IdentitySource { get; set; }

        public DateTime IdentityStartDate { get; set; }

        public DateTime IdentityExpireDate { get; set; }

        public string Nationality { get; set; }

        public DateTime BirthDate { get; set; }

        public string BirthDateOnHijri { get; set; } = null;

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

        public DateTime AttorneyDate { get; set; }

        public int AttorneyPartyId { get; set; }

        public string AttorneyPartyName { get; set; }

        public string Email { get; set; }

        public PartyStatus? PartyStatus { get; set; }

    }

    public class PartyDetailsDto : BaseDto<int>
    {
        public string Name { get; set; }

        public PartyTypes PartyType { get; set; }

        public string PartyTypeName { get; set; }

        public string IdentityType { get; set; }

        public string IdentityValue { get; set; }

        public string IdentitySource { get; set; }

        public DateTime IdentityStartDate { get; set; }

        public DateTime IdentityExpireDate { get; set; }

        public string Nationality { get; set; }

        public DateTime DOB { get; set; }

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

        public DateTime AttorneyDate { get; set; }

        public int AttorneyPartyId { get; set; }

        public string AttorneyPartyName { get; set; }

        public string Email { get; set; }

        public PartyStatus? PartyStatus { get; set; }
    }

    public class PartyDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PartyTypes? PartyType { get; set; }
        public string PartyTypeName { get; set; }

        public int? IdentityTypeId { get; set; }

        public string IdentityType { get; set; }

        public string IdentityValue { get; set; }

        public string IdentitySource { get; set; }

        public DateTime? IdentityStartDate { get; set; }

        public DateTime? IdentityExpireDate { get; set; }

        public int? NationalityId { get; set; }

        public string CommertialRegistrationNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public Genders? Gender { get; set; }

        public string Mobile { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public string Region { get; set; }

        public int? DistrictId { get; set; }

        public string Street { get; set; }

        public string BuidlingNumber { get; set; }

        public string PostalCode { get; set; }

        public string AddressDetails { get; set; }

        public string TelephoneNumber { get; set; }

        public string Email { get; set; }

    }
}
