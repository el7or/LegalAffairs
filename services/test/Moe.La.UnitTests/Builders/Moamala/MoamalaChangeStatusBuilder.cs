using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;

namespace Moe.La.UnitTests.Builders
{
    public class MoamalaChangeStatusBuilder
    {
        private MoamalaChangeStatusDto _moamalaChangeStatusDto = new MoamalaChangeStatusDto();

        public MoamalaChangeStatusBuilder MoamalaId(int moamalaId)
        {
            _moamalaChangeStatusDto.MoamalaId = moamalaId;
            return this;
        }

        public MoamalaChangeStatusBuilder Status(MoamalaStatuses status)
        {
            _moamalaChangeStatusDto.Status = status;
            return this;
        }

        public MoamalaChangeStatusBuilder AssignedToId(Guid? assignedToId)
        {
            _moamalaChangeStatusDto.AssignedToId = assignedToId;
            return this;
        }

        public MoamalaChangeStatusBuilder WorkItemTypeId(int? workItemTypeId)
        {
            _moamalaChangeStatusDto.WorkItemTypeId = workItemTypeId;
            return this;
        }

        public MoamalaChangeStatusBuilder GeneralManagementId(int branchId)
        {
            _moamalaChangeStatusDto.BranchId = branchId;
            return this;
        }

        public MoamalaChangeStatusBuilder DepartmentId(int? departmentId)
        {
            _moamalaChangeStatusDto.DepartmentId = departmentId;
            return this;
        }

        public MoamalaChangeStatusBuilder Note(string note)
        {
            _moamalaChangeStatusDto.Note = note;
            return this;
        }

        public MoamalaChangeStatusBuilder CurrentStep(MoamalaSteps step)
        {
            _moamalaChangeStatusDto.CurrentStep = step;
            return this;
        }

        public MoamalaChangeStatusBuilder BranchId(int branchId)
        {
            _moamalaChangeStatusDto.BranchId = branchId;
            return this;
        }

        public MoamalaChangeStatusBuilder WithDefaultValues()
        {
            _moamalaChangeStatusDto = new MoamalaChangeStatusDto
            {
                MoamalaId = 1,
                Status = MoamalaStatuses.Referred,
                BranchId = 1,
                WorkItemTypeId = 1,
                Note = "Change Status"
            };

            return this;
        }

        public MoamalaChangeStatusDto Build() => _moamalaChangeStatusDto;
    }
}
