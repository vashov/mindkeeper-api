using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MindKeeper.DataAccess.PostgreSource.Seed.ScientificDomains
{
    internal class ScientificDomain
    {
        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("subdomains")]
        public List<string> Subdomains { get; set; }
    }
}
