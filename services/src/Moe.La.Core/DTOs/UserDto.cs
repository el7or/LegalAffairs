using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    /// <summary>
    /// Used to display the user in a list.
    /// </summary>
    public class UserListItemDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public string Branch { get; set; }

        public string JobTitle { get; set; }

        public bool Enabled { get; set; }

        public string IdentityNumber { get; set; }

        public string EmployeeNo { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; }

        public string RoleGroup { get; set; }

        public string Signature { get; set; }

        public ICollection<RoleDto> Roles { get; set; }

        public ICollection<UserRoleDto> UserRoles { get; set; }

        public ICollection<string> Departments { get; set; } = new List<string>();

        public string DepartmentsGroup { get; set; }

        public ICollection<KeyValuePairsDto<Guid>> Researchers { get; set; }

        public string Consultant { get; set; }

    }

    /// <summary>
    /// Used to display the user information in the details view.
    /// </summary>
    public class UserDetailsDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public string Picture { get; set; }

        public string Signature { get; set; }

        public int BranchId { get; set; }

        public BranchDto Branch { get; set; }

        public int JobTitleId { get; set; }

        public JobTitleDto JobTitle { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string ExternalType { get; set; }

        public string ExternalId { get; set; }

        public string Token { get; set; }

        public bool HashCodeExpired { get; set; }

        public string IdentityNumber { get; set; }

        public string EmployeeNo { get; set; }

        public int? ExtensionNumber { get; set; }

        public bool Enabled { get; set; }

        public ICollection<RoleListItemDto> Roles { get; set; }

        public ICollection<ClaimDto> Claims { get; set; }

        public ICollection<UserRoleDepartmentDto> UserRoleDepartments { get; set; } = new List<UserRoleDepartmentDto>();

    }

    /// <summary>
    /// Used to create or edit user.
    /// </summary>
    public class UserDto
    {
        public Guid? Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public string Picture { get; set; }

        public string Signature { get; set; }

        public int? BranchId { get; set; }

        public int? JobTitleId { get; set; }

        public string IdentityNumber { get; set; }

        public string EmployeeNo { get; set; }

        public int? ExtensionNumber { get; set; }

        public bool Enabled { get; set; }

        public ICollection<Guid> Researchers { get; set; } = new List<Guid>();

        public ICollection<string> Roles { get; set; } = new List<string>();

        public ICollection<string> Claims { get; set; } = new List<string>();

        public ICollection<RefreshTokenDto> RefreshTokens { get; set; } = new List<RefreshTokenDto>();

        /// <summary>
        /// User's departments within a branch.
        /// </summary>
        public ICollection<UserRoleDepartmentDto> UserRoleDepartments { get; set; } = new List<UserRoleDepartmentDto>();
    }

    /// <summary>
    /// Used to associate a consultant with a researcher.
    /// </summary>
    public class ConsultantDto
    {
        public Guid ConsultantId { get; set; }
        public string Name { get; set; }
        public string EmployeeNo { get; set; }
    }

}