using Autofac;
using FluentValidation;
using System.Reflection;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace SAGE.Application.DependencyInjection;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var configuration = MediatRConfigurationBuilder.Create(string.Empty, assembly).WithAllOpenGenericHandlerTypesRegistered().Build();

        builder.RegisterMediatR(configuration);

        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();
    }
}