using System;
using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Countries
{
    public class CountryGetAllResult
    {
        public class Country
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public List<Country> Countries { get; set; }
    }
}
