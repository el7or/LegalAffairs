using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class LegalMemoListItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }

        public KeyValuePairsDto<int> Type { get; set; }

        public string SecondSubCategory { get; set; }

        public KeyValuePairsDto<Guid> CreatedByUser { get; set; }

        public KeyValuePairsDto<Guid?> ChangedUser { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreationTime { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedOnHigri { get; set; }

        public string UpdateTime { get; set; }

        public DateTime? RaisedOn { get; set; }

        public string RaisedOnHigri { get; set; }

        public bool? IsRead { get; set; }

        public bool? IsDeleted { get; set; }
    }

    public class LegalMemoForPrintDetailsDto
    {
        public string Text { get; set; }
        public string Researcher { get; set; }

        /// <summary>
        /// رقم القضية 
        /// </summary>
        public string CaseSourceNumber { get; set; }

        public string Court { get; set; }

        /// <summary>
        /// تاريخ بداية القضية
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// عنوان الدعوى
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// رقم الدائرة
        /// </summary>
        public string CircleNumber { get; set; }

        public string Defendant { get; set; }

        public string Plaintiff { get; set; }
    }

    public class LegalMemoDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }

        public KeyValuePairsDto<int> Type { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string updatedOnHigri { get; set; }

        public DateTime? RaisedOn { get; set; }

        public string RaisedOnHigri { get; set; }

        public string CreatedOnHigri { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int ReviewNumber { get; set; }

        public KeyValuePairsDto<int> SecondSubCategory { get; set; }

        public KeyValuePairsDto<Guid> CreatedByUser { get; set; }

        public KeyValuePairsDto<Guid?> ChangedUser { get; set; }

        public string UpdatedByUser { get; set; }

        public string Text { get; set; }

        public int InitialCaseId { get; set; }

        public string InitialCaseNumber { get; set; }

        public string DeletionReason { get; set; }

        public string SearchText { get; set; }

        public bool IsResearcher { get; set; }

        public int BoardMeetingId { get; set; } = 0;


        public ICollection<LegalBoardMemoDto> LegalBoardMemos { get; set; } = new List<LegalBoardMemoDto>();

    }

    public class LegalMemoDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LegalMemoStatuses Status { get; set; }

        public LegalMemoTypes Type { get; set; }

        public string Text { get; set; }

        public int SecondSubCategoryId { get; set; }

        public int ReviewNumber { get; set; }

        public bool? IsRead { get; set; }

        public int InitialCaseId { get; set; }

        public string DeletionReason { get; set; }

        public string SearchText { get; set; }

        public bool IsResearcher { get; set; }

    }

    public class DeletionLegalMemoDto
    {
        public int Id { get; set; }

        public string DeletionReason { get; set; }
    }
}
