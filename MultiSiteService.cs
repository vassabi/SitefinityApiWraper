using SitefinityWebApp.Services.Intrerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Multisite;
using Telerik.Sitefinity.Services;

namespace SitefinityWebApp.Services
{
    public class MultiSiteService : IMultiSiteService
    {
        private MultisiteContext _multisiteContext { get; set; }

        public MultiSiteService()
        {
            var multisiteContext = SystemManager.CurrentContext as MultisiteContext;
            if (multisiteContext != null)
            {
                this._multisiteContext = multisiteContext;
            }
            //this._multisiteContext = SystemManager.CurrentContext as MultisiteContext;
        }

        public string CurrentSiteName()
        {
            return this._multisiteContext.CurrentSite.Name;
        }

        public void ChangeCurrentSite(string newSiteName)
        {
            this._multisiteContext.ChangeCurrentSite(this.GetSiteByName(newSiteName));
        }

        public ISite GetSiteByName(string siteName)
        {
            return this._multisiteContext.GetSiteByName(siteName);
        }

        public ISite GetSiteById(Guid siteId)
        {
            return this._multisiteContext.GetSiteById(siteId);
        }

        public IEnumerable<ISite> GetSites(Func<ISite, bool> query)
        {
            return this.GetSites()
                .Where(query);
        }

        public bool DoesSiteExist(string siteName)
        {
            bool result = false;
            var site = this.GetSites().Where(x => x.Name == siteName).FirstOrDefault();
            if (site != null)
            {
                result = true;
            }
            return result;
        }

        public IEnumerable<ISite> GetSites()
        {
            return this._multisiteContext.GetSites();
        }

        public string ResolveUrl(string url)
        {
            return this._multisiteContext.ResolveUrl(url);
        }

        public ISite GetCurrentSite()
        {
            return _multisiteContext.CurrentSite;
        }

        public IEnumerable<MultisiteContext.SiteDataSourceLinkProxy> GetProviders(Type type)
        {
            return this.GetProviders(type.FullName);
        }

        public IEnumerable<MultisiteContext.SiteDataSourceLinkProxy> GetProviders(string typeName)
        {
            return this.GetCurrentSite().GetProviders(typeName);
        }
    }
}