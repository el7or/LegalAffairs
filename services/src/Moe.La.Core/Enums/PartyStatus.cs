using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum PartyStatus
    {
        [LocalizedDescription("Self", typeof(EnumsLocalization))]
        Self = 1,

        [LocalizedDescription("Agent", typeof(EnumsLocalization))]
        Agent = 2,

        [LocalizedDescription("OrganizationAgent", typeof(EnumsLocalization))]
        OrganizationAgent = 3
    }
}
