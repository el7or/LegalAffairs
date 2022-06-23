using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System.Collections.Generic;

namespace Moe.La.UnitTests.Builders
{
    class LegalBoardBuilder
    {
        private LegalBoardDto _legalBoardDto = new LegalBoardDto();
        public LegalBoardBuilder Name(string name)
        {
            _legalBoardDto.Name = name;
            return this;
        }

        public LegalBoardBuilder StatusId(LegalBoardStatuses status)
        {
            _legalBoardDto.StatusId = status;
            return this;
        }

        public LegalBoardBuilder TypeId(LegalBoardTypes type)
        {
            _legalBoardDto.TypeId = type;
            return this;
        }

        public LegalBoardBuilder WithDefaultValues()
        {
            _legalBoardDto = new LegalBoardDto
            {
                Name = "لجنة قضائية 1",
                StatusId = LegalBoardStatuses.Activated,
                TypeId = LegalBoardTypes.Secondary,
                LegalBoardMembers = new List<LegalBoradMemberDto>()
                {
                    new   LegalBoradMemberDto(){
                        UserId = TestUsers.AdminId,
                        MembershipType=LegalBoardMembershipTypes.Head
                    },
                     new   LegalBoradMemberDto(){
                        UserId = TestUsers.BoardMemberId,
                        MembershipType=LegalBoardMembershipTypes.Member
                    }
                }
            };

            return this;
        }

        public LegalBoardDto Build() => _legalBoardDto;
    }
}
