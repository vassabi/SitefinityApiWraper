using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.OpenAccess;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Data;
using System.Text.RegularExpressions;
using SitefinityWebApp.Services.Intrerfaces;
using SitefinityWebApp.Services.Extensions;

namespace SitefinityWebApp.Services
{
    public class TaxonomyService : ITaxonomyService
    {
        private TaxonomyManager _taxonomyManager { get; set; }

        public TaxonomyService()
        {
            this._taxonomyManager = TaxonomyManager.GetManager();
        }

        public bool FlatTaxonomyExists(string name)
        {
            return this.GetFlatTaxonomyByName(name).IsNotNull();
        }
        public bool HierarchicalTaxonomyExists(string name)
        {
            return this.GetHierarchicalTaxonomyForClassification(name).IsNotNull();
        }

        public IEnumerable<string> GetTaxonomyTitlesFromField(string taxonomyName, string fieldName, DynamicContent content)
        {
            var taxonomies = this.GetTaxonsForItemField(content, fieldName, taxonomyName);
            if (taxonomies.IsNotNull() && taxonomies.Count() > 0)
            {
                return taxonomies.Select(t => t.Title.Value);
            }
            return new List<string>();
        }

        public string GetSingleTaxonomyTitleFromField(string taxonomyName, string fieldName, DynamicContent content)
        {
            var taxonomy = this.GetTaxonsForItemField(content, fieldName, taxonomyName);
            if (taxonomy.IsNotNull() && taxonomy.Count() > 0)
            {
                return taxonomy.FirstOrDefault().Title;
            }
            return "";
        }

        public void CreateFlatTaxonomy(string taxonName, string name, string title, string description, Guid id)
        {
            using (new ElevatedModeRegion(this._taxonomyManager))
            {
                var taxonomy = this._taxonomyManager.CreateTaxonomy<FlatTaxonomy>(id);
                taxonomy.TaxonName = taxonName;
                taxonomy.Name = name;
                taxonomy.Title = title;
                taxonomy.Description = description;
                this._taxonomyManager.SaveChanges();
            }
        }

        public void CreateHierarchicalTaxonomy(string taxonName, string name, string title, string description, Guid id)
        {
            using (new ElevatedModeRegion(this._taxonomyManager))
            {
                var taxonomy = this._taxonomyManager.CreateTaxonomy<HierarchicalTaxonomy>(id);
                taxonomy.TaxonName = taxonName;
                taxonomy.Name = name;
                taxonomy.Title = title;
                taxonomy.Description = description;
                this._taxonomyManager.SaveChanges();
            }
        }

        public List<FlatTaxon> GetFlatTaxonsFromGuidList(TrackedList<Guid> taxonGuids)
        {
            var taxons = taxonGuids.Select(_taxonomyManager.GetTaxon<FlatTaxon>);
            return taxons.ToList();
        }

        public List<HierarchicalTaxon> GetHierarchicalTaxonsFromGuidList(TrackedList<Guid> taxonGuids)
        {
            var taxons = taxonGuids.Select(_taxonomyManager.GetTaxon<HierarchicalTaxon>);
            return taxons.ToList();
        }

        public FlatTaxonomy GetFlatTaxonomyByTitle(string classificationTitle)
        {
            FlatTaxonomy taxonomy = this._taxonomyManager.GetTaxonomies<FlatTaxonomy>().FirstOrDefault(t => t.Title.Equals(classificationTitle, StringComparison.InvariantCultureIgnoreCase));
            return taxonomy;
        }

        public FlatTaxonomy GetFlatTaxonomyByName(string classificationName)
        {
            FlatTaxonomy taxonomy = this._taxonomyManager.GetTaxonomies<FlatTaxonomy>().FirstOrDefault(t => t.Name.ToLower() == classificationName.ToLower());
            return taxonomy;
        }

        public FlatTaxon GetFlatTaxonByClassificationNameTaxonTitle(string classificationName, string taxonTitle)
        {
            var taxonsForClassification = this.GetAllFlatTaxonsByName(classificationName);
            if (taxonsForClassification.IsNotNull() && taxonsForClassification.Count > 0)
                return taxonsForClassification.FirstOrDefault(t => t.Title == taxonTitle);
            else
                return null;
        }

        public List<FlatTaxon> GetAllFlatTaxonsByName(string classificationName)
        {
            FlatTaxonomy taxonomy = this._taxonomyManager.GetTaxonomies<FlatTaxonomy>().FirstOrDefault(t => t.Name.ToLower() == classificationName.ToLower());
            return taxonomy != null && taxonomy.Taxa.Any() ? taxonomy.Taxa.Select(o => _taxonomyManager.GetTaxon<FlatTaxon>(o.Id)).ToList() : null;
        }

        public List<HierarchicalTaxon> GetAllHierarchicalTaxonsByName(string classificationName)
        {
            HierarchicalTaxonomy taxonomy = this._taxonomyManager.GetTaxonomies<HierarchicalTaxonomy>().FirstOrDefault(t => t.Name.ToLower() == classificationName.ToLower());
            return taxonomy != null ? taxonomy.Taxa.Select(o => _taxonomyManager.GetTaxon<HierarchicalTaxon>(o.Id)).ToList() : null;
        }

        /// <summary>
        /// Returns the first HierarchicalTaxon of the first level of the classification tree that matches the branchTitle parameter.
        /// </summary>
        public HierarchicalTaxon GetClassificationBranch(string classificationName, string branchTitle)
        {
            HierarchicalTaxonomy taxonomy = this.GetHierarchicalTaxonomyForClassification(classificationName);
            var taxa = taxonomy != null ? taxonomy.Taxa.Where(t => t.Parent == null && t.Title == branchTitle).FirstOrDefault() : null;
            return taxa != null ? (HierarchicalTaxon)taxa : null;
        }

        /// <summary>
        /// Returns all HierarchicalTaxon of the first level of the classification.
        /// </summary>
        public List<HierarchicalTaxon> GetChildHierarchicalTaxonsForClassification(string classificationName)
        {
            HierarchicalTaxonomy taxonomy = this.GetHierarchicalTaxonomyForClassification(classificationName);
            return taxonomy != null ? taxonomy.Taxa.Where(t => t.Parent == null).Select(o => _taxonomyManager.GetTaxon<HierarchicalTaxon>(o.Id)).ToList() : null;
        }

        public IEnumerable<Taxon> GetTaxonsForItemField(DynamicContent content, string fieldName, string taxonomyName)
        {
            return this.GetTaxonsForItem(content, fieldName, taxonomyName);
        }

        public List<Guid> GetHierarchalTaxonGuidsForItem(DynamicContent item, string fieldName, string taxonomyName)
        {
            return this.GetTaxonsForItem(item, fieldName, taxonomyName)
                .Select(t => t.Id)
                .ToList();
        }

        public List<HierarchicalTaxon> GetHierarchicalTaxons(DynamicContent item, string fieldName, string taxonomyName)
        {
            return this.GetTaxonsForItem(item, fieldName, taxonomyName)
                .Select(t => (HierarchicalTaxon)t)
                .ToList();
        }

        private IEnumerable<Taxon> GetTaxonsForItem(DynamicContent item, string fieldName, string taxonomyName)
        {
            TrackedList<Guid> categories = item.GetValue<TrackedList<Guid>>(fieldName);

            Taxonomy taxonomyParent = this._taxonomyManager.GetTaxonomies<Taxonomy>().FirstOrDefault(t => t.Name.ToLower() == taxonomyName.ToLower());

            return taxonomyParent.Taxa.Where(t => categories.Contains(t.Id));
        }

        public FlatTaxon CreateFlatTaxonValue(string taxonomyName, string taxonValue)
        {
            Taxonomy taxonomyParent = this._taxonomyManager.GetTaxonomies<Taxonomy>().FirstOrDefault(t => t.Name.ToLower() == taxonomyName.ToLower());
            FlatTaxon taxon = this._taxonomyManager.CreateTaxon<FlatTaxon>();
            taxon.Name = taxonValue;
            taxon.Title = taxonValue;
            taxon.UrlName = HttpUtility.UrlPathEncode(taxonValue.CreateUrlName());

            taxonomyParent.Taxa.Add(taxon);
            using (new ElevatedModeRegion(this._taxonomyManager))
            {
                _taxonomyManager.SaveChanges();
            }

            return taxon;
        }

        public HierarchicalTaxon CreateHierarchicalTaxonByTaxonomy(string TaxonomyName, string NewTaxonName, string TaxonDescription = "")
        {
            var taxon = this._taxonomyManager.CreateTaxon<HierarchicalTaxon>(Guid.NewGuid());
            taxon.Title = NewTaxonName;
            taxon.Name = NewTaxonName;
            taxon.Description = TaxonDescription;
            taxon.UrlName = getCleanUrlFromString(NewTaxonName);

            var taxonomy = this._taxonomyManager.GetTaxonomies<HierarchicalTaxonomy>().Where(t => t.Name == TaxonomyName).FirstOrDefault();
            taxon.Taxonomy = taxonomy;
            taxonomy.Taxa.Add(taxon);
            using (new ElevatedModeRegion(this._taxonomyManager))
            {
                this._taxonomyManager.SaveChanges();
            }

            return taxon;
        }

        /// <summary>
        /// Creates a new taxon given the root HierarchicalTaxonomy, the HierarchicalTaxon parent, and the name of the new Child Taxon
        /// </summary>
        /// <param name="taxonomy"></param>
        /// <param name="rootTaxon"></param>
        /// <param name="newChildTaxonName"></param>
        /// <returns></returns>
        public HierarchicalTaxon AddHierarchicalTaxonToRootTaxon(HierarchicalTaxonomy taxonomy, HierarchicalTaxon rootTaxon, string newChildTaxonName)
        {
            var newTaxon = this._taxonomyManager.CreateTaxon<HierarchicalTaxon>();
            newTaxon.Title = newChildTaxonName;
            newTaxon.Name = newChildTaxonName;
            newTaxon.UrlName = new Lstring(Regex.Replace(newChildTaxonName, @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-").ToLower());
            newTaxon.Description = string.Empty;
            newTaxon.Parent = rootTaxon;
            newTaxon.Taxonomy = taxonomy;
            rootTaxon.Subtaxa.Add(newTaxon);

            using (new Telerik.Sitefinity.Data.ElevatedModeRegion(this._taxonomyManager))
            {
                this._taxonomyManager.SaveChanges();
            }
            return newTaxon;
        }

        public HierarchicalTaxonomy GetHierarchicalTaxonomyForClassification(string classificationName)
        {
            HierarchicalTaxonomy taxonomy = this._taxonomyManager.GetTaxonomies<HierarchicalTaxonomy>().FirstOrDefault(t => t.Name.ToLower() == classificationName.ToLower());
            return taxonomy;
        }

        private string getCleanUrlFromString(string name)
        {
            var cleanUrlName = Regex.Replace(name, @"[^\w\d_\-\s]+", "");
            return Regex.Replace(cleanUrlName, @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
        }
    }

}