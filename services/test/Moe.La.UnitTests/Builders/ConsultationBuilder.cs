using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Consultations;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.UnitTests.Builders
{
    public class ConsultationBuilder
    {
        private ConsultationDto _consultation = new ConsultationDto();

        public ConsultationBuilder Id(int id)
        {
            _consultation.Id = id;
            return this;
        }

        public ConsultationBuilder Subject(string subject)
        {
            _consultation.Subject = subject;
            return this;
        }

        public ConsultationBuilder ConsultationNumber(string consultationNumber)
        {
            _consultation.ConsultationNumber = consultationNumber;
            return this;
        }

        public ConsultationBuilder LegalAnalysis(string legalAnalysis)
        {
            _consultation.LegalAnalysis = legalAnalysis;
            return this;
        }

        public ConsultationBuilder Status(ConsultationStatus status)
        {
            _consultation.Status = status;
            return this;
        }

        public ConsultationBuilder DepartmentVision(string departmentVision)
        {
            _consultation.DepartmentVision = departmentVision;
            return this;
        }

        public ConsultationBuilder BranchId(int branchId)
        {
            _consultation.BranchId = branchId;
            return this;
        }

        public ConsultationBuilder DepartmentId(int? departmentId)
        {
            _consultation.DepartmentId = (int)departmentId;
            return this;
        }

        public ConsultationBuilder MoamalaId(int moamalaId)
        {
            _consultation.MoamalaId = moamalaId;
            return this;
        }

        public ConsultationBuilder WorkItemTypeId(int? workItemTypeId)
        {
            _consultation.WorkItemTypeId = workItemTypeId;
            return this;
        }

        public ConsultationBuilder SubWorkItemTypeId(int? subWorkItemTypeId)
        {
            _consultation.SubWorkItemTypeId = subWorkItemTypeId;
            return this;
        }

        public ConsultationBuilder ConsultationDate(DateTime? consultationDate)
        {
            _consultation.ConsultationDate = consultationDate;
            return this;
        }

        public ConsultationBuilder ImportantElements(string importantElements)
        {
            _consultation.ImportantElements = importantElements;
            return this;
        }

        public ConsultationBuilder IsWithNote(bool? isWithNote)
        {
            _consultation.IsWithNote = isWithNote;
            return this;
        }

        public ConsultationBuilder ConsultationMerits(ICollection<ConsultationMeritsDto> consultationMerits)
        {
            _consultation.ConsultationMerits = consultationMerits;
            return this;
        }

        public ConsultationBuilder ConsultationGrounds(ICollection<ConsultationGroundsDto> consultationGrounds)
        {
            _consultation.ConsultationGrounds = consultationGrounds;
            return this;
        }

        public ConsultationBuilder ConsultationVisuals(ICollection<ConsultationVisualDto> consultationVisuals)
        {
            _consultation.ConsultationVisuals = consultationVisuals;
            return this;
        }

        public ConsultationBuilder WithDefaultValues()
        {
            _consultation = new ConsultationDto
            {
                Id = 0,
                Subject = "نموذج 1",
                LegalAnalysis = "التحليل القانوني",
                Status = ConsultationStatus.Draft,
                BranchId = 1,
                MoamalaId = 1,
                ConsultationMerits = new List<ConsultationMeritsDto>()
                {
                    new ConsultationMeritsDto(){ Text = "حيثيات جديد"},
                    new ConsultationMeritsDto(){ Text = "سند 1"},
                },
                ConsultationGrounds = new List<ConsultationGroundsDto>()
                {
                    new ConsultationGroundsDto(){ Text = "سند جديد"},
                    new ConsultationGroundsDto(){ Text = "سند 1"},
                }
            };

            return this;
        }

        public ConsultationDto Build() => _consultation;

    }
}
