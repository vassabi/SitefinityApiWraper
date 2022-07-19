using SitefinityWebApp.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Models
{
    public class ContentItemModelBase
    {
        public bool Exists { get { return this.OriginalContentId.IsNotNullOrEmpty(); } }
        public string Title { get; set; }
        public string UrlName { get; set; }
        public string DefaultUrl { get; set; }
        public string ProviderName { get; set; }
        public string FirstCharacter { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public Guid LiveId { get; set; }
        public Guid OriginalContentId { get; set; }
        public Guid SystemParentId { get; set; }
    }
}