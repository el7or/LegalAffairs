using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class PartyBuilder
    {
        private PartyDto _party = new PartyDto();

        public PartyBuilder Name(string name)
        {
            _party.Name = name;
            return this;
        }

        public PartyBuilder PartyTypeId(PartyTypes partyType)
        {
            _party.PartyType = partyType;
            return this;
        }

        public PartyBuilder IdentityTypeId(int identityTypeId)
        {
            _party.IdentityTypeId = identityTypeId;
            return this;
        }

        public PartyBuilder IdentityValue(string identityValue)
        {
            _party.IdentityValue = identityValue;
            return this;
        }

        public PartyBuilder IdentitySource(string identitySource)
        {
            _party.IdentitySource = identitySource;
            return this;
        }

        public PartyBuilder IdentityStartDate(DateTime identityStartDate)
        {
            _party.IdentityStartDate = identityStartDate;
            return this;
        }

        public PartyBuilder IdentityExpireDate(DateTime identityExpireDate)
        {
            _party.IdentityExpireDate = identityExpireDate;
            return this;
        }

        public PartyBuilder Nationality(int nationality)
        {
            _party.NationalityId = nationality;
            return this;
        }

        public PartyBuilder BirthDate(DateTime birthDate)
        {
            _party.BirthDate = birthDate;
            return this;
        }

        public PartyBuilder Gender(Genders gender)
        {
            _party.Gender = gender;
            return this;
        }

        public PartyBuilder Mobile(string mobile)
        {
            _party.Mobile = mobile;
            return this;
        }

        public PartyBuilder CityId(int cityId)
        {
            _party.CityId = cityId;
            return this;
        }

        public PartyBuilder Region(string region)
        {
            _party.Region = region;
            return this;
        }

        public PartyBuilder District(int district)
        {
            _party.DistrictId = district;
            return this;
        }

        public PartyBuilder Street(string street)
        {
            _party.Street = street;
            return this;
        }

        public PartyBuilder BuildingNumber(string buildingNumber)
        {
            _party.BuidlingNumber = buildingNumber;
            return this;
        }

        public PartyBuilder PostalCode(string postalCode)
        {
            _party.PostalCode = postalCode;
            return this;
        }

        public PartyBuilder AddressDetails(string addressDetails)
        {
            _party.AddressDetails = addressDetails;
            return this;
        }

        public PartyBuilder TelephoneNumber(string telephoneNumber)
        {
            _party.TelephoneNumber = telephoneNumber;
            return this;
        }

        //public PartyBuilder AttorneyNumber(string attorneyNumber)
        //{
        //    _party.AttorneyNumber = attorneyNumber;
        //    return this;
        //}

        //public PartyBuilder AttorneyDate(DateTime attorneyDate)
        //{
        //    _party.AttorneyDate = attorneyDate;
        //    return this;
        //}

        //public PartyBuilder AttorneyPartyId(int attorneyPartyId)
        //{
        //    _party.AttorneyPartyId = attorneyPartyId;
        //    return this;
        //}

        //public PartyBuilder AttorneyPartyName(string attorneyPartyName)
        //{
        //    _party.AttorneyPartyName = attorneyPartyName;
        //    return this;
        //}

        public PartyBuilder WithDefaultValues()
        {
            _party = new PartyDto
            {
                PartyType = PartyTypes.Person,
                IdentityTypeId = 1,
                IdentityValue = "1234567890",
                IdentitySource = "الرياض",
                IdentityStartDate = new DateTime(2020, 01, 01),
                IdentityExpireDate = new DateTime(2030, 01, 01),
                //Nationality = "سعودي",
                BirthDate = new DateTime(1990, 01, 01),
                //Gender = "ذكر",
                Mobile = "0506051123",
                CityId = 1,
                ProvinceId = 1,
                Region = "الرياض",
                //District = "الرياض",
                Street = "15 hope st.",
                BuidlingNumber = "12345",
                PostalCode = "12345",
                TelephoneNumber = "0111234567"
            };

            return this;
        }

        public PartyDto Build() => _party;
    }
}
