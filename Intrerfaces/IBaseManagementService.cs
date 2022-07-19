using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.DynamicModules.Model;

namespace SitefinityWebApp.Services.Intrerfaces
{
    public interface IBaseManagementService : IBaseServiceProvider
    {
        Guid GetRelatedContentChildItemId(Guid contentId, string fieldName);
        List<Guid> GetRelatedContentChildItemIds(Guid contentId, string fieldName);
        List<Guid> GetRelatedContentParentItemIds(Guid childItemId, string fieldName);
        List<DynamicContent> GetMasterContent();
        List<DynamicContent> GetLiveContent();
    }
}