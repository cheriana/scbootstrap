namespace ScBootstrap.Logic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Models.Common;
    using Models.Domain;
    using Sitecore;
    using Sitecore.Data.Items;

    public class TextpageService : ITextpageService
    {
        public Textpage GetTextpage(string pathInfo)
        {
            var path = String.Format("{0}/{1}", Context.Site.StartPath, pathInfo);
            var item = Context.Database.GetItem(path);
            return item == null ? new Textpage() : Mapper.Map<Item, Textpage>(item);
        }

        public IList<Navigation> GetNavigation(string pathInfo)
        {
            var path = String.Format("{0}/{1}", Context.Site.StartPath, pathInfo);
            var item = Context.Database.GetItem(path);
            if (item == null) return new List<Navigation>().AsReadOnly();

            var home = Context.Database.GetItem(Context.Site.StartPath);
            var list = item.Parent.ID == home.ID ? item.Children.ToList() : item.Parent.Children.ToList();
            return Mapper.Map<List<Item>, List<Navigation>>(list).AsReadOnly();
        }
    }
}
