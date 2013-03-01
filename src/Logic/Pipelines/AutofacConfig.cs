namespace ScBootstrap.Logic.Pipelines
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Controllers;
    using Sitecore.Pipelines;
    using Services;

    public class AutofacConfig
    {
        public void Process(PipelineArgs args)
        {
            // Enable dependency injection into controllers is to set the 
            // DependencyResolver in the application startup method
            var builder = new ContainerBuilder();

            // register all controllers found in this assembly
            builder.RegisterControllers(typeof(HomeController).Assembly);

            // add custom class to the container as Transient instance
            builder.RegisterType<CommonService>().As<ICommonService>();
            builder.RegisterType<TextpageService>().As<ITextpageService>();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<MailService>().As<IMailService>();
            builder.RegisterType<GalleryService>().As<IGalleryService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
