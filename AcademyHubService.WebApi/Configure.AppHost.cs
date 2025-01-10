using Funq;
using ServiceStack.DataAnnotations;
using ServiceStack.Validation;
using AcademyHubService.ReadModel;
using AcademyHubService.ReadModel.Queries.RavenDB;
using AcademyHubService.WebApi.Impl;
using AcademyHubService.WebApi.ServiceInterface;

[assembly: HostingStartup(typeof(AcademyHubService.WebApi.AppHost))]

namespace AcademyHubService.WebApi;

public class AppHost : AppHostBase, IHostingStartup
{
    public void Configure(IWebHostBuilder builder) =>
        builder.ConfigureAppConfiguration(
                b => { b.AddJsonFile("config/appsettings.json"); })
            .ConfigureServices((ctx, services) =>
            {
                //Licensing.RegisterLicense(ctx.Configuration["ServiceStack:Licence"]);
                var store = new RavenDocumentStoreFactory().CreateAndInitializeDocumentStore(
                    RavenConfig.FromConfiguration(ctx
                        .Configuration)); // leave it here to avoid lazy loading until this is refactored so that this comment is NOT NEEDED
                services.AddSingleton(store);
                services.AddTransient<ITimeProvider, StarnetTimeProvider>();
                services.AddTransient<ITypeaheadQueries, TypeaheadQueries>();
                services.AddTransient<IMessageBus, NSBus>();
                services.AddTransient<IOrganizationQueries, OrganizationQueries>();
                services.AddTransient<IQueryById, QueryById>();

                // Configure ASP.NET Core IOC Dependencies
            })
            .Configure(app =>
            {
                // Configure ASP.NET Core App
                if (!HasInit)
                    app.UseServiceStack(new AppHost());
            });

    public AppHost() : base("AcademyHubService.WebApi", typeof(HelloServices).Assembly)
    {
        typeof(Authenticate).AddAttributes(new ExcludeAttribute(Feature.Metadata));
        ServiceStack.Text.JsConfig.TreatEnumAsInteger = true;
        ServiceStack.Text.JsConfig.AssumeUtc = true;
        ServiceStack.Text.JsConfig.AlwaysUseUtc = true;
        ServiceStack.Text.JsConfig.DateHandler = ServiceStack.Text.DateHandler.ISO8601;
    }

    public override void Configure(Container container)
    {
        // Configure ServiceStack only IOC, Config & Plugins
        SetConfig(new HostConfig
        {
            DefaultRedirectPath = "/metadata",
            UseSameSiteCookies = true,
            UseSecureCookies = true
        });
        ConfigureCORS();
        Plugins.Add(new ValidationFeature());
    }

    void ConfigureCORS()
    {
        var str = AppSettings.GetString("CORS:Whitelist");
        if (!string.IsNullOrWhiteSpace(str))
            if (str == "*")
                Plugins.Add(new CorsFeature());
            else
                Plugins.Add(new CorsFeature(allowCredentials: true, allowedHeaders: "Content-Type, Authorization",
                    allowOriginWhitelist: str.Split(';')));
    }
}