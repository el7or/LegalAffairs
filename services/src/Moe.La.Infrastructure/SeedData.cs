using Moe.La.Core.Constants;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;

namespace Moe.La.Infrastructure
{
    public class SeedData
    {
        private static DateTime now = DateTime.Now;

        private static bool IsDevelopment { get; set; } = true;

        private static int MinistryBranchId = 1, RiyadhBranchId = 2;

        /// <summary>
        /// Initialize the database.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="isDevelopment">Determine if this is the development environment.</param>
        public static void Initialize(LaDbContext db, bool isDevelopment)
        {

            if (db is null)
            {
                throw new ArgumentNullException(nameof(db));
            }

            IsDevelopment = isDevelopment;

            // Roles.
            db.Roles.AddRange(CreateRoles());

            // RoleClaims.
            db.RoleClaims.AddRange(CreateRoleClaims());

            // Cities.
            db.Cities.AddRange(CreateCities());

            // Courts.
            db.Courts.AddRange(CreateCourts());

            // IdentityTypes.
            db.IdentityTypes.AddRange(CreateIdentityTypes());

            // JobTitles.
            db.JobTitles.AddRange(CreateJobTitles());

            // PartyTypes.
            db.PartyTypes.AddRange(CreatePartyTypes());

            // PartyEntityTypes
            db.PartyEntityTypes.AddRange(CreatePartyEntityTypes());

            // Provencies.
            db.Provinces.AddRange(CreateProvinces());

            // Countries.
            db.Countries.AddRange(CreateCountries());

            // AttachmentTypes.
            db.AttachmentsTypes.AddRange(CreateAttachmentTypes());

            // FieldMissionTypes.
            db.FieldMissionTypes.AddRange(CreateFieldMissionType());

            // LegailAffairDepartments.
            db.Branches.AddRange(CreateBranches());

            //Departments
            db.Departments.AddRange(CreateDepartments());

            // WorkflowStatuses.
            db.WorkflowStatuses.AddRange(CreateWorkflowStatuses());

            // WorkflowStepsCategories.
            db.WorkflowStepsCategories.AddRange(CreateWorkflowStepCategories());

            // Users with roles.
            db.Users.AddRange(CreatUsersWithRoles());

            //MinistryDepartments
            db.MinistryDepartments.AddRange(CreateMinistryDepartments());

            //MinistrySectors
            db.MinistrySectors.AddRange(CreateMinistrySectors());

            //InvestigationRecordPartyTypes
            db.InvestigationRecordPartyTypes.AddRange(CreateInvestigationRecordPartyType());

            // GovernmentOrganizations
            db.GovernmentOrganizations.AddRange(CreateGovernmentOrganizations());

            // Templates
            db.LetterTemplates.AddRange(CreateLetterTemplates());

            // Case Categories
            db.SecondSubCategories.AddRange(CreateSecondSubCategories());

            db.SaveChanges();
        }

        private static LetterTemplate[] CreateLetterTemplates()
        {
            return new[]
            {
                new LetterTemplate
                {
                    Id = 1,
                    Name = "قالب طلب مستند داعم - 1",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    Thumbnail = $"{ApplicationConstants.TemplateImageLocation}template3.png",
                    Type = LetterTemplateTypes.CaseSupportingDocumentLetter,
                    Text = @"<div style='font-family: 'Traditional Arabic';'>
                                        <p style='font-size: 1em;text-align: center;font-weight: bold;'>عاجل جدا</p>
                                        <div style='font-size: 1em;font-weight: bold;'>
                                                <p style='width: 90%;text-align: right;float: right;'> سعادة مدير #DepartmentName</p>
                                                <p style='width: 10%;text-align: left;float: left;'> وفقه الله</p>
                                        </div><br />
                                        <div style='font-size: 1em;text-align: right;font-weight: bold;'>السلام عليكم ورحمة الله وبركاته،</div>
                                        <div style='text-align: right;font-size: 1em;'> إشارة إلى الدعوى المقامة من/ #Plaintiff والمقيدة في #CaseCourtName رقم #CaseNumber لعام #CaseDate هـ بشأن #CaseSubject. <br />آمل إطلاع سيادتكم والإيعاز لمن يلزم بالرد على ما ورد في لائحة الدعوى المرفقة مدعما بالمستندات النظامية؛ حتى نتمكن من إعداد مذكرة الرد قبل موعد الجلسة القادمة والمحددة لنظرها يوم #HearingDate هـ. <br /><br /></div>
                                </div>"
                },
                new LetterTemplate
                {
                    Id = 2,
                    Name = "قالب طلب خطاب إلحاقى - 1",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    Thumbnail = $"{ApplicationConstants.TemplateImageLocation}template1.png",
                    Type = LetterTemplateTypes.AttachedLetter,
                    Text = @"<div style='font-family: 'Traditional Arabic';'>
                                        <p style='font-size: 1em;text-align: center;font-weight: bold;'>عاجل جدا</p>
                                        <div style='font-size: 1em;font-weight: bold;'>
                                                <p style='width: 90%;text-align: right;float: right;'> سعادة مدير #DepartmentName</p>
                                                <p style='width: 10%;text-align: left;float: left;'> وفقه الله</p>
                                        </div><br />
                                        <div style='font-size: 1em;text-align: right;font-weight: bold;'>السلام عليكم ورحمة الله وبركاته،</div>
                                        <div class='section-arabic' style='font-size: 1em;'> إلحاقا لخطابنا رقم #CaseSupportingDocumentRequestNumber وتاريخ #CaseSupportingDocumentRequestDate هـ. بشأن الدعوى المقامة من  #Plaintiff والمقيدة في #CaseCourtName رقم #CaseNumber لعام #CaseDate هـ. <br>نفيدكم أنه حتى تاريخه لم ترد الإجابة على الخطاب المشار إليه أعلاه. <br />آمل إطلاع سعادتكم والإيعاز لمن يلزم بالإفادة عن ما ورد في لائحة الدعوى المرفقة مدعما بالمستندات النظامية؛ لنتمكن من إعداد مذكرة الرد وذلك قبل موعد الجلسة القادمة و المحدد لنظرها يوم #HearingDate هـ. </div>
                                  </div>"
                },
                new LetterTemplate
                {
                    Id = 3,
                    Name = "قالب طلب خطاب إلحاقى - 2",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    Thumbnail = $"{ApplicationConstants.TemplateImageLocation}template2.png",
                    Type = LetterTemplateTypes.AttachedLetter,
                    Text = @"<div style='font-family: 'Traditional Arabic';'>
                                        <p style='font-size: 1em;text-align: center;font-weight: bold;'>عاجل جدا</p>
                                        <div style='font-size: 1em;font-weight: bold;'>
                                                <p style='width: 90%;text-align: right;float: right;'> سعادة مدير #DepartmentName</p>
                                                <p style='width: 10%;text-align: left;float: left;'> وفقه الله</p>
                                        </div><br />
                                        <div style='font-size: 1em;text-align: right;font-weight: bold;'>السلام عليكم ورحمة الله وبركاته،</div>
                                        <div class='section-arabic' style='font-size: 1em;'> إشارة للدعوى رقم #CaseNumber لعام #CaseDate هـ والمقامة من #Plaintiff والمقيدة في #CaseCourtName <br /> وإلحاقا لخطابنا رقم #CaseSupportingDocumentRequestNumber وتاريخ #CaseSupportingDocumentRequestDate هـ بشأن الإفادة عن طلبات الدائرة ناظرة القضية.<br>نفيدكم أنه حتى تاريخه لم ترد الإجابة على الخطاب المشار إليه أعلاه. <br />آمل من سعادتكم الاطلاع وسرعة موافاتنا بالمطلوب مدعما ذلك بالمستندات اللازمة؛ حتى نتمكن من تقديم اللائحة الاعتراضية على الحكم خلال المدة النظامية وفي حال عدم تزويدنا بالمطلوب فإن الإدارة تخلي مسؤوليتها عن ذلك. <br /></div>
                                  </div>"
                },
                new LetterTemplate
                {
                    Id = 4,
                    Name = "قالب طلب تصدير الحكم - 1",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    Thumbnail = $"{ApplicationConstants.TemplateImageLocation}caseClose1.png",
                    Type = LetterTemplateTypes.CaseClosingLetter,
                    Text =  @"<div style='font-family: 'Traditional Arabic';'>
                                        <p style='font-size: 1em;text-align: center;font-weight: bold;'>عاجل جدا</p>
                                        <div style='font-size: 1em;font-weight: bold;'>
                                                <p style='width: 90%;text-align: right;float: right;'> سعادة مدير #DepartmentName</p>
                                                <p style='width: 10%;text-align: left;float: left;'> وفقه الله</p>
                                        </div><br />
                                        <div style='font-size: 1em;text-align: right;font-weight: bold;'>السلام عليكم ورحمة الله وبركاته،</div>
                                        <div class='section-arabic' style='font-size: 1em;'> بشأن الدعوى رقم #CaseNumber لعام #CaseDate والمقدمة من #Plaintiff الحكم الصادر من #CaseCourtName بحكمها رقم #RuleNumber لعام #RuleDate والمتضمن <br /> #CaseJudgmentText <br /> وحيث أصبح الحكم نهائيا وواجب النفاذ لذا آمل الاطلاع و إكمال اللازم حيال تنفيذ الحكم بعد مطابقته بنسخة الحكم المختوم بختم التنفيذ مع التأكد من عدم ازدواجية الصرف</div>
                                   </div>"
                }
            };
        }

        private static WorkflowStatus[] CreateWorkflowStatuses()
        {
            return new[]
            {
                new WorkflowStatus{ Id = 1, StatusArName = "غير مكتملة" },
                new WorkflowStatus{ Id = 2, StatusArName = "جديدة"},
                new WorkflowStatus{ Id = 3, StatusArName = "تحت الإجراء" },
                new WorkflowStatus{ Id = 4, StatusArName = "معادة" },
                new WorkflowStatus{ Id = 5, StatusArName = "ملغاه" },
                new WorkflowStatus{ Id = 6, StatusArName = "مكتملة" }
            };
        }

        private static WorkflowStepCategory[] CreateWorkflowStepCategories()
        {
            return new[]
            {
                new WorkflowStepCategory { Id = 1, CategoryArName = "إنشائية", CreatedOn = now },
                new WorkflowStepCategory { Id = 2, CategoryArName = "إجرائية", CreatedOn = now },
                new WorkflowStepCategory { Id = 3, CategoryArName = "إنهائية", CreatedOn = now },
            };
        }

        private static AppRole[] CreateRoles()
        {
            return new[]
            { 
                // مدير النظام 
                new AppRole{ Id = ApplicationRolesConstants.Admin.Code, Name = ApplicationRolesConstants.Admin.Name, NormalizedName = ApplicationRolesConstants.Admin.Name.ToUpper(), NameAr = ApplicationRolesConstants.Admin.NameAr, ConcurrencyStamp = "8de18ffe-2ed8-4ec7-9fad-a847fc51636e", Priority = 1 },
                // المشرف العام
                new AppRole{ Id = ApplicationRolesConstants.GeneralSupervisor.Code, Name = ApplicationRolesConstants.GeneralSupervisor.Name, NormalizedName = ApplicationRolesConstants.GeneralSupervisor.Name.ToUpper(), NameAr = ApplicationRolesConstants.GeneralSupervisor.NameAr, ConcurrencyStamp = "191c07fa-3f91-490b-bf8c-c95540b96b03", Priority = 2 },
                // مشرف المناطق
                new AppRole{ Id = ApplicationRolesConstants.RegionsSupervisor.Code, Name = ApplicationRolesConstants.RegionsSupervisor.Name, NormalizedName = ApplicationRolesConstants.RegionsSupervisor.Name.ToUpper(), NameAr = ApplicationRolesConstants.RegionsSupervisor.NameAr, ConcurrencyStamp = "ed60cb03-5004-4177-a12b-8458e398248a", Priority = 3 },
                // المدير العام
                new AppRole{ Id = ApplicationRolesConstants.BranchManager.Code, Name = ApplicationRolesConstants.BranchManager.Name, NormalizedName = ApplicationRolesConstants.BranchManager.Name.ToUpper(), NameAr = ApplicationRolesConstants.BranchManager.NameAr,ConcurrencyStamp = "6edd9dca-a8bb-4ab7-99ef-67b3eef5136e", Priority = 4 },
                // مدير إدارة متخصصة
                new AppRole{ Id = ApplicationRolesConstants.DepartmentManager.Code, Name = ApplicationRolesConstants.DepartmentManager.Name, NormalizedName = ApplicationRolesConstants.DepartmentManager.Name.ToUpper(), NameAr = ApplicationRolesConstants.DepartmentManager.NameAr, ConcurrencyStamp = "c58b4f8c-1cec-4002-a95f-f4e5d4e9a7d8", Priority = 5 , IsDistributable = true},
                // رئيس اللجنة المركزية
                new AppRole{ Id = ApplicationRolesConstants.MainBoardHead.Code, Name = ApplicationRolesConstants.MainBoardHead.Name, NormalizedName = ApplicationRolesConstants.MainBoardHead.Name.ToUpper(), NameAr = ApplicationRolesConstants.MainBoardHead.NameAr, ConcurrencyStamp = "32a28783-7d5f-40e5-9356-33117b702bab", Priority = 6 },
                // رئيس لجنة
                new AppRole{ Id = ApplicationRolesConstants.SubBoardHead.Code, Name = ApplicationRolesConstants.SubBoardHead.Name, NormalizedName = ApplicationRolesConstants.SubBoardHead.Name.ToUpper(), NameAr = ApplicationRolesConstants.SubBoardHead.NameAr, ConcurrencyStamp = "32a28783-7d5f-40e5-9356-33117b702bab", Priority = 7 },
                // رئيس اللجان
                new AppRole{ Id = ApplicationRolesConstants.AllBoardsHead.Code, Name = ApplicationRolesConstants.AllBoardsHead.Name, NormalizedName = ApplicationRolesConstants.AllBoardsHead.Name.ToUpper(), NameAr = ApplicationRolesConstants.AllBoardsHead.NameAr, ConcurrencyStamp = "32a28783-7d5f-40e5-9356-33117b702cbc", Priority = 7 },
                // عضو لجنة
                new AppRole{ Id = ApplicationRolesConstants.BoardMember.Code, Name = ApplicationRolesConstants.BoardMember.Name, NormalizedName = ApplicationRolesConstants.BoardMember.Name.ToUpper(), NameAr = ApplicationRolesConstants.BoardMember.NameAr, ConcurrencyStamp = "a098390f-3810-4aec-bc55-bc679559b573", Priority = 8 },
                // مستشار
                new AppRole{ Id = ApplicationRolesConstants.LegalConsultant.Code, Name = ApplicationRolesConstants.LegalConsultant.Name, NormalizedName = ApplicationRolesConstants.LegalConsultant.Name.ToUpper(), NameAr = ApplicationRolesConstants.LegalConsultant.NameAr, ConcurrencyStamp = "6f745482-2f6c-4d8f-8677-07622166794d", Priority = 9, IsDistributable = true },
                // باحث
                new AppRole{ Id = ApplicationRolesConstants.LegalResearcher.Code, Name = ApplicationRolesConstants.LegalResearcher.Name, NormalizedName = ApplicationRolesConstants.LegalResearcher.Name.ToUpper(), NameAr = ApplicationRolesConstants.LegalResearcher.NameAr, ConcurrencyStamp = "cddb916a-5c90-4e92-a9c7-ccb0aa8afc40", Priority = 10, IsDistributable = true },
                // موزع معاملات
                new AppRole{ Id = ApplicationRolesConstants.Distributor.Code, Name = ApplicationRolesConstants.Distributor.Name, NormalizedName = ApplicationRolesConstants.Distributor.Name.ToUpper(), NameAr = ApplicationRolesConstants.Distributor.NameAr, ConcurrencyStamp = "0424039a-5e2f-4816-88c7-d69afa5c7fdf", Priority = 11 },
                // محقق
                new AppRole{ Id = ApplicationRolesConstants.Investigator.Code, Name = ApplicationRolesConstants.Investigator.Name, NormalizedName = ApplicationRolesConstants.Investigator.Name.ToUpper(), NameAr = ApplicationRolesConstants.Investigator.NameAr, ConcurrencyStamp = "043e1a8c-a210-4c7c-abdf-aeb155a7b4a0", Priority = 12, IsDistributable = true },
                // مسئول الإتصالات الإدارية
                new AppRole{ Id = ApplicationRolesConstants.AdministrativeCommunicationSpecialist.Code, Name = ApplicationRolesConstants.AdministrativeCommunicationSpecialist.Name, NormalizedName = ApplicationRolesConstants.AdministrativeCommunicationSpecialist.Name.ToUpper(), NameAr = ApplicationRolesConstants.AdministrativeCommunicationSpecialist.NameAr, ConcurrencyStamp = "BDF75718-282A-4409-A1E2-D9062A08DFB9", Priority = 13 },
                // مدخل بيانات
                new AppRole{ Id = ApplicationRolesConstants.DataEntry.Code, Name = ApplicationRolesConstants.DataEntry.Name, NormalizedName = ApplicationRolesConstants.DataEntry.Name.ToUpper(), NameAr = ApplicationRolesConstants.DataEntry.NameAr, ConcurrencyStamp = "6D52A2BA-2706-4E12-8970-5FE4260D21DD", Priority = 14 },
            };
        }

        private static AppRoleClaim[] CreateRoleClaims()
        {
            return new[]
            {
                new AppRoleClaim{ RoleId = ApplicationRolesConstants.Distributor.Code, NameAr = ApplicationRoleClaimsConstants.ConfidentialMoamla.Name, ClaimType = ApplicationRoleClaimsConstants.ConfidentialMoamla.ClaimType, ClaimValue = ApplicationRoleClaimsConstants.ConfidentialMoamla.ClaimValue, Description = ApplicationRoleClaimsConstants.ConfidentialMoamla.Description},
            };
        }

        private static AppUser[] CreatUsersWithRoles()
        {
            var users = new List<AppUser>
            {
                // System Administrator
                new AppUser
                {
                    Id = ApplicationConstants.SystemAdministratorId,
                    CreatedOn = now,
                    FirstName = "نظام",
                    LastName = "مرافعة",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    BranchId = MinistryBranchId,
                    Enabled = true
                }
            };

            if (IsDevelopment)
            {
                var devUsers = new[]
                {
                    // Admin
                    new AppUser
                    {
                        Id = new Guid("f4dd3f12-d6ec-4e9c-a2b2-ed7cfae15c7b"),
                        CreatedOn = now,
                        FirstName = "احمد",
                        LastName = "الرويشد",
                        UserName = "1111111111",
                        NormalizedUserName = "1111111111",
                        IdentityNumber = "1111111111",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "admin@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567891",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000000",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.Admin.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType= "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // GeneralSupervisor
                    new AppUser
                    {
                        Id = new Guid("c1cc4289-0e4a-4e6b-9b21-6b6c39841eb1"),
                        CreatedOn = now,
                        FirstName = "عبيد",
                        LastName = "الدوسري",
                        UserName = "1111111112",
                        NormalizedUserName = "1111111112",
                        IdentityNumber = "1111111112",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "GeneralSupervisor@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567892",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000001",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.GeneralSupervisor.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType= "Branch",
                                ClaimValue= MinistryBranchId.ToString()
                            }
                        }
                    },
                    // LitigationManager
                    new AppUser
                    {
                        Id = new Guid("b4dfa7a8-94b6-46d3-9a8f-ca4ba323d6b3"),
                        CreatedOn = now,
                        FirstName = "ناصر",
                        LastName = "العتيبي",
                        UserName = "1111111113",
                        NormalizedUserName = "1111111113",
                        IdentityNumber = "1111111113",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "LitigationManager@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567893",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000002",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.DepartmentManager.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Litigation
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            },
                            new AppUserClaim
                            {
                                ClaimType = "Department",
                                ClaimValue = ((int)Departments.Litigation).ToString()
                            }
                        }
                    },
                    // RegionsSupervisor
                    new AppUser
                    {
                        Id = new Guid("3a350fdf-ea98-4bdf-b230-1b8f378ba700"),
                        CreatedOn = now,
                        FirstName = "صالح",
                        LastName = "المطيري",
                        UserName = "1111111114",
                        NormalizedUserName = "1111111114",
                        IdentityNumber = "1111111114",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "RegionsSupervisor@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567894",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000003",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.RegionsSupervisor.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // BranchManager
                    new AppUser
                    {
                        Id = new Guid("773751be-8998-46f4-8693-ea1b5d877b8f"),
                        CreatedOn = now,
                        FirstName = "خالد",
                        LastName = "العنزي",
                        UserName = "1111111115",
                        NormalizedUserName = "1111111115",
                        IdentityNumber = "1111111115",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "BranchManager@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567895",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000004",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.BranchManager.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // MainBoardHead
                    new AppUser
                    {
                        Id = new Guid("22a1955d-7ea2-4dbc-b8fc-b47710188635"),
                        CreatedOn = now,
                        FirstName = "زياد",
                        LastName = "المالكي",
                        UserName = "1111111116",
                        NormalizedUserName = "1111111116",
                        IdentityNumber = "1111111116",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "MainBoardHead@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567896",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000005",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.MainBoardHead.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // AllBoardsHead
                    new AppUser
                    {
                        Id = new Guid("18b96059-0101-42d8-8f31-cf52c94e5e5e"),
                        CreatedOn = now,
                        FirstName = "ابراهيم",
                        LastName = "احمد",
                        UserName = "1111111180",
                        NormalizedUserName = "1111111180",
                        IdentityNumber = "1111111180",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "AllBoardsHead@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234451457",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000006",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.AllBoardsHead.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // BoardHead
                    new AppUser
                    {
                        Id = new Guid("18b96059-0101-42d8-8f31-cf52c9a9e8f6"),
                        CreatedOn = now,
                        FirstName = "احمد",
                        LastName = "الحميد",
                        UserName = "1111111117",
                        NormalizedUserName = "1111111117",
                        IdentityNumber = "1111111117",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "BoardHead@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567897",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000007",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.LegalConsultant.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // LegalConsultant
                    new AppUser
                    {
                        Id = new Guid("51251117-896b-4fe9-8da2-545c21195f63"),
                        CreatedOn = now,
                        FirstName = "عبدالرحمن",
                        LastName = "الحمود",
                        UserName = "1111111118",
                        NormalizedUserName = "1111111118",
                        IdentityNumber = "1111111118",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "BoardMember@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567898",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000008",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.LegalConsultant.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Litigation
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // LegalConsultant
                    new AppUser
                    {
                        Id = new Guid("c0f56471-7b26-4b0d-986e-1e0cd8ce512f"),
                        CreatedOn = now,
                        FirstName = "ذياب",
                        LastName = "الشمري",
                        UserName = "1111111119",
                        NormalizedUserName = "1111111119",
                        IdentityNumber = "1111111119",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "LegalConsultant1@moe.sa",
                        Enabled = true,
                        EmployeeNo = "1234567899",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000009",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.LegalConsultant.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Litigation
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // LegalConsultant
                    new AppUser
                    {
                        Id = new Guid("b7180aaa-891d-48f4-b706-7a399610dff4"),
                        CreatedOn = now,
                        FirstName = "علي",
                        LastName = "الهلالي",
                        UserName = "1111111120",
                        NormalizedUserName = "1111111120",
                        IdentityNumber = "1111111120",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "LegalConsultant2@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567890",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = RiyadhBranchId,
                        PhoneNumber = "500000010",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.LegalConsultant.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Litigation
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = RiyadhBranchId.ToString()
                            }
                        }
                    },
                    // LegalResearcher
                    new AppUser
                    {
                        Id = new Guid("aa78b684-9a4f-482c-b8c9-f3fc200690a5"),
                        CreatedOn = now,
                        FirstName = "حمد",
                        LastName = "عبدالله",
                        UserName = "1111111121",
                        NormalizedUserName = "1111111121",
                        IdentityNumber = "1111111121",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "LegalResearcher1@moe.sa",
                        Enabled = true,
                        EmployeeNo= "2234567891",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000011",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.LegalResearcher.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Litigation
                                    },
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.RegulationsAndLaws
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // LegalResearcher
                    new AppUser
                    {
                        Id = new Guid("477829d0-2471-4bae-8c03-4ed1e4f1ca1b"),
                        CreatedOn = now,
                        FirstName = "طلال",
                        LastName = "الحمراني",
                        UserName = "1111111122",
                        NormalizedUserName = "1111111122",
                        IdentityNumber = "1111111122",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "LegalResearcher2@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567892",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = RiyadhBranchId,
                        PhoneNumber = "500000012",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.LegalResearcher.Code ,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        DepartmentId = (int) Departments.Litigation,
                                        CreatedBy = new Guid("f4dd3f12-d6ec-4e9c-a2b2-ed7cfae15c7b")
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = RiyadhBranchId.ToString()
                            }
                        }
                    },
                    // Distributor
                    new AppUser
                    {
                        Id = new Guid("eef15323-953e-4d8e-a287-3e5ffb2edb80"),
                        CreatedOn = now,
                        FirstName = "عبدالله",
                        LastName = "الشهراني",
                        UserName = "1111111123",
                        NormalizedUserName = "1111111123",
                        IdentityNumber = "1111111123",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "Distributor@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567893",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000013",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.Distributor.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // DataEntry
                    new AppUser
                    {
                        Id = new Guid("B69C2AE2-4E24-4AA9-93F3-1AF4DC2592E8"),
                        CreatedOn = now,
                        FirstName = "عبد العزيز",
                        LastName = "القحطاني",
                        UserName = "1111111133",
                        NormalizedUserName = "1111111133",
                        IdentityNumber = "1111111133",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "Distributor@moe.sa",
                        Enabled = true,
                        EmployeeNo = "3234567893",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000014",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.DataEntry.Code
                            }
                        },
                        UserClaims =new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // DepartmentManager
                    new AppUser
                    {
                        Id = new Guid("dfc67d25-f173-4d24-bedd-a4bc2e7cee5d"),
                        CreatedOn = now,
                        FirstName = "منصور",
                        LastName = "البقمي",
                        UserName = "1111111124",
                        NormalizedUserName = "1111111124",
                        IdentityNumber = "1111111124",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "InvestigationManager@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567894",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000015",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.DepartmentManager.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Investigation
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // Investigator
                    new AppUser
                    {
                        Id = new Guid("862b9377-9fcc-4193-ad5c-05251ebe65e7"),
                        CreatedOn = now,
                        FirstName = "سامي",
                        LastName = "الجندل",
                        UserName = "1111111125",
                        NormalizedUserName = "1111111125",
                        IdentityNumber = "1111111125",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "Investigator@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567895",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000016",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.Investigator.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // Investigator
                    new AppUser
                    {
                        Id = new Guid("DA822EDE-5B47-4527-ADEF-72D07C9C076E"),
                        CreatedOn = now,
                        FirstName = "فهد",
                        LastName = "السيوفي",
                        UserName = "1111111126",
                        NormalizedUserName = "1111111126",
                        IdentityNumber = "1111111126",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "Investigator2@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567896",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000017",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.Investigator.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // DepartmentManager
                    new AppUser
                    {
                        Id = new Guid("683A1BFC-7FC6-4119-88EC-4C63D2EFA92B"),
                        CreatedOn = now,
                        FirstName = "محمد",
                        LastName = "أحمد",
                        UserName = "1111111127",
                        NormalizedUserName = "1111111127",
                        IdentityNumber = "1111111127",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "ConsultingAndLegalStudiesManager@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567897",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000018",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.DepartmentManager.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Consulting
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // DepartmentManager
                    new AppUser
                    {
                        Id = new Guid("8C589966-4E35-4747-A3FC-D0C0BE878AC5"),
                        CreatedOn = now,
                        FirstName = "محمود",
                        LastName = "أحمد",
                        UserName = "1111111128",
                        NormalizedUserName = "1111111128",
                        IdentityNumber = "1111111128",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "GrievancesAndLegalSettlementsManager@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567898",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000019",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.DepartmentManager.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Grievances
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // DepartmentManager
                    new AppUser
                    {
                        Id = new Guid("AD68BC19-6D82-4E3E-B7E8-945865F9EBC9"),
                        CreatedOn = now,
                        FirstName = "حسن",
                        LastName = "أحمد",
                        UserName = "1111111129",
                        NormalizedUserName = "1111111129",
                        IdentityNumber = "1111111129",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "ContractsAndLegalAgreementsManager@moe.sa",
                        Enabled = true,
                        EmployeeNo = "2234567899",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000020",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.DepartmentManager.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.Contracts
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // DepartmentManager
                    new AppUser
                    {
                        Id = new Guid("90EA8133-834A-4E59-88EE-A952A1247CA1"),
                        CreatedOn = now,
                        FirstName = "حسين",
                        LastName = "أحمد",
                        UserName = "1111111130",
                        NormalizedUserName = "1111111130",
                        IdentityNumber = "1111111130",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "HumanRightsManager@moe.sa",
                        Enabled = true,
                        EmployeeNo= "3234567890",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000021",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.DepartmentManager.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.HumanRights
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // DepartmentManager
                    new AppUser
                    {
                        Id = new Guid("5B62CD74-0144-4CF5-9EE7-B59E42724A41"),
                        CreatedOn = now,
                        FirstName = "مصطفى",
                        LastName = "أحمد",
                        UserName = "1111111131",
                        NormalizedUserName = "1111111131",
                        IdentityNumber = "1111111131",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "SystemsAndRegulationsAndDecisionsManager@moe.sa",
                        Enabled = true,
                        EmployeeNo = "3234567891",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000022",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.DepartmentManager.Code,
                                UserRoleDepartmets = new List<UserRoleDepartment>
                                {
                                    new UserRoleDepartment
                                    {
                                        CreatedOn = now,
                                        CreatedBy = ApplicationConstants.SystemAdministratorId,
                                        DepartmentId = (int) Departments.RegulationsAndLaws
                                    }
                                }
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    },
                    // AdministrativeCommunicationSpecialist
                    new AppUser
                    {
                        Id = new Guid("CE7AA602-9F12-419F-9743-0E856F1E4195"),
                        CreatedOn = now,
                        FirstName = "علي",
                        LastName = "أحمد",
                        UserName = "1111111132",
                        NormalizedUserName = "1111111132",
                        IdentityNumber = "1111111132",
                        PasswordHash = "AQAAAAEAACcQAAAAEDN+G0Jfr6BWHPnNUAbu/8tlSkd1TsiBuYtyzlvO2EU3XqeoFQe+4KZ1dEoMDiGGbw==",
                        Email = "AdministrativeCommunicationSpecialist@moe.sa",
                        Enabled = true,
                        EmployeeNo = "3234567892",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        BranchId = MinistryBranchId,
                        PhoneNumber = "500000023",
                        UserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = ApplicationRolesConstants.AdministrativeCommunicationSpecialist.Code
                            }
                        },
                        UserClaims = new List<AppUserClaim>
                        {
                            new AppUserClaim
                            {
                                ClaimType = "Branch",
                                ClaimValue = MinistryBranchId.ToString()
                            }
                        }
                    }
                };

                users.AddRange(devUsers);
            }

            return users.ToArray();
        }

        public static City[] CreateCities()
        {
            return new[]
            {
                new City { Id = 1, Name = "الباحة", ProvinceId = 1, CreatedOn = now },
                new City { Id = 2, Name = "سكاكا", ProvinceId = 2, CreatedOn = now },
                new City { Id = 3, Name = "القريات", ProvinceId = 2, CreatedOn = now },
                new City { Id = 4, Name = "عرعر", ProvinceId = 3, CreatedOn = now },
                new City { Id = 5, Name = "الرياض", ProvinceId = 4, CreatedOn = now },
                new City { Id = 6, Name = "الخرج", ProvinceId = 4, CreatedOn = now },
                new City { Id = 7, Name = "الزلفي", ProvinceId = 4, CreatedOn = now },
                new City { Id = 8, Name = "وادي الدواسر", ProvinceId = 4, CreatedOn = now },
                new City { Id = 9, Name = "الدوادمي", ProvinceId = 4, CreatedOn = now },
                new City { Id = 10, Name = "الحوطة", ProvinceId = 4, CreatedOn = now },
                new City { Id = 11, Name = "الأفلاج", ProvinceId = 4, CreatedOn = now },
                new City { Id = 12, Name = "الاحساء", ProvinceId = 5, CreatedOn = now },
                new City { Id = 13, Name = "القطيف", ProvinceId = 5, CreatedOn = now },
                new City { Id = 14, Name = "الهفوف", ProvinceId = 5, CreatedOn = now },
                new City { Id = 15, Name = "المبرز", ProvinceId = 5, CreatedOn = now },
                new City { Id = 16, Name = "حفر الباطن", ProvinceId = 5, CreatedOn = now },
                new City { Id = 17, Name = "الجبيل", ProvinceId = 5, CreatedOn = now },
                new City { Id = 18, Name = "الثقبة", ProvinceId = 5, CreatedOn = now },
                new City { Id = 19, Name = "الخبر", ProvinceId = 5, CreatedOn = now },
                new City { Id = 20, Name = "الظهران", ProvinceId = 5, CreatedOn = now },
                new City { Id = 21, Name = "سيهات", ProvinceId = 5, CreatedOn = now },
                new City { Id = 22, Name = "تاروت", ProvinceId = 5, CreatedOn =now },
                new City { Id = 23, Name = "عنيزة", ProvinceId = 6, CreatedOn = now },
                new City { Id = 24, Name = "المدينة المنورة", ProvinceId = 7, CreatedOn = now },
                new City { Id = 25, Name = "ينبع البحر", ProvinceId = 7, CreatedOn = now },
                new City { Id = 26, Name = "الفريش", ProvinceId = 7, CreatedOn = now },
                new City { Id = 27, Name = "تبوك", ProvinceId = 8, CreatedOn = now },
                new City { Id = 28, Name = "ضباء", ProvinceId = 8, CreatedOn = now },
                new City { Id = 29, Name = "الدمام", ProvinceId = 5, CreatedOn = now },
                new City { Id = 30, Name = "الرس", ProvinceId = 6, CreatedOn = now },
                new City { Id = 31, Name = "جيزان", ProvinceId = 9, CreatedOn = now },
                new City { Id = 32, Name = "صبياء", ProvinceId = 9, CreatedOn = now },
                new City { Id = 33, Name = "بيش", ProvinceId = 9, CreatedOn = now },
                new City { Id = 34, Name = "خميس مشيط", ProvinceId = 10, CreatedOn = now },
                new City { Id = 35, Name = "بيشه", ProvinceId = 10, CreatedOn = now },
                new City { Id = 36, Name = "أحد رفيدة", ProvinceId = 10, CreatedOn = now },
                new City { Id = 37, Name = "بارق", ProvinceId = 10, CreatedOn = now },
                new City { Id = 38, Name = "جدة", ProvinceId = 11, CreatedOn = now },
                new City { Id = 39, Name = "مكة", ProvinceId = 11, CreatedOn = now },
                new City { Id = 40, Name = "المظيلف", ProvinceId = 11, CreatedOn = now },
                new City { Id = 41, Name = "الطائف", ProvinceId = 11, CreatedOn = now },
                new City { Id = 42, Name = "الحوية", ProvinceId = 11, CreatedOn =now },
                new City { Id = 43, Name = "بحره", ProvinceId = 11, CreatedOn = now },
                new City { Id = 44, Name = "نجران", ProvinceId = 12, CreatedOn = now },
                new City { Id = 45, Name = "شروره", ProvinceId = 12, CreatedOn = now },
                new City { Id = 46, Name = "حائل", ProvinceId = 13, CreatedOn = now },
                new City { Id = 47, Name = "الغاط", ProvinceId = 6, CreatedOn = now },
                new City { Id = 48, Name = "محايل عسير", ProvinceId = 10, CreatedOn = now }
            };
        }

        public static Court[] CreateCourts()
        {
            return new[]
            {
                new Court { Id = 1, Name = "المحكمة العامة", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 2, Name = "المحكمة الإدارية بالرياض", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 3, Name = "المحكمة الإدارية العليا", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 4, Name = "محكمة الاستئناف الإدارية بالرياض", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.Appeal, CreatedOn = now },
                new Court { Id = 5, Name = "المحكمة الجزائية", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 6, Name = "الهيئة العمالية الابتدائية", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 7, Name = "الهيئة العمالية العليا", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 8, Name = "لجان التسوية العمالية", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 9, Name = "لجنة النظر في مخالفات المطبوعات والنشر", CourtCategory = CourtCategories.QuasiJudicialCommittees, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 10, Name = "محكمة التنفيذ", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 11, Name = "محكمة الاحوال الشخصية", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 12, Name = "الدوائر الحقوقية", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 13, Name = "لجنة المنازعات المصرفية", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 14, Name = "المحكمة الإدارية بجدة", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 15, Name = "محكمة الاستئناف بمكة المكرمة", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.Appeal, CreatedOn = now },
                new Court { Id = 16, Name = "المحكمة الإدارية بمكة المكرمة", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 17, Name = "المحكمة الإدارية بالدمام", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 18, Name = "محكمة الاستئناف الإدارية بالمنطقة الشرقية", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.Appeal, CreatedOn = now },
                new Court { Id = 19, Name = "المحكمة الإدارية بجازان", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 20, Name = "المحكمة الإدارية بأبها", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 21, Name = "المحكمة الإدارية بسكاكا", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 22, Name = "المحكمة الإدارية ببريدة", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 23, Name = "المحكمة الإدارية بحائل", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 24, Name = "المحكمة الإدارية بعرعر", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 25, Name = "المحكمة الإدارية بنجران", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 26, Name = "المحكمة الإدارية بالباحة", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 27, Name = "المحكمة الإدارية بتبوك", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 28, Name = "المحكمة الإدارية بالمدينة المنورة", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 29, Name = "محكمة الاستنئناف العامة بمكة المكرمة", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 30, Name = "محكمة الاستئناف الادارية بالمدينة المنورة", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.Appeal, CreatedOn = now },
                new Court { Id = 31, Name = "المحكمة العامة بالأحساء", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 32, Name = "محكمة الاستئناف الإدارية بمنطقة عسير", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.Appeal, CreatedOn = now },
                new Court { Id = 33, Name = "المحكمة الإدارية بوادي الدواسر", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 34, Name = "المحكمة الإدارية بحفر الباطن", CourtCategory = CourtCategories.HouseOfGrievances, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 35, Name = "المحكمة الجزائية المتخصصة", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
                new Court { Id = 36, Name = "المحكمة العمالية بالرياض", CourtCategory = CourtCategories.MinistryOfJustice, LitigationType = LitigationTypes.FirstInstance, CreatedOn = now },
            };
        }

        private static IdentityType[] CreateIdentityTypes()
        {
            return new[]
            {
                new IdentityType{ Id = 1, Name = "هوية وطنية", CreatedOn = now },
                new IdentityType{ Id = 2, Name = "إقامة", CreatedOn = now },
                new IdentityType{ Id = 3, Name = "هوية خليجية", CreatedOn = now },
                new IdentityType{ Id = 4, Name = "هوية زائر", CreatedOn = now },
                //new IdentityType{ Id = 5, Name = "جواز سفر", CreatedOn = now },
                new IdentityType{ Id = 6, Name = "سجل تجاري", CreatedOn = now },
                new IdentityType{ Id = 7, Name = "رقم الحدود", CreatedOn = now }
            };
        }

        public static JobTitle[] CreateJobTitles()
        {
            return new[]
            {
                new JobTitle { Id = 1, Name = "مشرف", CreatedOn = now },
                new JobTitle { Id = 2, Name = "باحث", CreatedOn = now },
                new JobTitle { Id = 3, Name = "مستشار", CreatedOn = now },
                new JobTitle { Id = 4, Name = "محامي", CreatedOn = now },
                new JobTitle { Id = 5, Name = "محاسب", CreatedOn = now },
                new JobTitle { Id = 6, Name = "اداري", CreatedOn = now },
                new JobTitle { Id = 7, Name = "سكرتير", CreatedOn = now },
                new JobTitle { Id = 8, Name = "متدرب", CreatedOn = now },
                new JobTitle { Id = 9, Name = "عامل", CreatedOn = now },

            };
        }

        private static PartyType[] CreatePartyTypes()
        {

            return new[]
            {
                new PartyType{ Id = 1, Name = "فرد", CreatedOn = now },
                new PartyType{ Id = 2, Name = "جهة حكومية", CreatedOn = now },
                new PartyType{ Id = 3, Name = "شركة او مؤسسة", CreatedOn = now }
            };
        }

        private static PartyEntityType[] CreatePartyEntityTypes()
        {

            return new[]
            {
                new PartyEntityType{ Id = (int)PartyEntityTypes.Person, Name = "فرد", CreatedOn = now },
                new PartyEntityType{ Id = (int)PartyEntityTypes.Government, Name = "جهة حكومية", CreatedOn = now },
                new PartyEntityType{ Id = (int)PartyEntityTypes.Organization, Name = "شركة", CreatedOn = now }
            };
        }

        public static Province[] CreateProvinces()
        {
            return new[]
            {
                new Province { Id = 1, Name = "الباحة", CreatedOn = now },
                new Province { Id = 2, Name = "الجوف", CreatedOn = now },
                new Province { Id = 3, Name = "الشمالية", CreatedOn = now },
                new Province { Id = 4, Name = "الرياض", CreatedOn = now },
                new Province { Id = 5, Name = "الشرقية", CreatedOn = now },
                new Province { Id = 6, Name = "القصيم", CreatedOn = now },
                new Province { Id = 7, Name = "المدينة", CreatedOn = now},
                new Province { Id = 8, Name = "تبوك", CreatedOn = now },
                new Province { Id = 9, Name = "جيزان", CreatedOn = now },
                new Province { Id = 10, Name = "عسير", CreatedOn = now },
                new Province { Id = 11, Name = "مكة", CreatedOn = now },
                new Province { Id = 12, Name = "نجران", CreatedOn = now },
                new Province { Id = 13, Name = "حائل", CreatedOn = now }
            };
        }

        private static Country[] CreateCountries()
        {
            return new[]
            {
                new Country { Id = 1, NameEn = "Afghanistan", NameAr = "أفغانستان", NationalityEn = "Afghan", NationalityAr = "أفغاني", ISO31661CodeAlph3 = "AFG" , CreatedOn = now },
                new Country { Id = 2, NameEn = "Albania", NameAr = "ألبانيا", NationalityEn = "Albanian", NationalityAr = "ألباني", ISO31661CodeAlph3 = "ALB" , CreatedOn = now },
                new Country { Id = 3, NameEn = "Algeria", NameAr = "الجزائر", NationalityEn = "Algerian", NationalityAr = "جزائري", ISO31661CodeAlph3 = "DZA" , CreatedOn = now },
                new Country { Id = 4, NameEn = "American Samoa", NameAr = "ساموا الأمريكية", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "ASM" , CreatedOn = now },
                new Country { Id = 5, NameEn = "Andorra", NameAr = "أندورا", NationalityEn = "Andorran", NationalityAr = "أندورا", ISO31661CodeAlph3 = "AND" , CreatedOn = now },
                new Country { Id = 6, NameEn = "Angola", NameAr = "أنغولا", NationalityEn = "Angolan", NationalityAr = "أنغولي", ISO31661CodeAlph3 = "AGO" , CreatedOn = now },
                //new Country { Id = 7, NameEn = "Anguilla", NameAr = "AI", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "AIA" , CreatedOn = now },
                //new Country { Id = 8, NameEn = "Antarctica", NameAr = "AQ", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "ATA" , CreatedOn = now },
                //new Country { Id = 9, NameEn = "Antigua and Barbuda", NameAr = "AG", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "ATG" , CreatedOn = now },
                new Country { Id = 10, NameEn = "Argentina", NameAr = "الأرجنتين", NationalityEn = "Argentinian", NationalityAr = "أرجنتيني", ISO31661CodeAlph3 = "ARG" , CreatedOn = now },
                new Country { Id = 11, NameEn = "Armenia", NameAr = "أرمينيا", NationalityEn = "Armenian", NationalityAr = "أرماني", ISO31661CodeAlph3 = "ARM" , CreatedOn = now },
                //new Country { Id = 12, NameEn = "Aruba", NameAr = "AW", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "ABW" , CreatedOn = now },
                new Country { Id = 13, NameEn = "Australia", NameAr = "أستراليا", NationalityEn = "Australian", NationalityAr = "أسترالي", ISO31661CodeAlph3 = "AUS" , CreatedOn = now },
                new Country { Id = 14, NameEn = "Austria", NameAr = "النمسا", NationalityEn = "Austrian", NationalityAr = "نمساوي", ISO31661CodeAlph3 = "AUT" , CreatedOn = now },
                new Country { Id = 15, NameEn = "Azerbaijan", NameAr = "أذربيجان", NationalityEn = "Azerbaijani", NationalityAr = "أذربيجاني", ISO31661CodeAlph3 = "AZE" , CreatedOn = now },
                new Country { Id = 16, NameEn = "Bahamas", NameAr = "جزر البهاما", NationalityEn = "Bahamian", NationalityAr = "بهامي", ISO31661CodeAlph3 = "BHS" , CreatedOn = now },
                new Country { Id = 17, NameEn = "Bahrain", NameAr = "البحرين", NationalityEn = "بحريني", NationalityAr = "Bahraini", ISO31661CodeAlph3 = "BHR" , CreatedOn = now },
                new Country { Id = 18, NameEn = "Bangladesh", NameAr = "بنجلاديش", NationalityEn = "Bangladeshi", NationalityAr = "بنجلاديشي", ISO31661CodeAlph3 = "BGD" , CreatedOn = now },
                new Country { Id = 19, NameEn = "Barbados", NameAr = "بربادوس", NationalityEn = "Barbadian", NationalityAr = "بربادوسي", ISO31661CodeAlph3 = "BRB" , CreatedOn = now },
                new Country { Id = 20, NameEn = "Belarus", NameAr = "بيلاروسيا", NationalityEn = "Belarusian", NationalityAr = "بيلاروسي", ISO31661CodeAlph3 = "BLR" , CreatedOn = now },
                new Country { Id = 21, NameEn = "Belgium", NameAr = "بلجيكا", NationalityEn = "Belgian", NationalityAr = "بلجيكي", ISO31661CodeAlph3 = "BEL" , CreatedOn = now },
                new Country { Id = 22, NameEn = "Belize", NameAr = "بليز", NationalityEn = "Belizean", NationalityAr = "بليزي", ISO31661CodeAlph3 = "BLZ" , CreatedOn = now },
                new Country { Id = 23, NameEn = "Benin", NameAr = "بنين", NationalityEn = "Beninese", NationalityAr = "بنيني", ISO31661CodeAlph3 = "BEN" , CreatedOn = now },
                //new Country { Id = 24, NameEn = "Bermuda", NameAr = "BM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "BMU" , CreatedOn = now },
                new Country { Id = 25, NameEn = "Bhutan", NameAr = "بوتان", NationalityEn = "Bhutanese", NationalityAr = "بوتاني", ISO31661CodeAlph3 = "BTN" , CreatedOn = now },
                new Country { Id = 26, NameEn = "Bolivia", NameAr = "بوليفيا", NationalityEn = "Bolivian", NationalityAr = "بوليفي", ISO31661CodeAlph3 = "BOL" , CreatedOn = now },
                //new Country { Id = 27, NameEn = "Bonaire, Sint Eustatius and Saba", NameAr = "BQ", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "BES" , CreatedOn = now },
                new Country { Id = 28, NameEn = "Bosnia and Herzegovina", NameAr = "البوسنة والهرسك", NationalityEn = "Bosnian", NationalityAr = "بوسني", ISO31661CodeAlph3 = "BIH" , CreatedOn = now },
                new Country { Id = 29, NameEn = "Botswana", NameAr = "بوتسوانا", NationalityEn = "Botswanan", NationalityAr = "بوتسواني", ISO31661CodeAlph3 = "BWA" , CreatedOn = now },
                new Country { Id = 30, NameEn = "Bouvet Island", NameAr = "BV", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "BVT" , CreatedOn = now },
                new Country { Id = 31, NameEn = "Brazil", NameAr = "البرازيل", NationalityEn = "Brazilian", NationalityAr = "برازيلي", ISO31661CodeAlph3 = "BRA" , CreatedOn = now },
                //new Country { Id = 32, NameEn = "British Indian Ocean Territory", NameAr = "IO", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "IOT" , CreatedOn = now },
                new Country { Id = 33, NameEn = "Brunei Darussalam", NameAr = "بروناي", NationalityEn = "Bruneian", NationalityAr = "بروناي", ISO31661CodeAlph3 = "BRN" , CreatedOn = now },
                new Country { Id = 34, NameEn = "Bulgaria", NameAr = "بلغاريا", NationalityEn = "Bulgarian", NationalityAr = "بلغاري", ISO31661CodeAlph3 = "BGR" , CreatedOn = now },
                new Country { Id = 35, NameEn = "Burkina Faso", NameAr = "بوركينا", NationalityEn = "Burkinese", NationalityAr = "بوركيني", ISO31661CodeAlph3 = "BFA" , CreatedOn = now },
                new Country { Id = 36, NameEn = "Burundi", NameAr = "بوروندي", NationalityEn = "Burundian", NationalityAr = "بوروندي", ISO31661CodeAlph3 = "BDI" , CreatedOn = now },
                //new Country { Id = 37, NameEn = "Cabo Verde", NameAr = "CV", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "CPV" , CreatedOn = now },
                new Country { Id = 38, NameEn = "Cambodia", NameAr = "كمبوديا", NationalityEn = "Cambodian", NationalityAr = "كمبودي", ISO31661CodeAlph3 = "KHM" , CreatedOn = now },
                new Country { Id = 39, NameEn = "Cameroon", NameAr = "كاميرون", NationalityEn = "Cameroonian", NationalityAr = "كاميروني", ISO31661CodeAlph3 = "CMR" , CreatedOn = now },
                new Country { Id = 40, NameEn = "Canada", NameAr = "كندا", NationalityEn = "Canadian", NationalityAr = "كندي", ISO31661CodeAlph3 = "CAN" , CreatedOn = now },
                //new Country { Id = 41, NameEn = "Cayman Islands", NameAr = "KY", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "CYM" , CreatedOn = now },
                //new Country { Id = 42, NameEn = "Central African Republic", NameAr = "CF", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "CAF" , CreatedOn = now },
                new Country { Id = 43, NameEn = "Chad", NameAr = "تشاد", NationalityEn = "Chadian", NationalityAr = "تشادي", ISO31661CodeAlph3 = "TCD" , CreatedOn = now },
                new Country { Id = 44, NameEn = "Chile", NameAr = "تشيلي", NationalityEn = "Chilean", NationalityAr = "شيلي", ISO31661CodeAlph3 = "CHL" , CreatedOn = now },
                new Country { Id = 45, NameEn = "China", NameAr = "الصين", NationalityEn = "Chinese", NationalityAr = "صيني", ISO31661CodeAlph3 = "CHN" , CreatedOn = now },
                //new Country { Id = 46, NameEn = "Christmas Island", NameAr = "CX", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "CXR" , CreatedOn = now },
                //new Country { Id = 47, NameEn = "Cocos (Keeling) Islands", NameAr = "CC", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "CCK" , CreatedOn = now },
                new Country { Id = 48, NameEn = "Colombia", NameAr = "كولومبيا", NationalityEn = "Colombian", NationalityAr = "كولومبي", ISO31661CodeAlph3 = "COL" , CreatedOn = now },
                //new Country { Id = 49, NameEn = "Comoros", NameAr = "KM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "COM" , CreatedOn = now },
                //new Country { Id = 50, NameEn = "Congo (the Democratic Republic of the)", NameAr = "CD", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "COD" , CreatedOn = now },
                new Country { Id = 51, NameEn = "Congo", NameAr = "كونغو", NationalityEn = "Congolese", NationalityAr = "كونغولي", ISO31661CodeAlph3 = "COG" , CreatedOn = now },
                new Country { Id = 52, NameEn = "Cook Islands", NameAr = "CK", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "COK" , CreatedOn = now },
                new Country { Id = 53, NameEn = "Costa Rica", NameAr = "كوستا ريكا", NationalityEn = "Costa Rican", NationalityAr = "كوستاريكي", ISO31661CodeAlph3 = "CRI" , CreatedOn = now },
                new Country { Id = 54, NameEn = "Croatia", NameAr = "كرواتيا", NationalityEn = "Croatian", NationalityAr = "كرواتي", ISO31661CodeAlph3 = "HRV" , CreatedOn = now },
                new Country { Id = 55, NameEn = "Cuba", NameAr = "كوبا", NationalityEn = "Cuban", NationalityAr = "كوبي", ISO31661CodeAlph3 = "CUB" , CreatedOn = now },
                //new Country { Id = 56, NameEn = "Curaçao", NameAr = "CW", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "CUW" , CreatedOn = now },
                new Country { Id = 57, NameEn = "Cyprus", NameAr = "قبرص", NationalityEn = "Cypriot", NationalityAr = "قبرصي", ISO31661CodeAlph3 = "CYP" , CreatedOn = now },
                new Country { Id = 58, NameEn = "Czechia", NameAr = "الجمهرية التشيكية", NationalityEn = "Czech", NationalityAr = "تشيكي", ISO31661CodeAlph3 = "CZE" , CreatedOn = now },
                //new Country { Id = 59, NameEn = "Côte d'Ivoire", NameAr = "CI", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "CIV" , CreatedOn = now },
                new Country { Id = 60, NameEn = "Denmark", NameAr = "الدانمارك", NationalityEn = "Danish", NationalityAr = "دانماركي", ISO31661CodeAlph3 = "DNK" , CreatedOn = now },
                new Country { Id = 61, NameEn = "Djibouti", NameAr = "جيبوتي", NationalityEn = "Djiboutian", NationalityAr = "جيبوتي", ISO31661CodeAlph3 = "DJI" , CreatedOn = now },
                new Country { Id = 62, NameEn = "Dominica", NameAr = "دومينيكا", NationalityEn = "Dominican", NationalityAr = "دومينيكاني", ISO31661CodeAlph3 = "DMA" , CreatedOn = now },
                new Country { Id = 63, NameEn = "Dominican Republic", NameAr = "جمهورية الدومينيكان", NationalityEn = "Dominican", NationalityAr = "دومينيكاني", ISO31661CodeAlph3 = "DOM" , CreatedOn = now },
                new Country { Id = 64, NameEn = "Ecuador", NameAr = "إكوادور", NationalityEn = "Ecuadorean", NationalityAr = "إكوادورري", ISO31661CodeAlph3 = "ECU" , CreatedOn = now },
                new Country { Id = 65, NameEn = "Egypt", NameAr = "مصر", NationalityEn = "Egyptian", NationalityAr = "مصري", ISO31661CodeAlph3 = "EGY" , CreatedOn = now },
                new Country { Id = 66, NameEn = "El Salvador", NameAr = "السلفادور", NationalityEn = "Salvadorean", NationalityAr = "سلفادوري", ISO31661CodeAlph3 = "SLV" , CreatedOn = now },
                //new Country { Id = 67, NameEn = "Equatorial Guinea", NameAr = "GQ", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "GNQ" , CreatedOn = now },
                new Country { Id = 68, NameEn = "Eritrea", NameAr = "إريتريا", NationalityEn = "Eritrean", NationalityAr = "إريتري", ISO31661CodeAlph3 = "ERI" , CreatedOn = now },
                new Country { Id = 69, NameEn = "Estonia", NameAr = "إستونيا", NationalityEn = "Estonian", NationalityAr = "إستوني", ISO31661CodeAlph3 = "EST" , CreatedOn = now },
                //new Country { Id = 70, NameEn = "Eswatini", NameAr = "SZ", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SWZ" , CreatedOn = now },
                new Country { Id = 71, NameEn = "Ethiopia", NameAr = "إثيوبيا", NationalityEn = "Ethiopian", NationalityAr = "إثيوبي", ISO31661CodeAlph3 = "ETH" , CreatedOn = now },
                //new Country { Id = 72, NameEn = "Falkland Islands (Malvinas)", NameAr = "FK", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "FLK" , CreatedOn = now },
                //new Country { Id = 73, NameEn = "Faroe Islands", NameAr = "FO", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "FRO" , CreatedOn = now },
                new Country { Id = 74, NameEn = "Fiji", NameAr = "فيجي", NationalityEn = "Fijian", NationalityAr = "فيجي", ISO31661CodeAlph3 = "FJI" , CreatedOn = now },
                new Country { Id = 75, NameEn = "Finland", NameAr = "فنلندا", NationalityEn = "Finnish", NationalityAr = "فنلندي", ISO31661CodeAlph3 = "FIN" , CreatedOn = now },
                new Country { Id = 76, NameEn = "France", NameAr = "فرنسا", NationalityEn = "French", NationalityAr = "فرنسي", ISO31661CodeAlph3 = "FRA" , CreatedOn = now },
                //new Country { Id = 77, NameEn = "French Guiana", NameAr = "GF", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "GUF" , CreatedOn = now },
                //new Country { Id = 78, NameEn = "French Polynesia", NameAr = "PF", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "PYF" , CreatedOn = now },
                //new Country { Id = 79, NameEn = "French Southern Territories", NameAr = "TF", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "ATF" , CreatedOn = now },
                new Country { Id = 80, NameEn = "Gabon", NameAr = "غابون", NationalityEn = "Gabonese", NationalityAr = "غابوني", ISO31661CodeAlph3 = "GAB" , CreatedOn = now },
                new Country { Id = 81, NameEn = "Gambia", NameAr = "غامبيا", NationalityEn = "Gambian", NationalityAr = "غامبي", ISO31661CodeAlph3 = "GMB" , CreatedOn = now },
                new Country { Id = 82, NameEn = "Georgia", NameAr = "جورجيا", NationalityEn = "Georgian", NationalityAr = "جورجي", ISO31661CodeAlph3 = "GEO" , CreatedOn = now },
                new Country { Id = 83, NameEn = "Germany", NameAr = "ألمانيا", NationalityEn = "German", NationalityAr = "ألماني", ISO31661CodeAlph3 = "DEU" , CreatedOn = now },
                new Country { Id = 84, NameEn = "Ghana", NameAr = "غانا", NationalityEn = "Ghanaian", NationalityAr = "غاني", ISO31661CodeAlph3 = "GHA" , CreatedOn = now },
                //new Country { Id = 85, NameEn = "Gibraltar", NameAr = "GI", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "GIB" , CreatedOn = now },
                new Country { Id = 86, NameEn = "Greece", NameAr = "اليونان", NationalityEn = "Greek", NationalityAr = "يوناني", ISO31661CodeAlph3 = "GRC" , CreatedOn = now },
                //new Country { Id = 87, NameEn = "Greenland", NameAr = "GL", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "GRL" , CreatedOn = now },
                new Country { Id = 88, NameEn = "Grenada", NameAr = "غرينادا", NationalityEn = "Grenadian", NationalityAr = "غرينادي", ISO31661CodeAlph3 = "GRD" , CreatedOn = now },
                //new Country { Id = 89, NameEn = "Guadeloupe", NameAr = "GP", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "GLP" , CreatedOn = now },
                new Country { Id = 90, NameEn = "Guam", NameAr = "GU", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "GUM" , CreatedOn = now },
                new Country { Id = 91, NameEn = "Guatemala", NameAr = "غواتيمالا", NationalityEn = "Guatemalan", NationalityAr = "غواتيمالي", ISO31661CodeAlph3 = "GTM" , CreatedOn = now },
                //new Country { Id = 92, NameEn = "Guernsey", NameAr = "GG", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "GGY" , CreatedOn = now },
                new Country { Id = 93, NameEn = "Guinea", NameAr = "غينيا", NationalityEn = "Guinean", NationalityAr = "غيني", ISO31661CodeAlph3 = "GIN" , CreatedOn = now },
                //new Country { Id = 94, NameEn = "Guinea-Bissau", NameAr = "GW", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "GNB" , CreatedOn = now },
                new Country { Id = 95, NameEn = "Guyana", NameAr = "غيانا", NationalityEn = "Guyanese", NationalityAr = "غياني", ISO31661CodeAlph3 = "GUY" , CreatedOn = now },
                new Country { Id = 96, NameEn = "Haiti", NameAr = "هايتي", NationalityEn = "Haitian", NationalityAr = "هايتي", ISO31661CodeAlph3 = "GTI" , CreatedOn = now },
                //new Country { Id = 97, NameEn = "Heard Island and McDonald Islands", NameAr = "HM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "HMD" , CreatedOn = now },
                //new Country { Id = 98, NameEn = "Holy See", NameAr = "VA", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "VAT" , CreatedOn = now },
                new Country { Id = 99, NameEn = "Honduras", NameAr = "هندوراس", NationalityEn = "Honduran", NationalityAr = "هندوراسي", ISO31661CodeAlph3 = "HND" , CreatedOn = now },
                //new Country { Id = 100, NameEn = "Hong Kong", NameAr = "HK", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "HKG" , CreatedOn = now },
                new Country { Id = 101, NameEn = "Hungary", NameAr = "هنغاريا", NationalityEn = "Hungarian", NationalityAr = "هنغاري", ISO31661CodeAlph3 = "HUN" , CreatedOn = now },
                new Country { Id = 102, NameEn = "Iceland", NameAr = "أيسلندا", NationalityEn = "Icelandic", NationalityAr = "أيسلندي", ISO31661CodeAlph3 = "ISL" , CreatedOn = now },
                new Country { Id = 103, NameEn = "India", NameAr = "الهند", NationalityEn = "Indian", NationalityAr = "هندي", ISO31661CodeAlph3 = "IND" , CreatedOn = now },
                new Country { Id = 104, NameEn = "Iran", NameAr = "إيران", NationalityEn = "Iranian", NationalityAr = "إيراني", ISO31661CodeAlph3 = "IRN" , CreatedOn = now },
                new Country { Id = 105, NameEn = "Iraq", NameAr = "العراق", NationalityEn = "Iraqi", NationalityAr = "عراقي", ISO31661CodeAlph3 = "IRQ" , CreatedOn = now },
                new Country { Id = 106, NameEn = "Ireland", NameAr = "أيرلندا", NationalityEn = "Irish", NationalityAr = "أيرلندي", ISO31661CodeAlph3 = "IRL" , CreatedOn = now },
                //new Country { Id = 107, NameEn = "Isle of Man", NameAr = "IM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "IMN" , CreatedOn = now },
                new Country { Id = 108, NameEn = "Italy", NameAr = "إيطاليا", NationalityEn = "Italian", NationalityAr = "إيطالي", ISO31661CodeAlph3 = "ITA" , CreatedOn = now },
                new Country { Id = 109, NameEn = "Jamaica", NameAr = "جامايكا", NationalityEn = "Jamaican", NationalityAr = "جامايكي", ISO31661CodeAlph3 = "JAM" , CreatedOn = now },
                new Country { Id = 110, NameEn = "Japan", NameAr = "اليابان", NationalityEn = "Japanese", NationalityAr = "ياباني", ISO31661CodeAlph3 = "JPN" , CreatedOn = now },
                //new Country { Id = 111, NameEn = "Jersey", NameAr = "JE", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "JEY" , CreatedOn = now },
                new Country { Id = 112, NameEn = "Jordan", NameAr = "الأردن", NationalityEn = "Jordanian", NationalityAr = "أردني", ISO31661CodeAlph3 = "JOR" , CreatedOn = now },
                new Country { Id = 113, NameEn = "Kazakhstan", NameAr = "كازاخستان", NationalityEn = "Kazakh", NationalityAr = "كازاخستاني", ISO31661CodeAlph3 = "KAZ" , CreatedOn = now },
                new Country { Id = 114, NameEn = "Kenya", NameAr = "كينيا", NationalityEn = "Kenyan", NationalityAr = "كيني", ISO31661CodeAlph3 = "KEN" , CreatedOn = now },
                //new Country { Id = 115, NameEn = "Kenya", NameAr = "KE", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "KEN" , CreatedOn = now },
                //new Country { Id = 116, NameEn = "Kiribati", NameAr = "KI", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "KIR" , CreatedOn = now },
                //new Country { Id = 117, NameEn = "Korea (Democratic People's Republic of)", NameAr = "KP", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "PRK" , CreatedOn = now },
                //new Country { Id = 118, NameEn = "Korea (the Republic of)", NameAr = "KR", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "KOR" , CreatedOn = now },
                new Country { Id = 119, NameEn = "Kuwait", NameAr = "الكويت", NationalityEn = "Kuwaiti", NationalityAr = "كويتي", ISO31661CodeAlph3 = "KWT" , CreatedOn = now },
                //new Country { Id = 120, NameEn = "Kyrgyzstan", NameAr = "KG", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "KGZ" , CreatedOn = now },
                //new Country { Id = 121, NameEn = "Lao People's Democratic Republic", NameAr = "LA", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "LAO" , CreatedOn = now },
                new Country { Id = 122, NameEn = "Latvia", NameAr = "لاتفيا", NationalityEn = "Latvian", NationalityAr = "لاتفي", ISO31661CodeAlph3 = "LVA" , CreatedOn = now },
                new Country { Id = 123, NameEn = "Lebanon", NameAr = "لبنان", NationalityEn = "Lebanese", NationalityAr = "لبناني", ISO31661CodeAlph3 = "LBN" , CreatedOn = now },
                new Country { Id = 124, NameEn = "Lesotho", NameAr = "LS", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "LSO" , CreatedOn = now },
                new Country { Id = 125, NameEn = "Liberia", NameAr = "ليبيريا", NationalityEn = "Liberian", NationalityAr = "ليبيري", ISO31661CodeAlph3 = "LBR" , CreatedOn = now },
                new Country { Id = 126, NameEn = "Libya", NameAr = "ليبيا", NationalityEn = "Libyan", NationalityAr = "ليبي", ISO31661CodeAlph3 = "LBY" , CreatedOn = now },
                new Country { Id = 127, NameEn = "Liechtenstein", NameAr = "ليختنشتاين", NationalityEn = "Liechtensteiner", NationalityAr = "ليختنشتايني", ISO31661CodeAlph3 = "LIE" , CreatedOn = now },
                new Country { Id = 128, NameEn = "Lithuania", NameAr = "ليتوانيا", NationalityEn = "Lithuanian", NationalityAr = "ليتواني", ISO31661CodeAlph3 = "LTU" , CreatedOn = now },
                new Country { Id = 129, NameEn = "Luxembourg", NameAr = "لوكسمبورغ", NationalityEn = "Luxembourger", NationalityAr = "لوكسمبورغي", ISO31661CodeAlph3 = "LUX" , CreatedOn = now },
                //new Country { Id = 130, NameEn = "Macao", NameAr = "MO", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MAC" , CreatedOn = now },
                new Country { Id = 131, NameEn = "Madagascar", NameAr = "مدغشقر", NationalityEn = "Madagascan", NationalityAr = "مدغشقري", ISO31661CodeAlph3 = "MDG" , CreatedOn = now },
                new Country { Id = 132, NameEn = "Malawi", NameAr = "ملاوي", NationalityEn = "Malawian", NationalityAr = "ملاوي", ISO31661CodeAlph3 = "MWI" , CreatedOn = now },
                new Country { Id = 133, NameEn = "Malaysia", NameAr = "ماليزيا", NationalityEn = "Malaysian", NationalityAr = "ماليزي", ISO31661CodeAlph3 = "MYS" , CreatedOn = now },
                new Country { Id = 134, NameEn = "Maldives", NameAr = "جزر المالديف", NationalityEn = "Maldivian", NationalityAr = "مالديفي", ISO31661CodeAlph3 = "MDV" , CreatedOn = now },
                new Country { Id = 135, NameEn = "Mali", NameAr = "مالي", NationalityEn = "Malian", NationalityAr = "مالي", ISO31661CodeAlph3 = "MLI" , CreatedOn = now },
                new Country { Id = 136, NameEn = "Malta", NameAr = "مالطا", NationalityEn = "Maltese", NationalityAr = "مالطي", ISO31661CodeAlph3 = "MLT" , CreatedOn = now },
                //new Country { Id = 137, NameEn = "Marshall Islands", NameAr = "MH", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MHL" , CreatedOn = now },
                //new Country { Id = 138, NameEn = "Martinique", NameAr = "MQ", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MTQ" , CreatedOn = now },
                new Country { Id = 139, NameEn = "Mauritania", NameAr = "موريتانيا", NationalityEn = "Mauritanian", NationalityAr = "موريتاني", ISO31661CodeAlph3 = "MRT" , CreatedOn = now },
                new Country { Id = 140, NameEn = "Mauritius", NameAr = "موريشيوس", NationalityEn = "Mauritian", NationalityAr = "موريشيوسي", ISO31661CodeAlph3 = "MUS" , CreatedOn = now },
                new Country { Id = 141, NameEn = "Mayotte", NameAr = "YT", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MYT" , CreatedOn = now },
                new Country { Id = 142, NameEn = "Mexico", NameAr = "المكسيك", NationalityEn = "Mexican", NationalityAr = "مكسيكي", ISO31661CodeAlph3 = "MEX" , CreatedOn = now },
                //new Country { Id = 143, NameEn = "Micronesia (Federated States of)", NameAr = "FM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "FSM" , CreatedOn = now },
                new Country { Id = 144, NameEn = "Moldova (the Republic of)", NameAr = "مولدوفا", NationalityEn = "Moldovan", NationalityAr = "مولدوفي", ISO31661CodeAlph3 = "MDA" , CreatedOn = now },
                //new Country { Id = 145, NameEn = "Monaco", NameAr = "MC", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MCO" , CreatedOn = now },
                new Country { Id = 146, NameEn = "Mongolia", NameAr = "منغوليا", NationalityEn = "Mongolian", NationalityAr = "منغولي", ISO31661CodeAlph3 = "MNG" , CreatedOn = now },
                new Country { Id = 147, NameEn = "Montenegro", NameAr = "الجبل الأسود", NationalityEn = "Montenegrin", NationalityAr = "مونتنيغري", ISO31661CodeAlph3 = "MNE" , CreatedOn = now },
                new Country { Id = 148, NameEn = "Montserrat", NameAr = "MS", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MSR" , CreatedOn = now },
                new Country { Id = 149, NameEn = "Morocco", NameAr = "المغرب", NationalityEn = "Moroccan", NationalityAr = "مغربي", ISO31661CodeAlph3 = "MAR" , CreatedOn = now },
                new Country { Id = 150, NameEn = "Mozambique", NameAr = "موزمبيق", NationalityEn = "Mozambican", NationalityAr = "موزمبيقي", ISO31661CodeAlph3 = "MOZ" , CreatedOn = now },
                //new Country { Id = 151, NameEn = "Myanmar", NameAr = "MM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MMR" , CreatedOn = now },
                new Country { Id = 152, NameEn = "Namibia", NameAr = "نامبيا", NationalityEn = "Namibian", NationalityAr = "ناميبي", ISO31661CodeAlph3 = "NAM" , CreatedOn = now },
                //new Country { Id = 153, NameEn = "Nauru", NameAr = "NR", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "NRU" , CreatedOn = now },
                new Country { Id = 154, NameEn = "Nepal", NameAr = "نيبال", NationalityEn = "Nepalese", NationalityAr = "نيبالي", ISO31661CodeAlph3 = "NPL" , CreatedOn = now },
                new Country { Id = 155, NameEn = "Netherlands", NameAr = "هولندا", NationalityEn = "Dutch", NationalityAr = "هولندي", ISO31661CodeAlph3 = "NLD" , CreatedOn = now },
                //new Country { Id = 156, NameEn = "New Caledonia", NameAr = "NC", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "NCL" , CreatedOn = now },
                new Country { Id = 157, NameEn = "New Zealand", NameAr = "نيوزيلندا", NationalityEn = "New Zealand", NationalityAr = "نيوزيلندي", ISO31661CodeAlph3 = "NZL" , CreatedOn = now },
                new Country { Id = 158, NameEn = "Nicaragua", NameAr = "نيكاراغوا", NationalityEn = "Nicaraguan", NationalityAr = "نيكاراغوي", ISO31661CodeAlph3 = "NIC" , CreatedOn = now },
                new Country { Id = 159, NameEn = "Niger", NameAr = "النيجر", NationalityEn = "Nigerien", NationalityAr = "نيجيري", ISO31661CodeAlph3 = "NER" , CreatedOn = now },
                new Country { Id = 160, NameEn = "Nigeria", NameAr = "نيجيريا", NationalityEn = "Nigerian", NationalityAr = "نيجيري", ISO31661CodeAlph3 = "NGA" , CreatedOn = now },
                //new Country { Id = 161, NameEn = "Niue", NameAr = "NU", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "NIU" , CreatedOn = now },
                //new Country { Id = 162, NameEn = "Norfolk Island", NameAr = "NF", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "NFK" , CreatedOn = now },
                //new Country { Id = 163, NameEn = "North Macedonia", NameAr = "MK", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "NKD" , CreatedOn = now },
                //new Country { Id = 164, NameEn = "Northern Mariana Islands", NameAr = "MP", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MNP" , CreatedOn = now },
                new Country { Id = 165, NameEn = "Norway", NameAr = "النرويج", NationalityEn = "Norwegian", NationalityAr = "نرويجي", ISO31661CodeAlph3 = "NOR" , CreatedOn = now },
                new Country { Id = 166, NameEn = "Oman", NameAr = "عمان", NationalityEn = "Omani", NationalityAr = "عماني", ISO31661CodeAlph3 = "OMN" , CreatedOn = now },
                new Country { Id = 167, NameEn = "Pakistan", NameAr = "باكستان", NationalityEn = "Pakistani", NationalityAr = "باكستاني", ISO31661CodeAlph3 = "PAK" , CreatedOn = now },
                //new Country { Id = 168, NameEn = "Palau", NameAr = "PW", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "PLW" , CreatedOn = now },
                //new Country { Id = 169, NameEn = "Palestine, State of", NameAr = "PS", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "PSE" , CreatedOn = now },
                new Country { Id = 170, NameEn = "Panama", NameAr = "بناما", NationalityEn = "Panamanian", NationalityAr = "بنمي", ISO31661CodeAlph3 = "PAN" , CreatedOn = now },
                //new Country { Id = 171, NameEn = "Papua New Guinea", NameAr = "PG", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "PNG" , CreatedOn = now },
                new Country { Id = 172, NameEn = "Paraguay", NameAr = "باراغواي", NationalityEn = "Paraguayan", NationalityAr = "باراغواي", ISO31661CodeAlph3 = "PRY" , CreatedOn = now },
                new Country { Id = 173, NameEn = "Peru", NameAr = "بيرو", NationalityEn = "Peruvian", NationalityAr = "بيروي", ISO31661CodeAlph3 = "PER" , CreatedOn = now },
                new Country { Id = 174, NameEn = "Philippines", NameAr = "الفلبين", NationalityEn = "Philippine", NationalityAr = "فلبيني", ISO31661CodeAlph3 = "PHL" , CreatedOn = now },
                //new Country { Id = 175, NameEn = "Pitcairn", NameAr = "PN", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "PCN" , CreatedOn = now },
                new Country { Id = 176, NameEn = "Poland", NameAr = "بولندا", NationalityEn = "Polish", NationalityAr = "بولندي", ISO31661CodeAlph3 = "POL" , CreatedOn = now },
                new Country { Id = 177, NameEn = "Portugal", NameAr = "البرتغال", NationalityEn = "Portuguese", NationalityAr = "برتغالي", ISO31661CodeAlph3 = "PRT" , CreatedOn = now },
                new Country { Id = 178, NameEn = "Puerto Rico", NameAr = "PR", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "PRI" , CreatedOn = now },
                new Country { Id = 179, NameEn = "Qatar", NameAr = "قطر", NationalityEn = "Qatari", NationalityAr = "قطري", ISO31661CodeAlph3 = "QAT" , CreatedOn = now },
                new Country { Id = 180, NameEn = "Romania", NameAr = "رومانيا", NationalityEn = "Romanian", NationalityAr = "روماني", ISO31661CodeAlph3 = "ROU" , CreatedOn = now },
                new Country { Id = 181, NameEn = "Russian Federation", NameAr = "روسيا", NationalityEn = "Russian", NationalityAr = "روسي", ISO31661CodeAlph3 = "RUS" , CreatedOn = now },
                new Country { Id = 182, NameEn = "Rwanda", NameAr = "رواندا", NationalityEn = "Rwandan", NationalityAr = "رواندي", ISO31661CodeAlph3 = "RWA" , CreatedOn = now },
                //new Country { Id = 183, NameEn = "Réunion", NameAr = "RE", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "REU" , CreatedOn = now },
                //new Country { Id = 184, NameEn = "Saint Barthélemy", NameAr = "BL", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "BLM" , CreatedOn = now },
                //new Country { Id = 185, NameEn = "Saint Helena, Ascension and Tristan da Cunha", NameAr = "SH", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SHN" , CreatedOn = now },
                //new Country { Id = 186, NameEn = "Saint Kitts and Nevis", NameAr = "KN", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "KNA" , CreatedOn = now },
                //new Country { Id = 187, NameEn = "Saint Lucia", NameAr = "LC", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "LCA" , CreatedOn = now },
                //new Country { Id = 188, NameEn = "Saint Martin (French part)", NameAr = "MF", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "MAF" , CreatedOn = now },
                //new Country { Id = 189, NameEn = "Saint Pierre and Miquelon", NameAr = "PM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SPM" , CreatedOn = now },
                //new Country { Id = 190, NameEn = "Saint Vincent and the Grenadines", NameAr = "VC", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "VCT" , CreatedOn = now },
                //new Country { Id = 191, NameEn = "Samoa", NameAr = "WS", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "WSM" , CreatedOn = now },
                //new Country { Id = 192, NameEn = "San Marino", NameAr = "SM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SMR" , CreatedOn = now },
                //new Country { Id = 193, NameEn = "Sao Tome and Principe", NameAr = "ST", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "STP" , CreatedOn = now },
                new Country { Id = 194, NameEn = "Saudi Arabia", NameAr = "المملكة العربية السعودية", NationalityEn = "Saudi Arabian", NationalityAr = "سعودي", ISO31661CodeAlph3 = "SAU" , CreatedOn = now },
                new Country { Id = 195, NameEn = "Senegal", NameAr = "السنغال", NationalityEn = "Senegalese", NationalityAr = "سنغالي", ISO31661CodeAlph3 = "SEN" , CreatedOn = now },
                new Country { Id = 196, NameEn = "Serbia", NameAr = "صربيا", NationalityEn = "Serbian", NationalityAr = "صربي", ISO31661CodeAlph3 = "SRB" , CreatedOn = now },
                //new Country { Id = 197, NameEn = "Seychelles", NameAr = "SC", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SYC" , CreatedOn = now },
                new Country { Id = 198, NameEn = "Sierra Leone", NameAr = "سيرا ليون", NationalityEn = "Sierra Leonian", NationalityAr = "سيرا ليوني", ISO31661CodeAlph3 = "SLE" , CreatedOn = now },
                new Country { Id = 199, NameEn = "Singapore", NameAr = "سنغافورة", NationalityEn = "Singaporean", NationalityAr = "سنغافوري", ISO31661CodeAlph3 = "SGP" , CreatedOn = now },
                //new Country { Id = 200, NameEn = "Sint Maarten (Dutch part)", NameAr = "SX", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SXM" , CreatedOn = now },
                new Country { Id = 201, NameEn = "Slovakia", NameAr = "سلوفاكيا", NationalityEn = "Slovak", NationalityAr = "سلوفاكي", ISO31661CodeAlph3 = "SVK" , CreatedOn = now },
                new Country { Id = 202, NameEn = "Slovenia", NameAr = "سلوفينيا", NationalityEn = "Slovenian", NationalityAr = "سلوفيني", ISO31661CodeAlph3 = "SVN" , CreatedOn = now },
                //new Country { Id = 203, NameEn = "Solomon Islands", NameAr = "SB", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SLB" , CreatedOn = now },
                new Country { Id = 204, NameEn = "Somalia", NameAr = "الصومال", NationalityEn = "Somali", NationalityAr = "صومالي", ISO31661CodeAlph3 = "SOM" , CreatedOn = now },
                new Country { Id = 205, NameEn = "South Africa", NameAr = "جنوب أفريقيا", NationalityEn = "South African", NationalityAr = "جنوب أفريقي", ISO31661CodeAlph3 = "ZAF" , CreatedOn = now },
                //new Country { Id = 206, NameEn = "South Georgia and the South Sandwich Islands", NameAr = "GS", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SGS" , CreatedOn = now },
                //new Country { Id = 207, NameEn = "South Sudan", NameAr = "SS", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SSD" , CreatedOn = now },
                new Country { Id = 208, NameEn = "Spain", NameAr = "أسبانيا", NationalityEn = "Spanish", NationalityAr = "أسباني", ISO31661CodeAlph3 = "ESP" , CreatedOn = now },
                new Country { Id = 209, NameEn = "Sri Lanka", NameAr = "سيريلانكا", NationalityEn = "Sri Lankan", NationalityAr = "سيريلانكي", ISO31661CodeAlph3 = "LKA" , CreatedOn = now },
                new Country { Id = 210, NameEn = "Sudan", NameAr = "السودان", NationalityEn = "Sudanese", NationalityAr = "سوداني", ISO31661CodeAlph3 = "SDN" , CreatedOn = now },
                //new Country { Id = 211, NameEn = "SuriNameEn", NameAr = "SR", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SUR" , CreatedOn = now },
                //new Country { Id = 212, NameEn = "Svalbard and Jan Mayen", NameAr = "SJ", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "SJM" , CreatedOn = now },
                new Country { Id = 213, NameEn = "Sweden", NameAr = "السويد", NationalityEn = "Swedish", NationalityAr = "سويدي", ISO31661CodeAlph3 = "SWE" , CreatedOn = now },
                new Country { Id = 214, NameEn = "Switzerland", NameAr = "سويسرا", NationalityEn = "Swiss", NationalityAr = "سويسري", ISO31661CodeAlph3 = "CHE" , CreatedOn = now },
                new Country { Id = 215, NameEn = "Syrian Arab Republic", NameAr = "سوريا", NationalityEn = "Syrian", NationalityAr = "سوري", ISO31661CodeAlph3 = "SYR" , CreatedOn = now },
                new Country { Id = 216, NameEn = "Taiwan (Province of China)", NameAr = "تايوان", NationalityEn = "Taiwanese", NationalityAr = "تايواني", ISO31661CodeAlph3 = "TWN" , CreatedOn = now },
                new Country { Id = 217, NameEn = "Tajikistan", NameAr = "طاجيكستان", NationalityEn = "Tajik", NationalityAr = "طاجيكي", ISO31661CodeAlph3 = "TJK" , CreatedOn = now },
                new Country { Id = 218, NameEn = "Tanzania, the United Republic of", NameAr = "تنزانيا", NationalityEn = "Tanzanian", NationalityAr = "تنزاني", ISO31661CodeAlph3 = "TZA" , CreatedOn = now },
                new Country { Id = 219, NameEn = "Thailand", NameAr = "تايلاند", NationalityEn = "Thai", NationalityAr = "تايلاندي", ISO31661CodeAlph3 = "THA" , CreatedOn = now },
                new Country { Id = 220, NameEn = "Timor-Leste", NameAr = "TL", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "TLS" , CreatedOn = now },
                new Country { Id = 221, NameEn = "Togo", NameAr = "توغو", NationalityEn = "Togolese", NationalityAr = "توغو", ISO31661CodeAlph3 = "TGO" , CreatedOn = now },
                //new Country { Id = 222, NameEn = "Tokelau", NameAr = "TK", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "TKL" , CreatedOn = now },
                //new Country { Id = 223, NameEn = "Tonga", NameAr = "TO", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "TON" , CreatedOn = now },
                new Country { Id = 224, NameEn = "Trinidad and Tobago", NameAr = "ترينيداد", NationalityEn = "Tobago", NationalityAr = "توباغو", ISO31661CodeAlph3 = "TON" , CreatedOn = now },
                new Country { Id = 225, NameEn = "Tunisia", NameAr = "تونس", NationalityEn = "Tunisian", NationalityAr = "تونسي", ISO31661CodeAlph3 = "TUN" , CreatedOn = now },
                new Country { Id = 226, NameEn = "Turkey", NameAr = "تركيا", NationalityEn = "Turkish", NationalityAr = "تركي", ISO31661CodeAlph3 = "TUR" , CreatedOn = now },
                new Country { Id = 227, NameEn = "Turkmenistan", NameAr = "تركمانستان", NationalityEn = "Turkmen", NationalityAr = "تركمان", ISO31661CodeAlph3 = "TKM" , CreatedOn = now },
                //new Country { Id = 228, NameEn = "Turks and Caicos Islands", NameAr = "TC", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "TCA" , CreatedOn = now },
                new Country { Id = 229, NameEn = "Tuvalu", NameAr = "TV", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "TUV" , CreatedOn = now },
                new Country { Id = 230, NameEn = "Uganda", NameAr = "أوغندا", NationalityEn = "Ugandan", NationalityAr = "أوغندي", ISO31661CodeAlph3 = "UGA" , CreatedOn = now },
                new Country { Id = 231, NameEn = "Ukraine", NameAr = "أوكرانيا", NationalityEn = "Ukrainian", NationalityAr = "أوكراني", ISO31661CodeAlph3 = "UKR" , CreatedOn = now },
                new Country { Id = 232, NameEn = "United Arab Emirates", NameAr = "الإمارات العربية المتحدة", NationalityEn = "Emirates", NationalityAr = "إماراتي", ISO31661CodeAlph3 = "ARE" , CreatedOn = now },
                new Country { Id = 233, NameEn = "United Kingdom of Great Britain and Northern Ireland", NameAr = "المملكة المتحدة", NationalityEn = "British", NationalityAr = "إنجليزي", ISO31661CodeAlph3 = "GBR" , CreatedOn = now },
                //new Country { Id = 234, NameEn = "United States Minor Outlying Islands", NameAr = "UM", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "UMI" , CreatedOn = now },
                new Country { Id = 235, NameEn = "United States of America", NameAr = "الولايات المتحدة الأمريكية", NationalityEn = "American", NationalityAr = "أمريكي", ISO31661CodeAlph3 = "USA" , CreatedOn = now },
                new Country { Id = 236, NameEn = "Uruguay", NameAr = "أوروغواي", NationalityEn = "Uruguayan", NationalityAr = "أوروغواي", ISO31661CodeAlph3 = "URY" , CreatedOn = now },
                new Country { Id = 237, NameEn = "Uzbekistan", NameAr = "أوزبكستان", NationalityEn = "Uzbek", NationalityAr = "أوزبكي", ISO31661CodeAlph3 = "UZB" , CreatedOn = now },
                new Country { Id = 238, NameEn = "Vanuatu", NameAr = "فانواتو", NationalityEn = "Vanuatuan", NationalityAr = "فانواتو", ISO31661CodeAlph3 = "VUT" , CreatedOn = now },
                new Country { Id = 239, NameEn = "Venezuela (Bolivarian Republic of)", NameAr = "فنزويلا", NationalityEn = "Venezuelan", NationalityAr = "فنزويلي", ISO31661CodeAlph3 = "VEN" , CreatedOn = now },
                new Country { Id = 240, NameEn = "Viet Nam", NameAr = "فيتنام", NationalityEn = "Vietnamese", NationalityAr = "فيتنامي", ISO31661CodeAlph3 = "VNM" , CreatedOn = now },
                //new Country { Id = 241, NameEn = "Virgin Islands (British)", NameAr = "VG", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "VGB" , CreatedOn = now },
                //new Country { Id = 242, NameEn = "Virgin Islands (U.S.)", NameAr = "VI", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "VIR" , CreatedOn = now },
                //new Country { Id = 243, NameEn = "Wallis and Futuna", NameAr = "WF", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "VLF" , CreatedOn = now },
                //new Country { Id = 244, NameEn = "Western Sahara", NameAr = "EH", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "ESH" , CreatedOn = now },
                new Country { Id = 245, NameEn = "Yemen", NameAr = "اليمن", NationalityEn = "Yemeni", NationalityAr = "يمني", ISO31661CodeAlph3 = "YEM" , CreatedOn = now },
                new Country { Id = 246, NameEn = "Zambia", NameAr = "زامبيا", NationalityEn = "Zambian", NationalityAr = "زامبي", ISO31661CodeAlph3 = "ZMB" , CreatedOn = now },
                new Country { Id = 247, NameEn = "Zimbabwe", NameAr = "زيمبابوي", NationalityEn = "Zimbabwean", NationalityAr = "زيمبابوي", ISO31661CodeAlph3 = "ZWE" , CreatedOn = now },
                //new Country { Id = 248, NameEn = "Aland Islands", NameAr = "AX", NationalityEn = "", NationalityAr = "", ISO31661CodeAlph3 = "ALA" , CreatedOn = now }
            };
        }

        public static AttachmentType[] CreateAttachmentTypes()
        {
            return new[]
            {
                new AttachmentType { Id = 1, Name = "ضبط الجلسة", GroupName = GroupNames.Hearing,  CreatedOn = now },
                new AttachmentType { Id = 2, Name = "استشارات وصياغة عقود", GroupName = GroupNames.Case, CreatedOn = now },
                new AttachmentType { Id = 3, Name = "ملف محضر الاجتماع", GroupName = GroupNames.InvestigationRecord,  CreatedOn = now },
                new AttachmentType { Id = 4, Name = "ملف  مرفق صوت", GroupName = GroupNames.InvestigationRecord, CreatedOn = now },
                new AttachmentType { Id = 5, Name = "فيديو للاجتماع", GroupName = GroupNames.InvestigationRecord, CreatedOn = now },
                new AttachmentType { Id = 6, Name = "صك الحكم", GroupName = GroupNames.CaseRule, CreatedOn = now },
                new AttachmentType { Id = 7, Name = "خطاب التمثيل", GroupName = GroupNames.RepresentativeLetterImage, CreatedOn = now },
                new AttachmentType { Id = 8, Name = "تحديث جلسة", GroupName = GroupNames.HearingUpdate, CreatedOn = now },
                new AttachmentType { Id = 9, Name = "أخرى", GroupName = GroupNames.Hearing, CreatedOn = now },
                new AttachmentType { Id = 10, Name = "معاملة", GroupName = GroupNames.Moamala, CreatedOn = now },
            };
        }

        public static FieldMissionType[] CreateFieldMissionType()
        {
            return new[]
            {
                new FieldMissionType { Id = 1, Name = "مهمه 1", CreatedOn = now },
                new FieldMissionType { Id = 2, Name = "مهمه 2", CreatedOn = now },
                new FieldMissionType { Id = 3, Name = "مهمه 3", CreatedOn = now },
            };
        }

        public static Branch[] CreateBranches()
        {
            return new[]
            {
                new Branch { Id = 1, Name = "وزارة التعليم",ParentId = null, CreatedOn = now,
                BranchDepartments = new List<BranchesDepartments>{
                        new BranchesDepartments { BranchId = 1, DepartmentId = 1, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 1, DepartmentId = 2, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 1, DepartmentId = 3, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 1, DepartmentId = 4, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 1, DepartmentId = 5, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 1, DepartmentId = 6, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 1, DepartmentId = 7, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                    }},
                new Branch { Id = 2, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم منطقة الرياض",ParentId= 1,CreatedOn = now,
                    BranchDepartments = new List<BranchesDepartments>{
                        new BranchesDepartments { BranchId = 2, DepartmentId = 1, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 2, DepartmentId = 2, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 2, DepartmentId = 3, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 2, DepartmentId = 4, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 2, DepartmentId = 5, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 2, DepartmentId = 6, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 2, DepartmentId = 7, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                    }
                },
                new Branch { Id = 3, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة الخرج", ParentId= 1, CreatedOn = now,
                BranchDepartments = new List<BranchesDepartments>{
                        new BranchesDepartments { BranchId = 3, DepartmentId = 1, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 3, DepartmentId = 4, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 3, DepartmentId = 6, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                        new BranchesDepartments { BranchId = 3, DepartmentId = 7, CreatedOn = now, CreatedBy = ApplicationConstants.SystemAdministratorId },
                    }},
                new Branch { Id = 4, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة الدوامى",ParentId= 1,CreatedOn = now },
                new Branch { Id = 5, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة المجمعة",ParentId= 1,CreatedOn = now },
                new Branch { Id = 6, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة القويعية",ParentId= 1,CreatedOn = now },
                new Branch { Id = 7, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة وادى الدواسر",ParentId= 1,CreatedOn = now },
                new Branch { Id = 8, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة وادى الأفلاج",ParentId= 1,CreatedOn = now },
                new Branch { Id = 9, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة وادى الزلفى",ParentId= 1,CreatedOn = now },
                new Branch { Id = 10, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة وادى شقراء",ParentId= 1,CreatedOn = now },
                new Branch { Id = 11, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة حوطة بنى تميم والحريق",ParentId= 1,CreatedOn = now },
                new Branch { Id = 12, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة عفيف",ParentId= 1,CreatedOn = now },
                new Branch { Id = 13, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة الغاط",ParentId= 1,CreatedOn = now },
                new Branch { Id = 14, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم منطقة مكة المكرمة",ParentId= 1,CreatedOn = now },
                new Branch { Id = 15, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة جدة",ParentId= 1, CreatedOn = now },
                new Branch { Id = 16, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة الطائف",ParentId= 1, CreatedOn = now },
                new Branch { Id = 17, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة القنفذة",ParentId= 1, CreatedOn = now },
                new Branch { Id = 18, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة الليث",ParentId= 1, CreatedOn = now },
                new Branch { Id = 19, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم منطقة المدينة المنورة",ParentId= 1,CreatedOn = now },
                new Branch { Id = 20, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة ينبع",ParentId= 1, CreatedOn = now },
                new Branch { Id = 21, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة العلا",ParentId= 1, CreatedOn = now },
                new Branch { Id = 22, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة المهد",ParentId= 1, CreatedOn = now },
                new Branch { Id = 23, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم منطقة القصيم",ParentId= 1,CreatedOn = now },
                new Branch { Id = 24, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة عنيزة",ParentId= 1, CreatedOn = now },
                new Branch { Id = 25, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة الرس",ParentId= 1, CreatedOn = now },
                new Branch { Id = 26, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة المذنب",ParentId= 1, CreatedOn = now },
                new Branch { Id = 27, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة البكيرية",ParentId= 1, CreatedOn = now },
                new Branch { Id = 28, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم منطقة الشرقية",ParentId= 1,CreatedOn = now },
                new Branch { Id = 29, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة الاحساء",ParentId= 1, CreatedOn = now },
                new Branch { Id = 30, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة حفر الباطن",ParentId= 1, CreatedOn = now },
                new Branch { Id = 31, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم بمنطقة عسير",ParentId= 1,CreatedOn = now },
                new Branch { Id = 32, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة بيشة",ParentId= 1, CreatedOn = now },
                new Branch { Id = 33, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة النماص",ParentId= 1, CreatedOn = now },
                new Branch { Id = 34, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة محايل عسير",ParentId= 1, CreatedOn = now },
                new Branch { Id = 35, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة سراة عبيدة",ParentId= 1, CreatedOn = now },
                new Branch { Id = 36, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة ظهران الجنوب ",ParentId= 1, CreatedOn = now },
                new Branch { Id = 37, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة رجال ألمع ",ParentId= 1, CreatedOn = now },
                new Branch { Id = 38, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم بمنطقة حائل",ParentId= 1,CreatedOn = now },
                new Branch { Id = 39, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم بمنطقة تبوك",ParentId= 1,CreatedOn = now },
                new Branch { Id = 40, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم بمنطقة الباحة",ParentId= 1,CreatedOn = now },
                new Branch { Id = 41, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة المخواة ",ParentId= 1, CreatedOn = now },
                new Branch { Id = 42, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم بمنطقة الحدود الشمالية",ParentId= 1,CreatedOn = now },
                new Branch { Id = 43, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم بمنطقة الحدود الجوف",ParentId= 1,CreatedOn = now },
                new Branch { Id = 44, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة القريات",ParentId= 1, CreatedOn = now },
                new Branch { Id = 45, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم بمنطقة جازن",ParentId= 1,CreatedOn = now },
                new Branch { Id = 46, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة صبيا",ParentId= 1, CreatedOn = now },
                new Branch { Id = 47, Name = "إدارة الشؤون القانونية بالإدارة العامة بتعليم بمنطقة نجران",ParentId= 1,CreatedOn = now },
                new Branch { Id = 48, Name = "إدارة الشؤون القانونية بإدارة التعليم بمحافظة شروة",ParentId= 1, CreatedOn = now },
            };
        }

        public static MinistryDepartment[] CreateMinistryDepartments()
        {
            return new[]
            {
                new MinistryDepartment { Id=1, Name = "إدارة المشتريات", CreatedOn = now , MinistrySectorId = 1 },
                new MinistryDepartment { Id=2, Name = "إدارة العمال", CreatedOn = now , MinistrySectorId = 2 },
                new MinistryDepartment { Id=3, Name = "إدارة المخازن", CreatedOn = now , MinistrySectorId = 2},
                new MinistryDepartment { Id=4, Name = "إدارة التسويق", CreatedOn = now , MinistrySectorId = 3},
            };
        }

        public static MinistrySector[] CreateMinistrySectors()
        {
            return new[]
            {
                new MinistrySector { Id= 1, Name = "قطاع 1", CreatedOn = now },
                new MinistrySector { Id= 2, Name = "قطاع 2", CreatedOn = now },
                new MinistrySector { Id= 3, Name = "قطاع 3", CreatedOn = now },
            };
        }

        public static InvestigationRecordPartyType[] CreateInvestigationRecordPartyType()
        {
            return new[]
            {
                new InvestigationRecordPartyType { Id=1, Name = "محقق معه", CreatedOn = now },
                new InvestigationRecordPartyType { Id=2, Name = "شاهد", CreatedOn = now },
                new InvestigationRecordPartyType { Id=3, Name = "خبير", CreatedOn = now },
            };
        }

        public static Department[] CreateDepartments()
        {
            return new[]
            {
                new Department
                {
                    Id = 1,
                    Name = "إدارة الترافع",
                    Order = 1,
                    WorkItemTypes = new List<WorkItemType>
                    {
                         new WorkItemType
                         {
                             Id = 11,
                             Name = "قضية",
                             RolesIds = ApplicationRolesConstants.LegalResearcher.Code.ToString(),
                             CreatedOn = now,
                             SubWorkItemTypes = new List<SubWorkItemType>
                             {
                                 new SubWorkItemType
                                 {
                                     Id = 1,
                                     Name = "قضية فرع 1",
                                     CreatedOn = now
                                 },
                                 new SubWorkItemType
                                 {
                                     Id = 2,
                                     Name = "قضية فرع 2",
                                     CreatedOn = now
                                 }
                             }
                         }
                    },
                    CreatedOn = now

                },
                new Department
                {
                    Id= 2,
                    Name = "إدارة التحقيقات",
                    Order = 2,
                    WorkItemTypes = new List<WorkItemType>
                    {
                        new WorkItemType {
                            Id = 12,
                            Name = "تحقيق",
                            RolesIds = ApplicationRolesConstants.Investigator.Code.ToString(),
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 3,
                                    Name = "تحقيق فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 4,
                                    Name = "تحقيق فرع 2",
                                    CreatedOn = now
                                }
                            }
                        }
                    },
                    CreatedOn = now
                },
                new Department
                {
                    Id = 3,
                    Name = "إدارة الاستشارات والدراسات القانونية",
                    WorkItemTypes = new List<WorkItemType>
                    {
                        new WorkItemType
                        {
                            Id = 1,
                            Name = "استشارة",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 5,
                                    Name = "استشارة فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 6,
                                    Name = "استشارة فرع 2",
                                    CreatedOn = now
                                }
                            }
                        },
                        new WorkItemType
                        {
                            Id = 2,
                            Name = "دراسة قانونية",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                 new SubWorkItemType
                                 {
                                     Id = 7,
                                     Name = "دراسة قانونية فرع 1",
                                     CreatedOn = now
                                 },
                                 new SubWorkItemType
                                 {
                                     Id = 8,
                                     Name = "دراسة قانونية فرع 2",
                                     CreatedOn = now
                                 }
                            }
                        }
                    },
                    CreatedOn = now
                },
                new Department
                {
                    Id = 4,
                    Name = "إدارة التظلمات والتسويات لقانونية",
                    WorkItemTypes = new List<WorkItemType>
                    {
                        new WorkItemType
                        {
                            Id = 3,
                            Name = "تظلم",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 9,
                                    Name = "تظلم فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 10,
                                    Name = "تظلم فرع 2",
                                    CreatedOn = now
                                }
                            }
                        },
                        new WorkItemType
                        {
                            Id = 4,
                            Name = "تسوية قانونية",
                            RolesIds = ApplicationRolesConstants.LegalConsultant.Code.ToString() +","+ ApplicationRolesConstants.LegalResearcher.Code.ToString(),
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 11,
                                    Name = "تسوية قانونية فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 12,
                                    Name = "تسوية قانونية فرع 2",
                                    CreatedOn = now
                                }
                            }
                        }
                    },
                    CreatedOn = now
                },
                new Department
                {
                    Id = 5,
                    Name = "إدارة العقود والاتفاقيات القانونية",
                    WorkItemTypes = new List<WorkItemType>
                    {
                        new WorkItemType
                        {
                            Id = 5,
                            Name = "عقد",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 13,
                                    Name = "عقد فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 14,
                                    Name = "عقد فرع 2",
                                    CreatedOn = now
                                }
                            }
                        },
                        new WorkItemType
                        {
                            Id = 6,
                            Name = "اتفاقية قانونية",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 15,
                                    Name = "اتفاقية قانونية فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 16,
                                    Name = "اتفاقية قانونية فرع 2",
                                    CreatedOn = now
                                }
                            }
                        }
                    },
                    CreatedOn = now
                },
                new Department
                {
                    Id = 6,
                    Name = "إدارة حقوق الانسان",
                    WorkItemTypes = new List<WorkItemType>
                    {
                        new WorkItemType
                        {
                            Id = 7,
                            Name = "معاملة حقوق انسان",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 17,
                                    Name = "معاملة حقوق انسان فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 18,
                                    Name = "معاملة حقوق انسان فرع 2",
                                    CreatedOn = now
                                }
                            }
                        }
                    },
                    CreatedOn = now
                },
                new Department
                {
                    Id = 7,
                    Name = "إدارة الأنظمة واللوائح والقرارات",
                    WorkItemTypes = new List<WorkItemType>
                    {
                        new WorkItemType
                        {
                            Id = 8,
                            Name = "نظام",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 19,
                                    Name = "نظام فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 20,
                                    Name = "نظام فرع 2",
                                    CreatedOn = now
                                }
                            }
                        },
                        new WorkItemType
                        {
                            Id = 9,
                            Name = "لائحة",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 21,
                                    Name = "لائحة فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 22,
                                    Name = "لائحة فرع 2",
                                    CreatedOn = now
                                }
                            }
                        },
                        new WorkItemType
                        {
                            Id = 10,
                            Name = "قرار",
                            RolesIds = $"{ApplicationRolesConstants.LegalConsultant.Code},{ApplicationRolesConstants.LegalResearcher.Code}",
                            CreatedOn = now,
                            SubWorkItemTypes = new List<SubWorkItemType>
                            {
                                new SubWorkItemType
                                {
                                    Id = 23,
                                    Name = "قرار فرع 1",
                                    CreatedOn = now
                                },
                                new SubWorkItemType
                                {
                                    Id = 24,
                                    Name = "قرار فرع 2",
                                    CreatedOn = now
                                }
                            }
                        }
                    },
                    CreatedOn = now
                }
            };
        }

        private static GovernmentOrganization[] CreateGovernmentOrganizations()
        {
            return new[]
            {
                new GovernmentOrganization{ Id = 1, Name = "وزارة الصحة", CreatedOn = now }
            };
        }

        public static SecondSubCategory[] CreateSecondSubCategories()
        {
            return new[] {
                new SecondSubCategory
                {
                    Id=1,
                    Name="ناجز فرعي 2",
                    CreatedOn = now,
                    IsActive = true,
                    FirstSubCategory = new FirstSubCategory
                    {
                        Id = 1,
                        Name = "ناجز فرعي 1",
                        CreatedOn = now,
                        MainCategory = new MainCategory
                        {
                            Id = 1,
                            CreatedOn = now,
                            Name = "ناجز رئيسي",
                            CaseSource = CaseSources.Najiz
                        }
                    }
                }
            };
        }
    }
}
