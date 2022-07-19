using SitefinityWebApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace SitefinityWebApp.Services.Extensions
{
    public static class ContentItemModelBaseExtensions
    {
        public static void CreateContentItemModelBase(this ContentItemModelBase item, DynamicContent content, string titleFieldName)
        {
            item.Title = content.GetValue<Lstring>(titleFieldName).NullToString();
            item.UrlName = content.UrlName;
            item.DefaultUrl = content.ItemDefaultUrl;
            item.LiveId = content.Id;
            item.OriginalContentId = content.OriginalContentId;
            item.SystemParentId = content.SystemParentId;
            item.LastModifiedDate = content.LastModified;
        }
    }
}