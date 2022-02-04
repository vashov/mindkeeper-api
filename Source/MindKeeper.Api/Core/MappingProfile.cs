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
                Shared.Models.ApiModels.Ideas.IdeasGetAllResult.IdeaResponse>();

            CreateMap<Shared.Models.ApiModels.Ideas.IdeasGetAllRequest,
                MindKeeper.Domain.Interfaces.Ideas.IdeaGetAllModel>();
        }
    }
}
