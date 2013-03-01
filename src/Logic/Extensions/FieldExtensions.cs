namespace ScBootstrap.Logic.Extensions
{
    using System;
    using System.Linq;
    using Models.Common;
    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Links;

    internal static class FieldExtensions
    {
        public static Image GetImage(this Field f)
        {
            var emptyImage = new Image { Src = string.Empty, Alt = string.Empty, Width = string.Empty, Height = string.Empty };
            if (f == null) return emptyImage;
            var imageField = new ImageField(f);
            if (imageField.MediaItem == null) return emptyImage;
            return new Image
            {
                Src = imageField.MediaItem.GetMediaUrl().UrlPathEncode(),
                Alt = imageField.Alt,
                Width = imageField.Width,
                Height = imageField.Height
            };
        }

        public static string GetText(this Field f)
        {
            if (f == null) return string.Empty;
            var type = f.Type;
            if (type.Equals("Single-Line Text") || type.Equals("Multi-Line Text")) return f.Value.HtmlEncode();
            if (type.Equals("Rich Text")) return LinkManager.ExpandDynamicLinks(f.Value);
            return string.Empty;
        }

        public static Link GetLink(this Field f)
        {
            var emptyLink = new Link { Url = string.Empty, Text = string.Empty, Title = string.Empty, Target = string.Empty };
            if (f == null) return emptyLink;
            if (!f.Type.Equals("General Link")) return emptyLink;
            var field = (LinkField) f;
            if (field.LinkType.Equals("external"))
                return new Link { Url = field.Url, Text = field.Text, Title = field.Title, Target = field.Target };
            if (field.TargetID.ToGuid().Equals(Guid.Empty)) 
                return emptyLink;
            if (field.IsInternal)
                return new Link { Url = field.TargetItem.GetItemUrl(), Text = field.Text, Title = field.Title, Target = field.Target }; 
            if (field.IsMediaLink)
                return new Link { Url = field.TargetItem.GetMediaUrl().UrlPathEncode(), Text = field.Text, Title = field.Title, Target = field.Target };
            return emptyLink;
        }

        public static bool GetBool(this Field f)
        {
            if (f == null) return false;
            return f.Type.Equals("Checkbox") && f.Value.Equals("1");
        }

        public static Image[] GetTreelistImages(this Field f)
        {
            if (f == null) return new Image[0];
            if (!f.Type.Equals("Treelist")) return new Image[0];
            if (f.Value.IsNullOrWhiteSpace()) return new Image[0];
            try
            {
                return ID.ParseArray(f.Value)
                    .Select(Context.Database.Items.GetItem)
                    .Where(item => item != null)
                    .Where(item => item.Paths.IsMediaItem)
                    .Where(item => item.TemplateName.Contains("Jpeg"))
                    .Select(item => new Image
                    {
                        Src = item.GetMediaUrl().UrlPathEncode(),
                        Alt = item["Alt"],
                        Width = item["Width"],
                        Height = item["Height"]
                    })
                    .ToArray();
            }
            catch
            {
                return new Image[0];
            }
        }
    }
}
