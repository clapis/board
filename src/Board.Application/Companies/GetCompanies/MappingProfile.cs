using AutoMapper;
using Board.Core.Entities;

namespace Board.Application.Companies.GetCompanies
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, GetCompaniesQueryResult.Company>()
                .ForMember(x => x.HasLogo, o => o.MapFrom(src => src.Logo != null));
        }
    }
}
