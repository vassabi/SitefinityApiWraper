using SitefinityWebApp.Services.Intrerfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.ContentLinks;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model.ContentLinks;
using Telerik.Sitefinity.Modules.Libraries;

namespace SitefinityWebApp.Services
{
    public class LibrariesService : ILibrariesService
    {
        public LibrariesManager _librariesManager { get; set; }
        private ContentLinksManager contentLinksManager { get; set; }
        private readonly string ApprovalWorkflowStatePublished = "Published";

        public LibrariesService()
        {
            this._librariesManager = LibrariesManager.GetManager();
            this.contentLinksManager = ContentLinksManager.GetManager();
        }

        public LibrariesService(string providerName)
        {
            this._librariesManager = LibrariesManager.GetManager(providerName);
        }
        public LibrariesManager manager()
        {
            return this._librariesManager;
        }
        public string GetProviderName()
        {
            return this._librariesManager.Provider.Name.ToString();
        }

        #region Image
        public IEnumerable<Image> GetImagesByOriginalContentIds(IEnumerable<Guid> guids)
        {
            return _librariesManager.GetImages().Where(x => guids.Contains(x.OriginalContentId)
            && x.ApprovalWorkflowState.ToString() == "Published"
            && x.Visible).ToList();
        }

        public IEnumerable<Image> GetImagesFromIds(IEnumerable<Guid> guids)
        {
            return _librariesManager.GetImages().Where(x => guids.Contains(x.Id)
            && x.ApprovalWorkflowState.ToString() == "Published"
            && x.Visible).ToList();
        }

        public Guid GetRelatedImageId(string parentItemType, Guid parentItemId, string fieldName)
        {
            ContentLink cLink = this.contentLinksManager.GetContentLinks().FirstOrDefault(x => x.ParentItemId == parentItemId && x.ComponentPropertyName == fieldName && x.ParentItemType == parentItemType && x.ChildItemType == "Telerik.Sitefinity.Libraries.Model.Image");
            if (cLink != null) return cLink.ChildItemId;
            return new Guid();
        }

        public Image GetImageFromId(Guid imageId)
        {
            try
            {
                if (imageId != Guid.Empty)
                {
                    var image = _librariesManager.GetImage(imageId);
                    if (image != null)
                    {
                        if (image.ApprovalWorkflowState.ToString() == "Published" && image.Visible)
                        {
                            return image;
                        }
                        else // image is master and need to get live version
                        {
                            return _librariesManager.GetImages().Where(i => i.OriginalContentId == imageId && i.Visible).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
            return null;
        }

        public string GetImageUrlFromId(Guid imageId)
        {
            string imageUrl = String.Empty;
            string imageLinkFull = String.Empty;
            try
            {
                if (imageId != Guid.Empty)
                {
                    var image = _librariesManager.GetImage(imageId);
                    if (image != null)
                    {
                        if (image.ApprovalWorkflowState.ToString() == "Published" && image.Visible)
                        {
                            imageLinkFull = image.ResolveMediaUrl();
                            imageLinkFull = imageLinkFull.Replace("~/", "/");
                            imageUrl = imageLinkFull;
                        }
                        else // image is master and need to get live version
                        {
                            image = _librariesManager.GetImages().Where(i => i.OriginalContentId == imageId && i.Visible).FirstOrDefault();
                            if (image != null)
                            {
                                imageLinkFull = image.ResolveMediaUrl();
                                imageLinkFull = imageLinkFull.Replace("~/", "/");
                                imageUrl = imageLinkFull;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
            return imageUrl;
        }


        public IEnumerable<Image> GetImagesFromAlbum(string album)
        {
            var currentAlbum = _librariesManager.GetAlbums().FirstOrDefault(a => a.Title == album);
            if (currentAlbum != null)
            {
                return currentAlbum.Images().Where(i => i.Status == ContentLifecycleStatus.Live && i.Visible == true);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Image> GetImagesFromAlbumByUrlName(string albumUrlName)
        {
            var album = _librariesManager.GetAlbums().FirstOrDefault(a => a.UrlName == albumUrlName);
            if (album != null)
            {
                return album.Images().Where(i => i.Status == ContentLifecycleStatus.Live && i.Visible == true);
            }
            else
            {
                return null;
            }
        }

        public IQueryable<Image> GetImages()
        {
            return _librariesManager.GetImages();
        }
        #endregion

        #region Document
        public Document GetDocumentFromId(Guid documentId)
        {
            try
            {
                if (documentId != Guid.Empty)
                {
                    var document = _librariesManager.GetDocument(documentId);
                    if (document != null)
                    {
                        if (document.ApprovalWorkflowState.ToString() == "Published")
                        {
                            return document;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
            return null;
        }

        public Document GetDocumentByTitle(string title)
        {
            if (!String.IsNullOrEmpty(title))
            {
                var document = _librariesManager.GetDocuments().Where(d => d.Title == title).FirstOrDefault();
                if (document != null)
                {
                    document = _librariesManager.Lifecycle.GetLive(document) as Document;
                }
                return document;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Album
        public Album GetAlbum(string albumName)
        {
            var album = _librariesManager.GetAlbums().FirstOrDefault(a => a.Title == albumName);
            return album;
        }

        public IEnumerable<Album> GetAlbums()
        {
            return _librariesManager.GetAlbums();
        }
        #endregion

        #region Video
        public Video GetVideoFromId(Guid videoId)
        {
            try
            {
                if (videoId != Guid.Empty)
                {
                    var video = _librariesManager.GetVideo(videoId);
                    if (video != null)
                    {
                        if (video.ApprovalWorkflowState.ToString() == "Published" && video.Visible)
                        {
                            return video;
                        }
                        else // image is master and need to get live version
                        {
                            return _librariesManager.GetVideos().Where(i => i.OriginalContentId == videoId && i.Visible).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
            return null;
        }
        #endregion
    }

}