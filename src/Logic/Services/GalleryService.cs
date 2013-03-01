namespace ScBootstrap.Logic.Services
{
    using System;
    using AutoMapper;
    using Models.Domain;
    using Sitecore;
    using Sitecore.Data.Items;

    public class GalleryService : IGalleryService
    {
        public GalleryList GetGalleryList(string pathInfo)
        {
            var path = String.Format("{0}/{1}", Context.Site.StartPath, pathInfo);
            var item = Context.Database.GetItem(path);
            return item == null ? new GalleryList() : Mapper.Map<Item, GalleryList>(item);
        }

        public Gallery GetGallery(string pathInfo)
        {
            var path = String.Format("{0}/{1}", Context.Site.StartPath, pathInfo);
            var item = Context.Database.GetItem(path);
            return item == null ? new Gallery() : Mapper.Map<Item, Gallery>(item);
        }
    }
}
