using System;

namespace Moe.La.Core.Constants
{
    /// <summary>
    /// صلاحيات الأدوار في النظام
    /// </summary>
    public static class ApplicationRoleClaimsConstants
    {
        /// <summary>
        /// المعاملة السرية للغاية
        /// </summary>
        public static class ConfidentialMoamla
        {
            public static readonly Guid RoleId = ApplicationRolesConstants.Distributor.Code;

            public static readonly string ClaimType = "Permission";

            public static readonly string ClaimValue = "ConfidentialMoamla";

            public static readonly string Name = "المعاملة السرية للغاية";

            public static readonly string Description = "تمنح صلاحية التعامل مع المعاملات السرية للمستخدم الذي لديه دور موزع المعاملات";
        }
    }
}
