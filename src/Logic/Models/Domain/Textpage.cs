namespace ScBootstrap.Logic.Models.Domain
{
    using System;

    public class Textpage
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
        public string Content { get; set; }
    }
}