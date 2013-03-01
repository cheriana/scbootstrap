namespace ScBootstrap.Logic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Extensions;
    using Models.Common;
    using Models.Domain;
    using Sitecore;
    using Sitecore.Data.Items;

    public class CommonService : ICommonService
    {
        public string GetStartUrl()
        {
            var home = Context.Database.GetItem(Context.Site.StartPath);
            return home.GetItemUrl();
        }

        public IList<Spotlight> GetSpotlights(string pathInfo)
        {
            var folder = Context.Database.GetItem(pathInfo);
            var items = folder.Children.Where(i => i.TemplateName.Equals("Spotlight")).ToList();
            return items.Count == 0 
                ? new List<Spotlight>().AsReadOnly() 
                : Mapper.Map<List<Item>, List<Spotlight>>(items).AsReadOnly();
        }

        public IList<TopNavigation> GetNavigation()
        {
            var home = Context.Database.GetItem(Context.Site.StartPath);
            var homeNavi = new TopNavigation
            {
                MenuTitle = home.Name,
                Url = home.GetItemUrl(),
                Children = new Navigation[0]
            };

            var naviItems = Mapper.Map<List<Item>, List<TopNavigation>>(home.Children.ToList());
            var list = new List<TopNavigation> { homeNavi };
            list.AddRange(naviItems);
            return list.AsReadOnly();
        }

        public IList<Navigation> GetBreadcrumb(string pathInfo)
        {
            var path = String.Format("{0}/{1}", Context.Site.StartPath, pathInfo);
            var item = Context.Database.GetItem(path);
            if (item == null) return new List<Navigation>().AsReadOnly();

            var home = Context.Database.GetItem(Context.Site.StartPath);
            var list = new List<Navigation>();
            Action<Item> getItems = null;
            getItems = i =>
            {
                list.Add(Mapper.Map<Item, Navigation>(i));
                if (!i.Paths.IsDescendantOf(home)) return;
                getItems(i.Parent);
            };
            
            getItems(item);
            list.Reverse();
            return list.AsReadOnly();
        }

        public IList<ContactLink> ContactLinks(string pathInfo)
        {
            var folder = Context.Database.GetItem(pathInfo);
            var items = folder.Children.Where(i => i.TemplateName.Equals("ContactLink")).ToList();
            return items.Count == 0 
                ? new List<ContactLink>().AsReadOnly() 
                : Mapper.Map<List<Item>, List<ContactLink>>(items).AsReadOnly();
        }
    }
}
