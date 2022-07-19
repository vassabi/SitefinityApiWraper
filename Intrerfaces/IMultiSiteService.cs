using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Multisite;

namespace SitefinityWebApp.Services.Intrerfaces
{
    public interface IMultiSiteService
    {
        string CurrentSiteName();
        void ChangeCurrentSite(string newSiteName);
        ISite GetSiteByName(string siteName);
        ISite GetSiteById(Guid siteId);
        IEnumerable<ISite> GetSites(Func<ISite, bool> query);
        IEnumerable<ISite> GetSites();
        string ResolveUrl(string url);
        ISite GetCurrentSite();
        bool DoesSiteExist(string siteName);
        IEnumerable<MultisiteContext.SiteDataSourceLinkProxy> GetProviders(Type type);
        IEnumerable<MultisiteContext.SiteDataSourceLinkProxy> GetProviders(string typeName);
    }
}