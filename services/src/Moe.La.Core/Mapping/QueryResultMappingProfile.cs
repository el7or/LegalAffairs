using AutoMapper;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class QueryResultMappingProfile : Profile
    {
        public QueryResultMappingProfile()
        {

            #region API Dto to Domain:  
            CreateMap(typeof(QueryResultDto<>), typeof(QueryResult<>));

            //CreateMap<QueryObjectDto, QueryObject>();
            #endregion



        }
    }
}
