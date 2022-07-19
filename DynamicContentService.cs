using SitefinityWebApp.Services.Intrerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.ContentLinks;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Model.ContentLinks;
using Telerik.Sitefinity.Multisite;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Workflow;

namespace SitefinityWebApp.Services
{
    public class DynamicContentService : IDynamicContentService
    {
        public Guid SiteId { get; set; }

        private DynamicModuleManager DynamicModuleManager { get; set; }
        private ContentLinksManager ContentLinksManager { get; set; }
        private IVersionService VersionService { get; set; }
        private string transactionName { get; set; }

        public DynamicContentService()
        {
            this.DynamicModuleManager = DynamicModuleManager.GetManager(DynamicModuleManager.GetDefaultProviderName());
            this.ContentLinksManager = ContentLinksManager.GetManager();
        }

        public DynamicContentService(string type)
            : this()
        {
            MultiSiteService multiSiteService = new MultiSiteService();
            ISite currentSite = multiSiteService.GetCurrentSite();

            this.SetSiteContext(type, currentSite);
        }

        public DynamicContentService(string type, string transactionName)
            : this(type)
        {
            this.transactionName = transactionName;
            this.VersionService = new VersionService(this.transactionName);
        }

        public void SwitchToSpecificSiteContext(Guid siteId, string type)
        {
            MultiSiteService multiSiteService = new MultiSiteService();
            ISite newSite = multiSiteService.GetSiteById(siteId);

            this.SetSiteContext(type, newSite);
        }

        public void SwitchToCurrentSiteContext(string type)
        {
            MultiSiteService multiSiteService = new MultiSiteService();
            ISite currentSite = multiSiteService.GetCurrentSite();

            this.SetSiteContext(type, currentSite);
        }

        public bool IsCurrentSiteContext()
        {
            return this.SiteId == new MultiSiteService().GetCurrentSite().Id;
        }

        public Type GetType(string type)
        {
            return TypeResolutionService
                .ResolveType(type);
        }

        public void LockContent(Guid id, string type, string fieldNameOfCulture)
        {
            var content = this.GetMasterContent(type).Where(x => x.GetValue<string>(fieldNameOfCulture) != null && x.Id == id).FirstOrDefault();
            this.DynamicModuleManager.Lifecycle.CheckOut(content);
            this.SaveChanges();
        }

        public void UnlockContent(Guid id, string type, string fieldNameOfCulture)
        {
            var content = this.GetMasterContent(type).Where(x => x.GetValue<string>(fieldNameOfCulture) != null && x.Id == id).FirstOrDefault();
            this.DynamicModuleManager.Lifecycle.DiscardAllTemps(content);
            this.SaveChanges();
        }

        public bool ContentIsLocked(Guid id, string type, string fieldNameOfCulture)
        {
            bool result = false;
            var tempItems = this.GetTempContent(type).Where(x => x.OriginalContentId == id && x.GetValue<string>(fieldNameOfCulture) != null).FirstOrDefault();
            if (tempItems != null)
            {
                result = true;
            }
            return result;
        }

        public DynamicContent CheckOutContent(DynamicContent content)
        {
            return (DynamicContent)this.DynamicModuleManager.Lifecycle.CheckOut(content);
        }

        public DynamicContent CheckInContent(DynamicContent content)
        {
            return (DynamicContent)this.DynamicModuleManager.Lifecycle.CheckIn(content);
        }

        public void DiscardAllTemps(DynamicContent masterContent)
        {
            this.DynamicModuleManager.Lifecycle.DiscardAllTemps(masterContent);
        }

        public void CreateVersion(IDataItem item, bool isPublished)
        {
            if (this.VersionService != null)
                this.VersionService.CreateVersion(item, isPublished);
        }

        public void CommitTransaction()
        {
            if (this.VersionService != null)
                this.VersionService.CommitTransaction();
        }

        public void SaveChanges()
        {
            using (new ElevatedModeRegion(this.DynamicModuleManager))
            {
                this.DynamicModuleManager.SaveChanges();
            }
        }

        public string SubmitMasterForApproval(DynamicContent content)
        {
            var checkedOutContent = (DynamicContent)this.DynamicModuleManager.Lifecycle.CheckOut(content);
            this.DynamicModuleManager.SaveChanges();
            var bag = new Dictionary<string, string>();
            bag.Add("ContentType", content.GetType().FullName);
            var message = WorkflowManager.MessageWorkflow(content.Id, content.GetType(),
                this.ProviderName, "SendForApproval", true, bag);
            return message;
        }

        public DynamicContent Publish(DynamicContent content)
        {
            var temp = (DynamicContent)this.DynamicModuleManager.Lifecycle.CheckOut(content);
            temp.ApprovalWorkflowState.Value = "Published";
            var checkedIn = (DynamicContent)this.DynamicModuleManager.Lifecycle.CheckIn(temp);
            var publishedItem = this.DynamicModuleManager.Lifecycle.Publish(checkedIn);
            this.VersionService.CreateVersion(checkedIn, true);
            content.SetWorkflowStatus(this.ApplicationName, "Published");
            this.DynamicModuleManager.SaveChanges();
            return (DynamicContent)publishedItem;
        }

        public IQueryable<DynamicContent> GetLiveContent(string type)
        {
            return this.GetContent(this.GetType(type), ContentLifecycleStatus.Live);
        }

        public IQueryable<DynamicContent> GetTempContent(string type)
        {
            return this.GetContent(this.GetType(type), ContentLifecycleStatus.Temp, false);
        }

        public IQueryable<DynamicContent> GetLiveContent(DynamicContent parent, string type)
        {
            return this.GetContent(parent, this.GetType(type), ContentLifecycleStatus.Live);
        }

        public IQueryable<DynamicContent> GetMasterContent(string type)
        {
            return this.GetContent(this.GetType(type), ContentLifecycleStatus.Master, false);
        }

        public IQueryable<DynamicContent> GetMasterContent(DynamicContent parent, string type)
        {
            return this.GetContent(parent, this.GetType(type), ContentLifecycleStatus.Master, false);
        }

        public IQueryable<DynamicContent> GetRelatedItems(DynamicContent parent, string relatedFieldName, ContentLifecycleStatus lifecycleStatus, bool checkVisible = true)
        {
            if (checkVisible)
            {
                return parent.GetRelatedItems<DynamicContent>(relatedFieldName).Where(x => x.Status == lifecycleStatus && x.Visible == true);
            }
            else
            {
                return parent.GetRelatedItems<DynamicContent>(relatedFieldName).Where(x => x.Status == lifecycleStatus);
            }

        }

        public IQueryable<DynamicContent> GetContent(Type type, ContentLifecycleStatus status, bool checkVisible = true)
        {
            if (checkVisible)
            {
                return this.DynamicModuleManager.GetDataItems(type)
                    .Where(c => c.Status == status && c.Visible);
            }
            else
            {
                return this.DynamicModuleManager.GetDataItems(type)
                    .Where(c => c.Status == status);
            }

        }

        public ContentLink GetRelatedContentLink(Guid parentOriginalContentId, string fieldName)
        {
            return GetContentLinks().FirstOrDefault(x => x.ParentItemId == parentOriginalContentId && x.ComponentPropertyName == fieldName);
        }

        public IQueryable<ContentLink> GetContentLinks()
        {
            return this.ContentLinksManager.GetContentLinks().Where(x => !x.IsChildDeleted && !x.IsParentDeleted);
        }

        public IQueryable<DynamicContent> GetContent(DynamicContent parent, Type type, ContentLifecycleStatus status, bool checkVisible = true)
        {
            if (checkVisible)
            {
                return this.DynamicModuleManager.GetChildItems(parent, type)
                    .Where(c => c.Status == status && c.Visible);
            }
            else
            {
                return this.DynamicModuleManager.GetDataItems(type)
                    .Where(c => c.Status == status);
            }
        }

        public DynamicContent GetContent(string type, Guid id)
        {
            return this.GetContent(this.GetType(type), id);
        }

        public DynamicContent GetContent(Type type, Guid id)
        {
            return this.DynamicModuleManager.GetDataItem(type, id);
        }

        public DynamicContent GetLiveContent(Type type, Func<DynamicContent, bool> filter)
        {
            return this.GetContent(type, ContentLifecycleStatus.Live)
                .Where(filter)
                .SingleOrDefault();
        }

        public DynamicContent GetMasterContent(Type type, Func<DynamicContent, bool> filter)
        {
            return this.GetContent(type, ContentLifecycleStatus.Master)
                .Where(filter)
                .SingleOrDefault();
        }

        public DynamicContent Create(string type)
        {
            return this.Create(this.GetType(type));
        }

        public DynamicContent Create(Type type)
        {
            using (new ElevatedModeRegion(this.DynamicModuleManager))
            {
                return this.DynamicModuleManager.CreateDataItem(type);
            }
        }

        public void RecompileDataItemsUrls(string type)
        {
            using (new ElevatedModeRegion(this.DynamicModuleManager))
            {
                this.DynamicModuleManager.RecompileDataItemsUrls(this.GetType(type));
            }
        }

        public void Publish(DynamicContent content, string applicationName)
        {
            using (new ElevatedModeRegion(this.DynamicModuleManager))
            {
                //DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(applicationName);
                content.SetWorkflowStatus(applicationName, "Draft");
                this.DynamicModuleManager.SaveChanges();
                this.DynamicModuleManager.Lifecycle.Publish(content);
                content.SetWorkflowStatus(applicationName, "Published");
                this.DynamicModuleManager.SaveChanges();
            }
        }

        public string ApplicationName
        {
            get
            {
                return this.DynamicModuleManager.Provider.ApplicationName;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ProviderName
        {
            get
            {
                return this.DynamicModuleManager.Provider.ToString();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ContentLink> GetAllRelatedContentLinksByChild(DynamicContent child)
        {
            return this.GetAllRelatedContentLinksByChildItemId(child.OriginalContentId);
        }

        public IEnumerable<ContentLink> GetAllRelatedContentLinksByChildItemId(Guid id)
        {
            return this.ContentLinksManager.GetContentLinks().Where(link => link.ChildItemId == id);
        }

        public IEnumerable<ContentLink> GetRelatedContentLinks(DynamicContent parent, string parentType)
        {
            return this.GetRelatedContentLinks(parent, TypeResolutionService.ResolveType(parentType));
        }

        public IEnumerable<ContentLink> GetRelatedContentLinks(DynamicContent parent, string parentType, string childType)
        {
            return this.GetRelatedContentLinks(parent, TypeResolutionService.ResolveType(parentType), TypeResolutionService.ResolveType(childType));
        }

        public IEnumerable<ContentLink> GetRelatedContentLinks(DynamicContent parent, Type parentType, Type childType)
        {
            return this.GetRelatedContentLinks(parent, parentType)
                .Where(p => p.ChildItemType == childType.ToString());
        }

        public IEnumerable<ContentLink> GetRelatedContentLinks(DynamicContent parent, Type parentType)
        {
            return this.ContentLinksManager.GetContentLinks().Where(link => link.ParentItemType == parentType.ToString() && link.ParentItemId == parent.OriginalContentId);
        }

        //public IEnumerable<ContentLink> GetRelatedContentLinksForMaster(DynamicContent parent, string parentType)
        //{
        //    return this.GetRelatedContentLinksForMaster(parent, TypeResolutionService.ResolveType(parentType));
        //}

        public IEnumerable<ContentLink> GetRelatedContentLinksForMaster(DynamicContent parent, Type parentType, Type childType)
        {
            return this.ContentLinksManager.GetContentLinks().Where(link => link.ParentItemType == parentType.ToString() && link.ParentItemId == parent.Id && link.ChildItemType == childType.ToString());
        }

        public IEnumerable<ContentLink> GetRelatedContentLinksForMaster(DynamicContent parent, string parentType, string childType)
        {
            return this.GetRelatedContentLinksForMaster(parent, TypeResolutionService.ResolveType(parentType), TypeResolutionService.ResolveType(childType));
        }

        public IEnumerable<DynamicContent> GetContentItems(IEnumerable<ContentLink> links, string type)
        {
            return this.GetContentItems(links, TypeResolutionService.ResolveType(type));
        }

        public IEnumerable<DynamicContent> GetContentItems(IEnumerable<ContentLink> links, Type type)
        {
            IEnumerable<Guid> contentIds = links.Select(l => l.ChildItemId);

            return this.DynamicModuleManager
                .GetDataItems(type)
                .Where(i => contentIds.Contains(i.OriginalContentId));
        }


        public IEnumerable<T> GetRelatedParentItems<T>(DynamicContent source, string parentItemProviderName = null, string fieldName = null) where T : IDataItem
        {
            return source.GetRelatedParentItems<T>(parentItemProviderName, fieldName);
        }

        public IEnumerable<DynamicContent> GetChildren(DynamicContent parent, string type)
        {
            return this.DynamicModuleManager.GetChildItems(parent, this.GetType(type))
                .Where(c => c.Status == ContentLifecycleStatus.Live && c.Visible == true);
        }

        public IEnumerable<DynamicContent> GetChildrenOfMaster(DynamicContent parent, string type)
        {
            return this.DynamicModuleManager.GetChildItems(parent, this.GetType(type)).Where(x => x.Status == ContentLifecycleStatus.Master);
        }

        public IEnumerable<DynamicContent> GetChildrenOfMasterVersionOfParent(DynamicContent parent, string parentType, string childType)
        {
            List<DynamicContent> result = new List<DynamicContent>();
            var links = this.GetRelatedContentLinksForMaster(parent, parentType, childType);
            if (links != null && links.Any())
            {
                foreach (var link in links)
                {
                    Guid childItemId = link.ChildItemId;
                    DynamicContent item = this.GetMasterContent(childType).Where(x => x.OriginalContentId == childItemId).FirstOrDefault();
                    if (item != null)
                    {
                        result.Add(item);
                    }
                    //get dynamic content item based on childitemid
                }
            }
            return result;
        }


        public bool CheckIfItemHasChildren(DynamicContent parent, string type)
        {
            return this.DynamicModuleManager.HasChildItems(parent);
        }

        public bool ContentExists(Type type, string contentTitle)
        {
            var contentItemExists = this.DynamicModuleManager.GetDataItems(type)
                                             .Where(i => i.Status == ContentLifecycleStatus.Master)
                                             .Any(x => x.GetValue<string>("Title") == contentTitle);

            return contentItemExists;
        }

        private void SetSiteContext(string type, ISite site)
        {
            string providerName = this.GetProviderName(type, site);

            this.DynamicModuleManager = DynamicModuleManager.GetManager(providerName);
            this.SiteId = site.Id;
        }

        private string GetProviderName(string type, ISite site)
        {
            string result = "";

            if (site != null)
            {
                var provider = site.GetDefaultProvider(type);

                if (provider != null)
                {
                    result = provider.ProviderName;
                }
            }

            return !string.IsNullOrEmpty(result) ?
                result : DynamicModuleManager.GetDefaultProviderName();
        }
    }

}