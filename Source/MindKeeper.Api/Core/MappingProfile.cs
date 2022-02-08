using AutoMapper;

namespace MindKeeper.Api.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MindKeeper.Domain.Entities.Idea,
                Shared.Models.ApiModels.Ideas.IdeaGetResult>();

            CreateMap<MindKeeper.Domain.Entities.Idea,
                Shared.Models.ApiModels.Ideas.IdeasGetAllResult.Idea>();

            CreateMap<Shared.Models.ApiModels.Ideas.IdeasGetAllRequest,
                MindKeeper.Domain.Interfaces.Ideas.IdeaGetAllModel>();

            CreateMap<MindKeeper.Domain.Entities.Achievement,
                Shared.Models.ApiModels.Statistics.AchievementsResult.Achivement>();

            CreateMap<MindKeeper.Domain.EntitiesComposed.StatsSystem,
                Shared.Models.ApiModels.Statistics.StatsSystemResult>();

            CreateMap<MindKeeper.Domain.EntitiesComposed.StatsUser,
                Shared.Models.ApiModels.Statistics.StatsUserResult>();
        }
    }
}
