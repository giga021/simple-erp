using FluentValidation;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Seedwork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.IzmeniZaglavljeNaloga
{
    public class IzmeniZaglavljeNalogaCommandValidator : AbstractValidator<IzmeniZaglavljeNalogaCommand>
	{
		public IzmeniZaglavljeNalogaCommandValidator()
		{
			RuleFor(x => x.IdNaloga).NotEqual(Guid.Empty).WithMessage("IdNaloga je obavezan podatak");
			RuleFor(x => x.DatumNaloga).GreaterThan(DateTime.MinValue).WithMessage("Datum naloga nije validan");
			RuleFor(x => x.IdTip).Must(x => TipNaloga.Get(x) != null).WithMessage("Nepoznat tip naloga");
		}
	}
}
