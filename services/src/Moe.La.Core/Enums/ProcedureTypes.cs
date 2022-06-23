using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum ProcedureTypes
    {
        [LocalizedDescription("Update", typeof(EnumsLocalization))]
        Update = 1,

        [LocalizedDescription("Task", typeof(EnumsLocalization))]
        Task = 2,

        [LocalizedDescription("FieldMission", typeof(EnumsLocalization))]
        FieldMission = 3
    }
}
