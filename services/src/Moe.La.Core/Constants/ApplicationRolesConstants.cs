using System;

namespace Moe.La.Core.Constants
{
    /// <summary>
    /// أدوار المستخدمين في النظام
    /// </summary>
    public static class ApplicationRolesConstants
    {
        /// <summary>
        /// مدير النظام
        /// </summary>
        public static class Admin
        {
            public static readonly Guid Code = new Guid("be931c3d-31d3-481e-8ba6-3dacd3513c56");
            public static readonly string NameAr = "مدير النظام";
            public static readonly string Name = "Admin";
        }

        /// <summary>
        /// المشرف العام
        /// </summary>
        public static class GeneralSupervisor
        {
            public static readonly Guid Code = new Guid("ef5e1433-1bd6-4b3a-b424-0c6b0acb1b07");
            public static readonly string NameAr = "المشرف العام";
            public static readonly string Name = "GeneralSupervisor";
        }

        /// <summary>
        /// مدير إدارة
        /// </summary>
        public static class DepartmentManager
        {
            public static readonly Guid Code = new Guid("adb309c0-05b1-48e4-8ae3-f50696e45cec");
            public static readonly string NameAr = "مدير إدارة";
            public static readonly string Name = "DepartmentManager";
        }

        /// <summary>
        /// مشرف المناطق
        /// </summary>
        public static class RegionsSupervisor
        {
            public static readonly Guid Code = new Guid("b987cf49-e6a9-49dc-bdcb-0bdf0cec2d04");
            public static readonly string NameAr = "مشرف المناطق";
            public static readonly string Name = "RegionsSupervisor";
        }

        /// <summary>
        /// مدير منطقة / إدارة تعليمية
        /// </summary>
        public static class BranchManager
        {
            public static readonly Guid Code = new Guid("83acae78-7d7c-487b-8d3e-7903323c3474");
            public static readonly string NameAr = "مدير المنطقة";
            public static readonly string Name = "BranchManager";
        }

        /// <summary>
        /// رئيس اللجان
        /// </summary>
        public static class AllBoardsHead
        {
            public static readonly Guid Code = new Guid("f82a418d-45ed-4f67-8cf3-09f656a7d37c");
            public static readonly string NameAr = "رئيس اللجان";
            public static readonly string Name = "AllBoardsHead";
        }

        /// <summary>
        /// أمين لجنة رئيسية
        /// </summary>
        public static class MainBoardHead
        {
            public static readonly Guid Code = new Guid("f82a418d-45fd-4f67-8cf3-09f656a7d36b");
            public static readonly string NameAr = "أمين لجنة رئيسية";
            public static readonly string Name = "MainBoardHead";
        }

        /// <summary>
        /// أمين لجنة فرعية
        /// </summary>
        public static class SubBoardHead
        {
            public static readonly Guid Code = new Guid("0e185521-b0ae-4014-b63e-42179bd5e888");
            public static readonly string NameAr = "أمين لجنة فرعية";
            public static readonly string Name = "SubBoardHead";
        }

        /// <summary>
        /// عضو لجنة
        /// </summary>
        public static class BoardMember
        {
            public static readonly Guid Code = new Guid("18eb4cdf-2990-4161-bf68-1c24112faa6b");
            public static readonly string NameAr = "عضو لجنة";
            public static readonly string Name = "BoardMember";
        }

        /// <summary>
        /// مستشار قانوني
        /// </summary>
        public static class LegalConsultant
        {
            public static readonly Guid Code = new Guid("68ae5f14-ed2f-415d-b84a-58b803ad131f");
            public static readonly string NameAr = "مستشار قانوني";
            public static readonly string Name = "LegalConsultant";
        }

        /// <summary>
        /// باحث قانوني
        /// </summary>
        public static class LegalResearcher
        {
            public static readonly Guid Code = new Guid("32caa271-7a42-4042-85bc-40965eb58f59");
            public static readonly string NameAr = "باحث قانوني";
            public static readonly string Name = "LegalResearcher";
        }

        /// <summary>
        /// موزع المعاملات
        /// </summary>
        public static class Distributor
        {
            public static readonly Guid Code = new Guid("565d9037-6020-4d40-9749-74ea379f9b71");
            public static readonly string NameAr = "موزع المعاملات";
            public static readonly string Name = "Distributor";
        }

        /// <summary>
        /// محقق
        /// </summary>
        public static class Investigator
        {
            public static readonly Guid Code = new Guid("4cda79ea-b607-4f25-9557-253542e5981a");
            public static readonly string NameAr = "محقق";
            public static readonly string Name = "Investigator";
        }

        /// <summary>
        /// مختص الاتصالات الادارية 
        /// </summary>
        public static class AdministrativeCommunicationSpecialist
        {
            public static readonly Guid Code = new Guid("E66EB3D5-29D9-4889-90FE-7E6F6540D329");
            public static readonly string NameAr = "مختص الاتصالات الادارية";
            public static readonly string Name = "AdministrativeCommunicationSpecialist";
        }

        /// <summary>
        /// مدخل بيانات 
        /// </summary>
        public static class DataEntry
        {
            public static readonly Guid Code = new Guid("A6918018-CCF9-4497-928F-0AD56D9B384D");
            public static readonly string NameAr = "مدخل بيانات";
            public static readonly string Name = "DataEntry";
        }
    }
}
