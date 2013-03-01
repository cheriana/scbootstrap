namespace ScBootstrap.Logic.Models.Domain
{
    using System;
    using Common;

    public class Spotlight
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Link Link { get; set; }
        public Image Image { get; set; }
    }
}