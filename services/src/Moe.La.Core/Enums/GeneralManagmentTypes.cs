using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum GeneralManagementTypes
    {
        [LocalizedDescription("Parent", typeof(EnumsLocalization))]
        Parent = 1,

        [LocalizedDescription("Child", typeof(EnumsLocalization))]
        Child = 2
    }
}
