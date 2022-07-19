using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Models
{
    public class ImageModel : BaseMediaModel
    {
        public string AlternativeText { get; set; }

        public IEnumerable<ThumbnailImageModel> Thumbnails { get; set; }

        public ImageModel()
        {
            this.Thumbnails = new List<ThumbnailImageModel>();
        }
    }
}