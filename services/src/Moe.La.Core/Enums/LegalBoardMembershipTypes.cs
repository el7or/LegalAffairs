using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum LegalBoardMembershipTypes
    {
        [LocalizedDescription("Head", typeof(EnumsLocalization))]
        Head = 1,

        [LocalizedDescription("Member", typeof(EnumsLocalization))]
        Member = 2
    }
}
