using System.Collections.Generic;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Modules.Libraries;
using SitefinityWebApp.Services.Models;

namespace SitefinityWebApp.Services.Extensions
{
    public static class ImageExtensions
    {
        public static ImageModel GetImageModel(this Image source)
        {
            ImageModel model = new ImageModel();
            if (source != null)
            {
                model.Id = source.OriginalContentId.IsNotNullOrEmpty() ? source.OriginalContentId : source.Id;
                model.LiveId = source.Id;
                model.OriginalContentId = source.OriginalContentId;
                model.Title = source.Title;
                model.Url = source.MediaUrl;
                model.Thumbnails = source.GetThumbnailModels();
                model.AlternativeText = source.AlternativeText;
                model.Description = source.Description;
                model.ItemDefaultUrl = source.ItemDefaultUrl;
            }
            return model;
        }

        public static IEnumerable<ThumbnailImageModel> GetThumbnailModels(this Image source)
        {
            List<ThumbnailImageModel> thumbnails = new List<ThumbnailImageModel>();

            if (source.Thumbnails != null)
            {
                source.Thumbnails.While(t =>
                {
                    thumbnails.Add(new ThumbnailImageModel
                    {
                        ThumbnailType = t.Name.ToLower(),
                        Url = t.ResolveMediaUrl()
                    });
                });
            }

            return thumbnails;
        }
    }

}