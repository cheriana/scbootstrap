namespace ScBootstrap.Logic.Extensions
{
    using Sitecore.Data.Items;
    using Sitecore.Links;
    using Sitecore.Resources.Media;

    internal static class ItemExtensions
    {
        public static string GetItemUrl(this Item item)
        {
            return LinkManager.GetItemUrl(item);
        }

        public static string GetMediaUrl(this Item item)
        {
            return MediaManager.GetMediaUrl(item);
        }
    }
}
