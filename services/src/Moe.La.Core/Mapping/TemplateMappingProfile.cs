using AutoMapper;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class TemplateMappingProfile : Profile
    {
        public TemplateMappingProfile()
        {
            CreateMap<LetterTemplate, LetterTemplateDto>()
                .ReverseMap();
        }
    }
}
