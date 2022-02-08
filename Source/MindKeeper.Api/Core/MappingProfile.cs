using AutoMapper;

namespace MindKeeper.Api.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Shared.Models.ApiModels.Ideas.IdeaCreateRequest,
                Domain.Interfaces.Ideas.IdeaCreateModel>();

            CreateMap<MindKeeper.Domain.Entities.Idea,
                Shared.Models.ApiModels.Ideas.IdeaGetResult>();

            CreateMap<MindKeeper.Domain.Entities.Idea,
                Shared.Models.ApiModels.Ideas.IdeasGetAllResult.Idea>();

            CreateMap<Shared.Models.ApiModels.Ideas.IdeasGetAllRequest,
                MindKeeper.Domain.Interfaces.Ideas.IdeaGetAllModel>();

            CreateMap<Shared.Models.ApiModels.Ideas.IdeaLinkAddRequest,
                MindKeeper.Domain.Interfaces.Ideas.IdeaLinkAddModel>();

            CreateMap<Shared.Models.ApiModels.Ideas.IdeaLinkDeleteRequest,
                MindKeeper.Domain.Interfaces.Ideas.IdeaLinkDeleteModel>();

            CreateMap<MindKeeper.Domain.Entities.Achievement,
                Shared.Models.ApiModels.Statistics.AchievementsResult.Achivement>();

            CreateMap<MindKeeper.Domain.EntitiesComposed.StatsSystem,
                Shared.Models.ApiModels.Statistics.StatsSystemResult>();

            CreateMap<MindKeeper.Domain.EntitiesComposed.StatsUser,
                Shared.Models.ApiModels.Statistics.StatsUserResult>();

            CreateMap<MindKeeper.Domain.Entities.Country,
                Shared.Models.ApiModels.Countries.CountryGetAllResult.Country>();

            CreateMap<MindKeeper.Domain.Entities.DomainEntity,
                Shared.Models.ApiModels.Domains.DomainGetAllResult.Domain>();

            CreateMap<MindKeeper.Domain.Entities.Subdomain,
                Shared.Models.ApiModels.Domains.DomainGetAllResult.Subdomain>();
        }
    }
}
