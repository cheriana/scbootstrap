namespace ScBootstrap.Logic.Services
{
    using System.Collections.Generic;
    using Models.Common;
    using Models.Domain;

    public interface ICommonService
    {
        string GetStartUrl();
        IList<Spotlight> GetSpotlights(string pathInfo);
        IList<TopNavigation> GetNavigation();
        IList<Navigation> GetBreadcrumb(string pathInfo);
        IList<ContactLink> ContactLinks(string pathInfo);
    }
}
