using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MindKeeper.DataAccess.SeedData.Models
{
    internal class ScientificDomainModel
    {
        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("subdomains")]
        public List<string> Subdomains { get; set; }
    }
}
