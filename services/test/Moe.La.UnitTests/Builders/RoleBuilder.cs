using Moe.La.Core.Dtos;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class RoleBuilder
    {
        private RoleDto _role = new RoleDto();

        public RoleBuilder Id(Guid id)
        {
            _role.Id = id;
            return this;
        }

        public RoleBuilder Name(string name)
        {
            _role.Name = name;
            return this;
        }

        public RoleBuilder NameAr(string nameAr)
        {
            _role.NameAr = nameAr;
            return this;
        }

        public RoleBuilder Priority(int? priority)
        {
            _role.Priority = priority;
            return this;
        }

        public RoleBuilder WithDefaultValues()
        {
            _role = new RoleDto()
            {
                Name = "NewRole",
                NameAr = "صلاحية جديدة",
                Priority = 1
            };

            return this;
        }

        public RoleDto Build() => _role;
    }
}
