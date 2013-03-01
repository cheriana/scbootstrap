namespace ScBootstrap.Logic.Models.Domain
{
    using System;
    using Common;

    public class Gallery
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
        public Image[] Images { get; set; }
    }
}
