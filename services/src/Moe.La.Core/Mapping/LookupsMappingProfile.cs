using AutoMapper;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Linq;

namespace Moe.La.Core.Mapping
{
    class LookupsMappingProfile : Profile
    {
        public LookupsMappingProfile()
        {
            #region Domain Models to API Dto:

            #region MinistryDepartment
            CreateMap<MinistryDepartment, MinistryDepartmentListItemDto>()
               .ForMember(res => res.MinistrySector, opt => opt.MapFrom(p => p.MinistrySector.Name));


            #endregion
            #region MinistrySector
            CreateMap<MinistrySector, MinistrySectorListItemDto>();


            #endregion
            #region InvestigationRecordPartyType
            CreateMap<InvestigationRecordPartyType, InvestigationRecordPartyTypeListItemDto>();

            #endregion

            #region City
            CreateMap<City, CityListItemDto>()
                .ForMember(res => res.Province, opt => opt.MapFrom(p => p.Province.Name));
            //.ForMember(res => res.UpdateUserFullName, opt => opt.MapFrom(c => c.UpdateUser.FirstName + " " + c.UpdateUser.LastName));

            #endregion

            #region Countries
            CreateMap<Country, CountryDto>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region Court
            CreateMap<Court, CourtListItemDto>()
                .ForMember(res => res.LitigationType, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.LitigationType)))
                .ForMember(res => res.CourtCategory, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.CourtCategory)));
            //.ForMember(res => res.UpdateUserFullName, opt => opt.MapFrom(c => c.UpdateUser.FirstName + " " + c.UpdateUser.LastName));

            #endregion

            #region FieldMissionType
            CreateMap<FieldMissionType, FieldMissionTypeListItemDto>();
            //.ForMember(res => res.UpdateUserFullName, opt => opt.MapFrom(c => c.UpdateUser.FirstName + " " + c.UpdateUser.LastName));

            #endregion

            #region GeneralManagement
            CreateMap<BranchDto, Branch>()
                .ForMember(s => s.Id, opt => opt.Ignore()).ReverseMap();

            CreateMap<Branch, BranchListItemDto>()
                .ForMember(res => res.Parent, opt => opt.MapFrom(s => s.Parent.Name))
                .ForMember(res => res.Departments, opt => opt.MapFrom(c => c.BranchDepartments.Select(cc => cc.DepartmentId).ToList()));

            CreateMap<Branch, BranchDto>()
                .ForMember(res => res.Departments, opt => opt.MapFrom(c => c.BranchDepartments.Select(cc => cc.DepartmentId).ToList()));
            #endregion

            #region IdentityType
            CreateMap<IdentityType, IdentityTypeListItemDto>();
            //.ForMember(res => res.UpdateUserFullName, opt => opt.MapFrom(c => c.UpdateUser.FirstName + " " + c.UpdateUser.LastName));

            #endregion

            #region JobTitle
            CreateMap<JobTitle, JobTitleListItemDto>();
            //.ForMember(res => res.UpdateUserFullName, opt => opt.MapFrom(c => c.UpdateUser.FirstName + " " + c.UpdateUser.LastName));

            #endregion


            #region PartyType
            CreateMap<PartyType, PartyTypeListItemDto>();
            //.ForMember(res => res.UpdateUserFullName, opt => opt.MapFrom(c => c.UpdateUser.FirstName + " " + c.UpdateUser.LastName));

            #endregion


            #region Province
            CreateMap<Province, ProvinceListItemDto>()
                .ForMember(res => res.Cities, opt => opt.MapFrom(m => m.Cities.Select(s => s.Name)));
            //.ForMember(res => res.UpdateUserFullName, opt => opt.MapFrom(c => c.UpdateUser.FirstName + " " + c.UpdateUser.LastName));
            #endregion


            #region WorkItemType
            CreateMap<WorkItemType, WorkItemTypeListItemDto>()
                .ForMember(res => res.Department, t => t.MapFrom(t => new KeyValuePairsDto<int>() { Id = t.Department.Id, Name = t.Department.Name }));
            CreateMap<WorkItemTypeDto, WorkflowType>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region SubWorkItemType
            CreateMap<SubWorkItemType, SubWorkItemTypeListItemDto>()
                .ForMember(res => res.WorkItemType, t => t.MapFrom(t => new KeyValuePairsDto<int>() { Id = t.WorkItemType.Id, Name = t.WorkItemType.Name }));
            #endregion

            #region District
            CreateMap<District, DistrictListItemDto>()
                  .ForMember(res => res.City, opt => opt.MapFrom(r => r.City.Name));

            #endregion

            #region GovernmentOrganization
            CreateMap<GovernmentOrganization, GovernmentOrganizationListItemDto>();
            CreateMap<GovernmentOrganization, GovernmentOrganizationDto>().ReverseMap();
            #endregion

            #region Department & Branch
            CreateMap<BranchesDepartments, DepartmentBranchDto>();
            CreateMap<DepartmentBranchDto, BranchesDepartments>();
            // .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region API Dto to Domain:

            #region MinistryDepartment
            CreateMap<MinistryDepartmentDto, MinistryDepartment>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            #endregion
            #region MinistrySector
            CreateMap<MinistrySectorDto, MinistrySector>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            #endregion

            #region InvestigationRecordPartyType
            CreateMap<InvestigationRecordPartyTypeDto, InvestigationRecordPartyType>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            #endregion

            #region City
            CreateMap<CityDto, City>()
                .ForMember(s => s.Id, opt => opt.Ignore())
                .ReverseMap();

            //CreateMap<CityQueryObjectDto, CityQueryObject>();
            #endregion

            #region Court
            CreateMap<CourtDto, Court>()
               .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region FieldMissionType
            CreateMap<FieldMissionTypeDto, FieldMissionType>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion



            #region IdentityType
            CreateMap<IdentityTypeDto, IdentityType>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region JobTitle
            CreateMap<JobTitleDto, JobTitle>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region Province
            CreateMap<ProvinceDto, Province>();
            //CreateMap<ProvinceQueryObjectDto, ProvinceQueryObject>();
            #endregion

            #region PartyType
            CreateMap<PartyTypeDto, PartyType>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion


            #region AttachmentType
            CreateMap<AttachmentTypeDto, AttachmentType>()
              .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region InvestigationQuestion

            CreateMap<InvestigationQuestionDto, InvestigationQuestion>()
              .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<InvestigationQuestion, InvestigationQuestionDto>();

            CreateMap<InvestigationQuestion, InvestigationQuestionListItemDto>()
                .ForMember(res => res.Status, opt => opt.MapFrom(r => new KeyValuePairsDto<int> { Id = (int)r.Status, Name = EnumExtensions.GetDescription(r.Status) }));

            #endregion

            #region WorkItemType
            CreateMap<WorkItemTypeDto, WorkItemType>()
                .ForMember(res => res.Id, opt => opt.Ignore());
            #endregion

            #region SubWorkItemType
            CreateMap<SubWorkItemTypeDto, SubWorkItemType>()
                .ForMember(res => res.Id, opt => opt.Ignore());
            #endregion

            #region District
            CreateMap<DistrictDto, District>()
                .ForMember(s => s.Id, opt => opt.Ignore()).ReverseMap();
            #endregion

            #region GovernmentOrganization
            CreateMap<GovernmentOrganizationDto, GovernmentOrganization>()
                 .ForMember(s => s.Id, opt => opt.Ignore()).ReverseMap();

            #endregion

            #region Department
            CreateMap<DepartmentDto, Department>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<Department, DepartmentDto>();
            #endregion

            #endregion 
            #endregion 

            #region Main Category
            CreateMap<MainCategoryDto, MainCategory>()
                    .ForMember(s => s.Id, opt => opt.Ignore());

            CreateMap<MainCategory, MainCategoryListItemDto>();
            #endregion

            #region First Sub Category
            CreateMap<FirstSubCategoryDto, FirstSubCategory>()
              .ForMember(s => s.Id, opt => opt.Ignore());

            CreateMap<FirstSubCategory, FirstSubCategoryListItemDto>();
            #endregion

            #region Second Sub Caregory
            CreateMap<SecondSubCategoryDto, SecondSubCategory>()
                .ForMember(s => s.FirstSubCategory, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore());


            CreateMap<SecondSubCategory, SecondSubCategoryDto>()
                 .ForMember(s => s.CaseSource, opt => opt.MapFrom(opt => opt.FirstSubCategory.MainCategory.CaseSource))
                 .ForMember(s => s.MainCategory, opt => opt.MapFrom(opt => new MainCategoryDto { Id = opt.FirstSubCategory.MainCategory.Id, Name = opt.FirstSubCategory.MainCategory.Name }))
                .ForMember(s => s.FirstSubCategory, opt => opt.MapFrom(opt => new FirstSubCategoryDto { Id = opt.FirstSubCategory.Id, Name = opt.FirstSubCategory.Name, MainCategoryId = opt.FirstSubCategory.MainCategoryId }));

            CreateMap<SecondSubCategory, SecondSubCategoryListItemDto>()
                  .ForMember(res => res.FirstSubCategory, t => t.MapFrom(t => new KeyValuePairsDto<int>() { Id = t.FirstSubCategory.Id, Name = t.FirstSubCategory.Name }))
                  .ForMember(res => res.CaseSource, t => t.MapFrom(t => new KeyValuePairsDto<int>() { Id = (int)t.FirstSubCategory.MainCategory.CaseSource, Name = EnumExtensions.GetDescription(t.FirstSubCategory.MainCategory.CaseSource) }))
                  .ForMember(res => res.MainCategory, t => t.MapFrom(t => new KeyValuePairsDto<int>() { Id = t.FirstSubCategory.MainCategory.Id, Name = t.FirstSubCategory.MainCategory.Name }));
            #endregion
        }
    }
}
