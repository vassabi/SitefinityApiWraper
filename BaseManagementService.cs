using SitefinityWebApp.Services.Extensions;
using SitefinityWebApp.Services.Intrerfaces;
using SitefinityWebApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model.ContentLinks;

namespace SitefinityWebApp.Services
{
    public class BaseManagementService : IBaseManagementService
    {
        internal IDynamicContentService _service { get; set; }

        public string ServiceType { get; }
        public string ServiceTypeShortName { get; }

        public BaseManagementService(string managementServiceType, string managementServiceTypeShortName)
        {
            this.ServiceType = managementServiceType;
            this.ServiceTypeShortName = managementServiceTypeShortName;
            this._service = new DynamicContentService(this.ServiceTypeShortName);
        }

        public string GetApplicationName()
        {
            return this._service.ApplicationName;
        }

        public string GetProviderName()
        {
            return this._service.ProviderName;
        }

        public Guid GetRelatedContentChildItemId(Guid contentId, string fieldName)
        {
            ContentLink cLink = this._service.GetContentLinks().FirstOrDefault(x => x.ParentItemId == contentId && x.ComponentPropertyName == fieldName);
            if (cLink != null) return cLink.ChildItemId;
            return new Guid();
        }




        public List<Guid> GetRelatedContentChildItemIds(Guid contentId, string fieldName)
        {
            var cLinks = this._service.GetContentLinks().Where(x => x.ParentItemId == contentId && x.ComponentPropertyName == fieldName);
            if (cLinks != null)
                return cLinks.Select(c => c.ChildItemId).ToList();
            return new List<Guid>();
        }

        public List<Guid> GetRelatedContentParentItemIds(Guid childItemId, string fieldName)
        {
            var cLinks = this._service.GetContentLinks().Where(x => x.ChildItemId == childItemId && x.ComponentPropertyName == fieldName);
            if (cLinks != null)
                return cLinks.Select(c => c.ParentItemId).ToList();
            return new List<Guid>();
        }

        public List<DynamicContent> GetMasterContent()
        {
            return this._service.GetMasterContent(this.ServiceType).ToList();
        }

        public List<DynamicContent> GetLiveContent()
        {
            return this._service.GetLiveContent(this.ServiceType).ToList();
        }

        public List<DynamicContent> GetChildren(DynamicContent content, string type)
        {
            return this._service.GetChildren(content, type).ToList();
        }

        private ContentItemModelBase GetContentItemModelBase(DynamicContent content, string titleFieldName)
        {
            var model = new ContentItemModelBase();
            if (content.IsNotNull())
                model.CreateContentItemModelBase(content, titleFieldName);

            return model;
        }
    }

}