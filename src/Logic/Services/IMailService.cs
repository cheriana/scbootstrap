namespace ScBootstrap.Logic.Services
{
    using System.Collections.Specialized;

    public interface IMailService
    {
        bool SendMail(string from, string recipients, string subject, string htmlContent, string textContent);
        string GetHtmlFromTemplate(string path, ListDictionary replacements);
    }
}
