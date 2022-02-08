using System;
using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeaCreateRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Descritpion { get; set; }
        public Guid? ParentId { get; set; }
    }
}
