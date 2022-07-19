using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Models
{
    public class SearchResultSetModel
    {
        public SearchResultSetModel()
        {
            this.Results = new List<SearchResultItemModel>();
        }
        public List<SearchResultItemModel> Results { get; set; }
        public int TotalResultsCount { get; set; }
        public int TotalPagesCount { get; set; }
    }
}