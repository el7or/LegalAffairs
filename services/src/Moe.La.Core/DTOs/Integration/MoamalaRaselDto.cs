using Moe.La.Core.Enums;
using Moe.La.Core.Enums.Integration;
using System;

namespace Moe.La.Core.Dtos
{
    public class MoamalaRaselDto
    {
        public int? Id { get; set; }
        public int ItemNumber { get; set; }
        public string UnifiedNumber { get; set; }
        public string CustomNumber { get; set; }
        public int? PreviousItemNumber { get; set; }
        public string Subject { get; set; }
        public string Comments { get; set; }
        public int HijriCreatedDate { get; set; }
        public DateTime GregorianCreatedDate { get; set; }
        public int CenterIdFrom { get; set; }
        public string CenterArabicNameFrom { get; set; }
        public int? CenterIdTo { get; set; }
        public string CenterArabicNameTo { get; set; }
        public int? GroupIdFrom { get; set; }
        public string GroupNameFrom { get; set; }
        public int GroupIdTo { get; set; }
        public string GroupNameTo { get; set; }
        public int? ItemType { get; set; }
        public string ItemTypeName { get; set; }
        public ConfidentialDegrees ItemPrivacy { get; set; }
        public string ItemPrivacyName { get; set; }
        public MoamalaRaselStatuses Status { get; set; }


    }
    public class MoamalaRaselListItemDto : BaseDto<int>
    {
        public int ItemNumber { get; set; }
        public string UnifiedNumber { get; set; }
        public string CustomNumber { get; set; }
        public int? PreviousItemNumber { get; set; }
        public string Subject { get; set; }
        public string Comments { get; set; }
        public int HijriCreatedDate { get; set; }
        public string HijriCreatedOn { get; set; }
        public DateTime GregorianCreatedDate { get; set; }
        public int CenterIdFrom { get; set; }
        public string CenterArabicNameFrom { get; set; }
        public int CenterIdTo { get; set; }
        public string CenterArabicNameTo { get; set; }
        public int GroupIdFrom { get; set; }
        public string GroupNameFrom { get; set; }
        public int GroupIdTo { get; set; }
        public string GroupNameTo { get; set; }
        public int ItemType { get; set; }
        public string ItemTypeName { get; set; }
        public string CreatedOnTime { get; set; }
        public KeyValuePairsDto<int> ItemPrivacy { get; set; }
        public string ItemPrivacyName { get; set; }
        public KeyValuePairsDto<int> Status { get; set; }

    }
}
