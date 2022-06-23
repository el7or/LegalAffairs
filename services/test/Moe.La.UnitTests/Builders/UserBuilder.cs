using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Moe.La.UnitTests.Builders
{
    public class UserBuilder
    {
        private UserDto _user = new UserDto();

        public UserBuilder Id(Guid id)
        {
            _user.Id = id;
            return this;
        }

        public UserBuilder UserName(string userName)
        {
            _user.UserName = userName;
            return this;
        }

        public UserBuilder Password(string password)
        {
            _user.Password = password;
            return this;
        }

        public UserBuilder Email(string email)
        {
            _user.Email = email;
            return this;
        }

        public UserBuilder PhoneNumber(string phoneNumber)
        {
            _user.PhoneNumber = phoneNumber;
            return this;
        }

        public UserBuilder FirstName(string firstName)
        {
            _user.FirstName = firstName;
            return this;
        }

        public UserBuilder LastName(string lastName)
        {
            _user.LastName = lastName;
            return this;
        }

        public UserBuilder Picture(string picture)
        {
            _user.Picture = picture;
            return this;
        }

        public UserBuilder SignatureUrl(string signatureUrl)
        {
            _user.Signature = signatureUrl;
            return this;
        }
        public UserBuilder BranchId(int? branchId)
        {
            _user.BranchId = branchId;
            return this;
        }

        public UserBuilder JobTitleId(int? jobTitleId)
        {
            _user.JobTitleId = jobTitleId;
            return this;
        }

        public UserBuilder IdentityNumber(string identityNumber)
        {
            _user.IdentityNumber = identityNumber;
            return this;
        }

        public UserBuilder ExtensionNumber(int? extensionNumber)
        {
            _user.ExtensionNumber = extensionNumber;
            return this;
        }

        public UserBuilder Enabled(bool enabled)
        {
            _user.Enabled = enabled;
            return this;
        }

        public UserBuilder Researchers(Collection<Guid> researchers)
        {
            _user.Researchers = researchers;
            return this;
        }

        public UserBuilder Roles(List<string> roles)
        {
            _user.Roles = roles;
            return this;
        }

        public UserBuilder WithDefaultValues()
        {
            _user = new UserDto
            {
                UserName = "1111111199",
                Password = "12345678",
                Email = "user1@gmail.com",
                PhoneNumber = "0500000000",
                FirstName = "user1",
                LastName = "last name",
                Picture = null,
                Signature = "",
                BranchId = 1,
                JobTitleId = 1,
                IdentityNumber = "1111111199",
                ExtensionNumber = 1234,
                Enabled = true,
                Roles = new List<string>() { ApplicationRolesConstants.Admin.Name, ApplicationRolesConstants.DepartmentManager.Name }
            };

            return this;
        }

        public UserDto Build() => _user;
    }
}
