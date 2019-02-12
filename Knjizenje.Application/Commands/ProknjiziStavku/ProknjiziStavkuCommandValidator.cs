using FluentValidation;
using System;

namespace Knjizenje.Application.Commands.ProknjiziStavku
{
	public class ProknjiziStavkuCommandValidator : AbstractValidator<ProknjiziStavkuCommand>
	{
		public ProknjiziStavkuCommandValidator()
		{
			RuleFor(x => x.IdNaloga).NotEqual(Guid.Empty).WithMessage("IdNaloga je obavezan podatak");
			RuleFor(x => x.IdKonto).GreaterThan(0).WithMessage("IdKonto nije validan");
			RuleFor(x => x).Must(x => (x.Duguje != 0 && x.Potrazuje == 0) || (x.Duguje == 0 || x.Potrazuje != 0))
				.WithMessage("Dugovna ili potražna strana stavke mora biti definisana");
		}
	}
}
