using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Modules.Libraries;

namespace SitefinityWebApp.Services.Intrerfaces
{
    public interface ILibrariesService
    {
        Video GetVideoFromId(Guid videoId);
        string GetImageUrlFromId(Guid imageId);
        Image GetImageFromId(Guid imageId);
        Document GetDocumentFromId(Guid documentId);
        IEnumerable<Image> GetImagesFromAlbum(string album);
        IEnumerable<Image> GetImagesFromAlbumByUrlName(string albumUrlName);
        Album GetAlbum(string albumName);
        IEnumerable<Album> GetAlbums();
        IQueryable<Image> GetImages();
        IEnumerable<Image> GetImagesByOriginalContentIds(IEnumerable<Guid> guids);
        IEnumerable<Image> GetImagesFromIds(IEnumerable<Guid> guids);
        Document GetDocumentByTitle(string title);
        Guid GetRelatedImageId(string parentItemType, Guid parentItemId, string fieldName);
        string GetProviderName();
        LibrariesManager manager();
    }

}