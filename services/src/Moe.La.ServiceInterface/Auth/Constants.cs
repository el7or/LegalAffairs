
namespace Moe.La.ServiceInterface.Auth
{
    public static class Constants
    {
        public static class Strings
        {
            public static class MyJwtClaimIdentifiers
            {
                public const string Roles = "roles";
                public const string Permission = "Permission";
                public const string Branch = "Branch";
                public const string Department = "Department";
            }

            public static class MyJwtClaims
            {
                public const string ApiAccess = "Admin";
            }
        }
    }
}
