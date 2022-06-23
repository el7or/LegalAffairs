using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Models.Integration.MIM
{
    public class IdentityModel
    {
        public string Username { get; set; }
        public bool Enabled { get; set; }
        public bool IsExternal { get; set; }
        public bool IsSiliconBased { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string FourthName { get; set; }
        public Genders Gender { get; set; }
        public string MobileNumber { get; set; }
        public string NationalId { get; set; }
        public IdentityTypes NationalType { get; set; }
        public string Nationality { get; set; }
    }
}
