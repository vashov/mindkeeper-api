using System;
using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeaCreateRequest
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(length: 100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(length: 1000)]
        public string Description { get; set; }
        public Guid? ParentIdeaId { get; set; }
        public Guid? CountryId { get; set; }
        //public Guid? DomainId { get; set; }
        public Guid? SubdomainId { get; set; }
    }
}
