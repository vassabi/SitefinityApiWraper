using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Taxonomies.Model;

namespace SitefinityWebApp.Services.Models
{
    public class SearchResultFilterModel
    {
        public List<SearchCategory> Categories { get; set; }
        public List<SearchResultItemModel> SearchResults { get; set; }
        public string SearchTerm { get; set; }
        public int SearchResultsCount { get; set; }

        public class SearchCategory
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public List<SearchCategory> Children { get; set; }
        }
    }
}