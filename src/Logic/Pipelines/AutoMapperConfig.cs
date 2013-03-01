namespace ScBootstrap.Logic.Pipelines
{
    using AutoMapper;
    using Extensions;
    using Models.Common;
    using Models.Domain;
    using Sitecore.Data.Items;
    using Sitecore.Pipelines;
    using System.Linq;

    public class AutoMapperConfig
    {
        public void Process(PipelineArgs args)
        {
            // Configure Spotlight mapping
            Mapper.CreateMap<Item, Spotlight>()
            .ForMember(s => s.Id, i => i.MapFrom(src => src.ID.ToGuid()))
            .ForMember(s => s.Title, i => i.MapFrom(src => src.Fields["Title"].GetText()))
            .ForMember(s => s.Description, i => i.MapFrom(src => src.Fields["Description"].GetText()))
            .ForMember(s => s.Link, i => i.MapFrom(src => src.Fields["Link"].GetLink()))
            .ForMember(s => s.Image, i => i.MapFrom(src => src.Fields["Image"].GetImage()));

            // Configure Textpage mapping
            Mapper.CreateMap<Item, Textpage>()
            .ForMember(t => t.Id, i => i.MapFrom(src => src.ID.ToGuid()))
            .ForMember(t => t.Title, i => i.MapFrom(src => src.Fields["Title"].GetText()))
            .ForMember(t => t.Teaser, i => i.MapFrom(src => src.Fields["Teaser"].GetText()))
            .ForMember(t => t.Content, i => i.MapFrom(src => src.Fields["Content"].GetText()));

            // Configure Navigation mapping
            Mapper.CreateMap<Item, Navigation>()
            .ForMember(n => n.MenuTitle, i => i.MapFrom(src => src["MenuTitle"].IsNullOrWhiteSpace() ? src.Name : src.Fields["MenuTitle"].GetText()))
            .ForMember(n => n.Url, i => i.MapFrom(src => src.GetItemUrl()))
            .ForMember(n => n.HideNavi, i => i.MapFrom(src => src.Fields["HideNavi"].GetBool()));

            Mapper.CreateMap<Item, TopNavigation>()
            .ForMember(n => n.MenuTitle, i => i.MapFrom(src => src["MenuTitle"].IsNullOrWhiteSpace() ? src.Name : src.Fields["MenuTitle"].GetText()))
            .ForMember(n => n.Url, i => i.MapFrom(src => src.GetItemUrl()))
            .ForMember(n => n.HideNavi, i => i.MapFrom(src => src.Fields["HideNavi"].GetBool()))
            .ForMember(n => n.Children,
                i => i.MapFrom(src => src.Children.Any()
                ? Mapper.Map<Item[], Navigation[]>(src.Children.ToArray())
                : new Navigation[0]));

            // Configure contact links
            Mapper.CreateMap<Item, ContactLink>()
            .ForMember(t => t.Id, i => i.MapFrom(src => src.ID.ToGuid()))
            .ForMember(t => t.Title, i => i.MapFrom(src => src.Fields["Title"].GetText()))
            .ForMember(t => t.Description, i => i.MapFrom(src => src.Fields["Description"].GetText()))
            .ForMember(t => t.Icon, i => i.MapFrom(src => src.Fields["Icon"].GetText()));

            // Configure Galleries
            Mapper.CreateMap<Item, Gallery>()
            .ForMember(t => t.Id, i => i.MapFrom(src => src.ID.ToGuid()))
            .ForMember(n => n.Url, i => i.MapFrom(src => src.GetItemUrl()))
            .ForMember(t => t.Title, i => i.MapFrom(src => src.Fields["Title"].GetText()))
            .ForMember(t => t.Teaser, i => i.MapFrom(src => src.Fields["Teaser"].GetText()))
            .ForMember(n => n.Images, i => i.MapFrom(src => src.Fields["Images"].GetTreelistImages()));

            Mapper.CreateMap<Item, GalleryList>()
            .ForMember(t => t.Id, i => i.MapFrom(src => src.ID.ToGuid()))
            .ForMember(t => t.Title, i => i.MapFrom(src => src.Fields["Title"].GetText()))
            .ForMember(t => t.Teaser, i => i.MapFrom(src => src.Fields["Teaser"].GetText()))
            .ForMember(t => t.Galleries,
                i => i.MapFrom(src => src.Children.Any()
                ? Mapper.Map<Item[], Gallery[]>(src.Children.ToArray())
                : new Gallery[0]));

            // Ensure mappings are valid
            Mapper.AssertConfigurationIsValid();
        }
    }
}
