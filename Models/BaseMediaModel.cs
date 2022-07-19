using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Models
{
    public class BaseMediaModel
    {
        public bool Exists { get { return this.Id != Guid.Empty; } }
        public Guid OriginalContentId { get; set; }
        public Guid LiveId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string ItemDefaultUrl { get; set; }
    }
}