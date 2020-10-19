using System;
using AutoMapper;
using Board.Core.Entities;

namespace Board.Application.Jobs.GetJobs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Job, GetJobsQueryResult.Job>()
                .ForMember(x => x.CompanyId, o => o.MapFrom(src => src.Company.Id))
                .ForMember(x => x.CompanyName, o => o.MapFrom(src => src.Company.Name))
                .ForMember(x => x.HasLogo, o => o.MapFrom(src => src.Company.Logo != null))
                .ForMember(x => x.Age, o => o.MapFrom(src => (DateTime.UtcNow - src.PublishedOn.Value).TotalDays))
                .ForMember(x => x.IsFeatured, o => o.MapFrom(src => src.PublicationType == PublicationType.Featured));

        }
    }
}
