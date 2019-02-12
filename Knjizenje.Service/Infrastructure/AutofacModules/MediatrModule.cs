using Autofac;
using FluentValidation;
using Knjizenje.API.Application.Behaviors;
using Knjizenje.Application.Commands.ProknjiziIzvod;
using MediatR;
using System.Linq;
using System.Reflection;

namespace Knjizenje.API.Infrastructure.AutofacModules
{
	public class MediatrModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
				.AsImplementedInterfaces();

			builder.RegisterAssemblyTypes(typeof(ProknjiziIzvodCommand).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(IRequestHandler<,>));

			builder.RegisterAssemblyTypes(typeof(ProknjiziIzvodCommandHandler).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(INotificationHandler<>));

			builder.RegisterAssemblyTypes(typeof(ProknjiziIzvodCommandValidator).GetTypeInfo().Assembly)
				.Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
				.AsImplementedInterfaces();

			builder.Register<ServiceFactory>(ctx =>
			{
				var c = ctx.Resolve<IComponentContext>();
				return t => c.Resolve(t);
			});

			builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
		}
	}
}
