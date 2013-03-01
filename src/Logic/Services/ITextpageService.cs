namespace ScBootstrap.Logic.Services
{
    using System.Collections.Generic;
    using Models.Common;
    using Models.Domain;    

    public interface ITextpageService
    {
        Textpage GetTextpage(string pathInfo);
        IList<Navigation> GetNavigation(string pathInfo);
    }
}
