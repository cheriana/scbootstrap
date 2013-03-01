namespace ScBootstrap.Logic.Extensions
{
    using System.Web;
    using Sitecore;

    public static class StringExtensions
    {
        public static string HtmlEncode(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public static string UrlPathEncode(this string s)
        {
            return HttpUtility.UrlPathEncode(s);
        }

        public static string UrlDecode(this string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static string RemoveTags(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? string.Empty : StringUtil.RemoveTags(s);
        }

        public static bool ContainsUrl(this string s, string url)
        {
            if (string.IsNullOrWhiteSpace(s) && string.IsNullOrWhiteSpace(url)) return false;
            s = s.Trim('/');
            url = url.Trim('/');
            var values = s.Split('/');
            if (values.Length < 1) return false;
            var urls = url.Split('/');
            if (urls.Length < 1) return false;
            return values[0] == urls[0];
        }
    }
}
