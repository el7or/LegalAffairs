using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class InvestigationRecordPartyListItemDto : BaseDto<int>
    {
        public string PartyName { get; set; }
        public DateTime BirthDate { get; set; }
        public KeyValuePairsDto<int> StaffType { get; set; }
        public string AssignedWork { get; set; }
        public string WorkLocation { get; set; }
        public KeyValuePairsDto<int> InvestigationRecordPartyType { get; set; }
    }

    public class InvestigationRecordPartyDetailsDto : BaseDto<int>
    {
        public string PartyName { get; set; }

        public DateTime BirthDate { get; set; }
        public string BirthDateOnHijri { get; set; }
        public KeyValuePairsDto<int> StaffType { get; set; }

        public string AssignedWork { get; set; }
        public string WorkLocation { get; set; }

        public string IdentityNumber { get; set; }

        public KeyValuePairsDto<int> InvestigationRecordPartyType { get; set; }
        public DateTime CommencementDate { get; set; }
        public string CommencementDateOnHijri { get; set; }
        public DateTime IdentityDate { get; set; }
        public string IdentityDateOnHijri { get; set; }
        public KeyValuePairsDto<int> AppointmentStatus { get; set; }

        public string IdentitySource { get; set; }

        public string LastQualificationAttained { get; set; }
        public ICollection<InvestigationRecordPartyPenaltyDto> InvestigationPartyPenalties { get; set; } = new List<InvestigationRecordPartyPenaltyDto>();
        public ICollection<EvaluationDto> Evaluations { get; set; } = new List<EvaluationDto>();
        public ICollection<EducationalLevelDto> EducationalLevels { get; set; } = new List<EducationalLevelDto>();

    }

    public class InvestigationRecordPartyDto
    {
        public int? Id { get; set; }

        public int InvestigationRecordId { get; set; } = 0;

        public string PartyName { get; set; }

        public DateTime BirthDate { get; set; }

        public StaffType StaffType { get; set; }

        public string StaffTypeName { get; set; }

        public string AssignedWork { get; set; }

        public string WorkLocation { get; set; }

        public string IdentityNumber { get; set; }

        public int InvestigationRecordPartyTypeId { get; set; }

        public string InvestigationRecordPartyTypeName { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime IdentityDate { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }

        public string IdentitySource { get; set; }

        public string LastQualificationAttained { get; set; }
        public ICollection<InvestigationRecordPartyPenaltyDto> InvestigationPartyPenalties { get; set; } = new List<InvestigationRecordPartyPenaltyDto>();
        public ICollection<EvaluationDto> Evaluations { get; set; } = new List<EvaluationDto>();
        public ICollection<EducationalLevelDto> EducationalLevels { get; set; } = new List<EducationalLevelDto>();

    }

    public class InvestigationRecordPartyPenaltyDto
    {
        public int? Id { get; set; }

        public string Penalty { get; set; }

        public string Reasons { get; set; }

        public int DecisionNumber { get; set; }

        public DateTime Date { get; set; }
        public string DateOnHijri { get; set; }

        public int InvestigationRecordPartyId { get; set; }

    }
    public class EvaluationDto
    {
        public int Id { get; set; }
        public int Percentage { get; set; }
        public int Year { get; set; }
        public int InvestigationRecordPartyId { get; set; }

    }
    public class EducationalLevelDto
    {
        public int Id { get; set; }
        public string EducationLevel { get; set; }
        public string Class { get; set; }
        public string ClassNumber { get; set; }
        public string ResidenceAddress { get; set; }
        public int InvestigationRecordPartyId { get; set; }

    }

    public class FaresUserDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string AssignedWork { get; set; }

        public string WorkLocation { get; set; }

        public string IdentityNumber { get; set; }

        public string IdentitySource { get; set; }


    }

}
