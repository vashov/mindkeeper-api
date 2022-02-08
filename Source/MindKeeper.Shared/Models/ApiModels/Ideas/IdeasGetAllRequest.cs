using System;
using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeasGetAllRequest
    {
        /// <summary>
        /// Filter by user
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Filter by ideas whose name starts with this parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Filter by parent idea.
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Filter by selected idea's id.
        /// </summary>
        public List<Guid> Ideas { get; set; }
    }
}
