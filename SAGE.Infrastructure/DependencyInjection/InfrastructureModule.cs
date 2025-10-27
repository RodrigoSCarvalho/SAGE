using Autofac;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using SAGE.Domain.ChangePlans;
using SAGE.Infrastructure.Persistence.Repositories;
using SAGE.Infrastructure.Search;
using SAGE.Infrastructure.Search.Repositories;


namespace SAGE.Infrastructure.DependencyInjection;
public class InfrastructureModule : Module
{
    private readonly IConfiguration _configuration;

    public InfrastructureModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var elasticsearchUrl = _configuration["ELASTICSEARCH_URL"] ?? "http://localhost:9200";

        builder.Register(c =>
        {
            var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUrl))
                .DefaultIndex("changeplans")
                .EnableDebugMode()
                .PrettyJson()
                .RequestTimeout(TimeSpan.FromSeconds(30));

            return new ElasticsearchClient(settings);
        }).AsSelf()
          .SingleInstance();

        builder.RegisterType<ElasticsearchInitializer>()
               .AsSelf()
               .SingleInstance();

        builder.RegisterType<ChangePlanRepository>()
               .As<IChangePlanRepository>()
               .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(ThisAssembly)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterType<ChangePlanSearchRepository>()
               .As<IChangePlanSearchRepository>()
               .InstancePerLifetimeScope();
    }
}

