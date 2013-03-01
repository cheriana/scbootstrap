namespace ScBootstrap.Logic.Models.Domain
{
    using System;

    public class GalleryList
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
        public Gallery[] Galleries { get; set; }
    }
}
