namespace ScBootstrap.Logic.Services
{
    using Models.Domain;

    public interface IGalleryService
    {
        GalleryList GetGalleryList(string pathInfo);
        Gallery GetGallery(string pathInfo);
    }
}
