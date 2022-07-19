using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;

namespace SitefinityWebApp.Services.Models
{
    public class SearchResultItemModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string DefaultUrl { get; set; }
        public string[] Categories { get; set; }
        public string[] CategoriesNames
        {
            get
            {
                if (Categories == null)
                    return null;

                string[] names = new string[Categories.Length];
                for(int i =0; i< Categories.Length; i++)
                {
                    Guid tId = new Guid();
                    if(Guid.TryParse(this.Categories[i], out tId))
                    {
                        names[i] = GetTaxonName(Guid.Parse(this.Categories[i]), "Categories");
                    }
                    else
                    {
                        names[i] = this.Categories[i];
                    }
                }
                return names;
            }
        }
        public SearchContentType Type { get; set; }
        public string PublicationDateString { get; set; }
        public DateTime? PublicationDate
        {
            get
            {
                if (string.IsNullOrEmpty(PublicationDateString))
                    return null;
                try
                {
                    int y = int.Parse(PublicationDateString.Substring(0, 4));
                    int m = int.Parse(PublicationDateString.Substring(4, 2));
                    int d = int.Parse(PublicationDateString.Substring(6, 2));
                    return new DateTime(y, m, d);
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
        }

        private string GetTaxonName(Guid taxonId, string taxonomyName)
        {
            var taxonomyManager = TaxonomyManager.GetManager();
            var taxonomy = taxonomyManager.GetTaxonomies<HierarchicalTaxonomy>().Where(t => t.Title == taxonomyName).SingleOrDefault();
            var title = string.Empty;
            var taxon = taxonomy.Taxa.Where(tx => tx.Id == taxonId).FirstOrDefault();
            if (taxon != null)
            {
                title = taxon.Title.ToString();
            }
            return title;
        }

    }
}