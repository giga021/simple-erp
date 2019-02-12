using Autofac;
using MediatR;
using Pregledi.Application.EventHandlers.GlavnaKnjigaHandlers;
using System.Reflection;

namespace Pregledi.API.Infrastructure.AutofacModules
{
	public class MediatrModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
				.AsImplementedInterfaces().InstancePerLifetimeScope();

			builder.RegisterAssemblyTypes(typeof(NalogOtvorenHandler).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(INotificationHandler<>)).InstancePerLifetimeScope();

			builder.Register<ServiceFactory>(ctx =>
			{
				var c = ctx.Resolve<IComponentContext>();
				return t => c.Resolve(t);
			});
		}
	}
}
