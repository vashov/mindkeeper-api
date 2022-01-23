using AutoMapper;

namespace MindKeeper.Api.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MindKeeper.Api.Data.Entities.Node,
                Shared.Models.ApiModels.Nodes.GetNodeResponse>();

            CreateMap<MindKeeper.Api.Data.Entities.Node,
                Shared.Models.ApiModels.Nodes.GetAllNodesResponse.NodeResponse>();

            CreateMap<Shared.Models.ApiModels.Nodes.GetAllNodesRequest,
                Data.Repositories.Nodes.Models.NodeFilter>();
        }
    }
}
