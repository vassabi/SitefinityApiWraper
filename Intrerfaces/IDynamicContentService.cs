using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Model.ContentLinks;

namespace SitefinityWebApp.Services.Intrerfaces
{
    public interface IDynamicContentService
    {
        Guid SiteId { get; set; }
        void SwitchToSpecificSiteContext(Guid siteId, string type);
        void SwitchToCurrentSiteContext(string type);
        bool IsCurrentSiteContext();
        Type GetType(string type);
        void LockContent(Guid id, string type, string fieldNameOfCulture);
        void UnlockContent(Guid id, string type, string fieldNameOfCulture);
        bool ContentIsLocked(Guid id, string type, string fieldNameOfCulture);
        void CreateVersion(IDataItem item, bool isPublished);
        void CommitTransaction();
        void SaveChanges();
        string SubmitMasterForApproval(DynamicContent content);
        DynamicContent Publish(DynamicContent content);
        IQueryable<DynamicContent> GetLiveContent(string type);
        IQueryable<DynamicContent> GetLiveContent(DynamicContent parent, string type);
        IQueryable<DynamicContent> GetTempContent(string type);
        IQueryable<DynamicContent> GetMasterContent(string type);
        IQueryable<DynamicContent> GetMasterContent(DynamicContent parent, string type);
        IQueryable<DynamicContent> GetContent(Type type, ContentLifecycleStatus status, bool checkVisible = true);
        IQueryable<DynamicContent> GetContent(DynamicContent parent, Type type, ContentLifecycleStatus status, bool checkVisible = true);
        IQueryable<DynamicContent> GetRelatedItems(DynamicContent parent, string relatedFieldName, ContentLifecycleStatus lifecycleStatus, bool checkVisible = true);
        DynamicContent CheckOutContent(DynamicContent content);
        DynamicContent CheckInContent(DynamicContent content);
        DynamicContent GetContent(string type, Guid id);
        DynamicContent GetContent(Type type, Guid id);
        DynamicContent GetLiveContent(Type type, Func<DynamicContent, bool> filter);
        DynamicContent GetMasterContent(Type type, Func<DynamicContent, bool> filter);
        DynamicContent Create(string type);
        DynamicContent Create(Type type);
        void RecompileDataItemsUrls(string type);
        void Publish(DynamicContent content, string applicationName);
        string ApplicationName { get; set; }
        string ProviderName { get; set; }
        ContentLink GetRelatedContentLink(Guid parentOriginalContentId, string fieldName);
        IEnumerable<ContentLink> GetAllRelatedContentLinksByChild(DynamicContent item);
        IEnumerable<ContentLink> GetAllRelatedContentLinksByChildItemId(Guid id);
        IEnumerable<ContentLink> GetRelatedContentLinks(DynamicContent parent, string type);
        IEnumerable<ContentLink> GetRelatedContentLinks(DynamicContent parent, Type type);
        IEnumerable<ContentLink> GetRelatedContentLinks(DynamicContent parent, string parentType, string childType);
        IEnumerable<ContentLink> GetRelatedContentLinks(DynamicContent parent, Type parentType, Type childType);
        IEnumerable<DynamicContent> GetChildrenOfMasterVersionOfParent(DynamicContent parent, string parentType, string childType);
        IEnumerable<DynamicContent> GetContentItems(IEnumerable<ContentLink> links, string type);
        IEnumerable<DynamicContent> GetContentItems(IEnumerable<ContentLink> links, Type type);
        IEnumerable<DynamicContent> GetChildrenOfMaster(DynamicContent parent, string type);
        IEnumerable<ContentLink> GetRelatedContentLinksForMaster(DynamicContent parent, Type parentType, Type childType);
        IEnumerable<ContentLink> GetRelatedContentLinksForMaster(DynamicContent parent, string parentType, string childType);

        IEnumerable<T> GetRelatedParentItems<T>(DynamicContent source, string parentItemProviderName = null, string fieldName = null) where T : IDataItem;
        IEnumerable<DynamicContent> GetChildren(DynamicContent parent, string type);
        IQueryable<ContentLink> GetContentLinks();
        bool CheckIfItemHasChildren(DynamicContent parent, string type);
    }

}