using System;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Taxonomies.Model;


namespace SitefinityWebApp.Services.Intrerfaces
{
    public interface ITaxonomyService
    {
        bool FlatTaxonomyExists(string name);
        bool HierarchicalTaxonomyExists(string name);
        void CreateFlatTaxonomy(string taxonName, string name, string title, string description, Guid id);
        void CreateHierarchicalTaxonomy(string taxonName, string name, string title, string description, Guid id);
        IEnumerable<string> GetTaxonomyTitlesFromField(string taxonomyName, string fieldName, DynamicContent content);
        string GetSingleTaxonomyTitleFromField(string taxonomyName, string fieldName, DynamicContent content);
        IEnumerable<Taxon> GetTaxonsForItemField(DynamicContent content, string fieldName, string taxonomyName);
        FlatTaxon GetFlatTaxonByClassificationNameTaxonTitle(string classificationName, string taxonTitle);
        List<FlatTaxon> GetAllFlatTaxonsByName(string classificationName);
        FlatTaxonomy GetFlatTaxonomyByTitle(string classificationTitle);
        FlatTaxonomy GetFlatTaxonomyByName(string classificationName);
        HierarchicalTaxon GetClassificationBranch(string classificationName, string branchTitle);
        HierarchicalTaxon CreateHierarchicalTaxonByTaxonomy(string TaxonomyName, string NewTaxonName, string TaxonDescription = "");
        List<HierarchicalTaxon> GetAllHierarchicalTaxonsByName(string classificationName);
        List<HierarchicalTaxon> GetChildHierarchicalTaxonsForClassification(string classificationName);
        List<Guid> GetHierarchalTaxonGuidsForItem(DynamicContent item, string fieldName, string taxonomyName);
        List<HierarchicalTaxon> GetHierarchicalTaxons(DynamicContent item, string fieldName, string taxonomyName);
        FlatTaxon CreateFlatTaxonValue(string taxonomyName, string taxonValue);
        List<FlatTaxon> GetFlatTaxonsFromGuidList(TrackedList<Guid> taxonGuids);
        List<HierarchicalTaxon> GetHierarchicalTaxonsFromGuidList(TrackedList<Guid> taxonGuids);
        HierarchicalTaxonomy GetHierarchicalTaxonomyForClassification(string classificationName);
        HierarchicalTaxon AddHierarchicalTaxonToRootTaxon(HierarchicalTaxonomy taxonomy, HierarchicalTaxon rootTaxon, string newChildTaxonName);
    }

}