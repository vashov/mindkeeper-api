using AutoMapper;
using MindKeeper.Shared.Models;

namespace MindKeeper.Api.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MindKeeper.Api.Data.Entities.Node,
                Shared.Models.ApiModels.Nodes.NodeGetResult>();

            CreateMap<MindKeeper.Api.Data.Entities.Node,
                Shared.Models.ApiModels.Nodes.NodesGetAllResult.NodeResponse>();

            CreateMap<Shared.Models.ApiModels.Nodes.NodesGetAllRequest,
                Data.Repositories.Nodes.Models.NodeFilter>();
        }
    }
}
