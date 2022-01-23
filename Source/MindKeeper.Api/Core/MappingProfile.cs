using AutoMapper;

namespace MindKeeper.Api.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MindKeeper.Api.Data.Entities.Node,
                Shared.Models.ApiModels.Nodes.NodeGetResponse>();

            CreateMap<MindKeeper.Api.Data.Entities.Node,
                Shared.Models.ApiModels.Nodes.NodesGetAllResponse.NodeResponse>();

            CreateMap<Shared.Models.ApiModels.Nodes.NodesGetAllRequest,
                Data.Repositories.Nodes.Models.NodeFilter>();
        }
    }
}
