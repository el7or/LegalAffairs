using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class MoamalaBuilder
    {
        private MoamalaDto moamala = new MoamalaDto();

        public MoamalaBuilder Id(int id)
        {
            moamala.Id = id;
            return this;
        }

        public MoamalaBuilder UnifiedNo(string unifiedNo)
        {
            moamala.UnifiedNo = unifiedNo;
            return this;
        }

        public MoamalaBuilder MoamalaNumber(string moamalaNumber)
        {
            moamala.MoamalaNumber = moamalaNumber;
            return this;
        }

        public MoamalaBuilder DepartmentId(int departmentId)
        {
            moamala.SenderDepartmentId = departmentId;
            return this;
        }

        public MoamalaBuilder ConfidentialDegree(ConfidentialDegrees confidentialDegree)
        {
            moamala.ConfidentialDegree = confidentialDegree;
            return this;
        }

        public MoamalaBuilder Subject(string subject)
        {
            moamala.Subject = subject;
            return this;
        }

        public MoamalaBuilder PassType(PassTypes passType)
        {
            moamala.PassType = passType;
            return this;
        }

        public MoamalaBuilder PassDate(DateTime passDate)
        {
            moamala.PassDate = passDate;
            return this;
        }

        public MoamalaBuilder Status(MoamalaStatuses status)
        {
            moamala.Status = status;
            return this;
        }

        public MoamalaBuilder WorkItemTypeId(int workItemTypeId)
        {
            moamala.WorkItemTypeId = workItemTypeId;
            return this;
        }

        public MoamalaBuilder IsRead(bool isRead)
        {
            moamala.IsRead = isRead;
            return this;
        }

        public MoamalaBuilder Description(string description)
        {
            moamala.Description = description;
            return this;
        }

        public MoamalaBuilder ReferralNote(string referralNote)
        {
            moamala.ReferralNote = referralNote;
            return this;
        }
        public MoamalaBuilder WithDefaultValues()
        {
            moamala = new MoamalaDto
            {
                UnifiedNo = "1111",
                MoamalaNumber = "1001",
                SenderDepartmentId = 1,
                ConfidentialDegree = ConfidentialDegrees.Normal,
                Subject = "تجربة معاملة",
                PassType = PassTypes.Import,
                PassDate = DateTime.Now,
                Status = MoamalaStatuses.New,
                WorkItemTypeId = 1,
                IsRead = false,
                Description = "تفاصيل المعاملة"
            };

            return this;
        }

        public MoamalaDto Build() => moamala;
    }
}
