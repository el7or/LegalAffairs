using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum CaseClosinReasons
    {
        [LocalizedDescription("EndOfObjectionPeriod", typeof(EnumsLocalization))]
        EndOfObjectionPeriod = 1,

        [LocalizedDescription("CreateNextCase", typeof(EnumsLocalization))]
        CreateNextCase = 2,
    }
}
