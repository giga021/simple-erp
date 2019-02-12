using FluentValidation;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Seedwork;
using System;

namespace Knjizenje.Application.Commands.IzmeniNalog
{
	public class IzmeniNalogCommandValidator : AbstractValidator<IzmeniNalogCommand>
	{
		public IzmeniNalogCommandValidator()
		{
			RuleFor(x => x.IdNaloga).NotEqual(Guid.Empty).WithMessage("IdNaloga je obavezan podatak");
			RuleFor(x => x.DatumNaloga).GreaterThan(DateTime.MinValue).WithMessage("Datum naloga nije validan");
			RuleFor(x => x.IdTip).Must(x => TipNaloga.Get(x) != null).WithMessage("Nepoznat tip naloga");
		}
	}
}
