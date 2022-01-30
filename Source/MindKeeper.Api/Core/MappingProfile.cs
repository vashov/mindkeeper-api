using AutoMapper;

namespace MindKeeper.Api.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MindKeeper.Domain.Entities.Node,
                Shared.Models.ApiModels.Nodes.NodeGetResult>();

            CreateMap<MindKeeper.Domain.Entities.Node,
                Shared.Models.ApiModels.Nodes.NodesGetAllResult.NodeResponse>();

            CreateMap<Shared.Models.ApiModels.Nodes.NodesGetAllRequest,
                MindKeeper.Domain.Filters.NodeFilter>();
        }
    }
}
