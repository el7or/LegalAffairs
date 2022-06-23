using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum InvestigationQuestionStatuses
    {
        [LocalizedDescription("Undefined", typeof(EnumsLocalization))]
        Undefined = 1,

        [LocalizedDescription("Private", typeof(EnumsLocalization))]
        Private = 2,

        [LocalizedDescription("Public", typeof(EnumsLocalization))]
        Public = 3,

    }
}
