using FluentValidation;
using Knjizenje.Application.Commands.DTO;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Seedwork;
using System;

namespace Knjizenje.Application.Commands.OtvoriNalog
{
	public class OtvoriNalogCommandValidator : AbstractValidator<OtvoriNalogCommand>
	{
		public OtvoriNalogCommandValidator()
		{
			RuleFor(x => x.DatumNaloga).GreaterThan(DateTime.MinValue).WithMessage("Datum naloga nije validan");
			RuleFor(x => x.IdTip).Must(x => TipNaloga.Get(x) != null).WithMessage("Nepoznat tip naloga");
			RuleFor(x => x.Stavke).NotEmpty().WithMessage("Stavke moraju biti definisane");
			RuleForEach(x => x.Stavke).SetValidator(new InlineValidator<StavkaDTO>()
			{
				v => v.RuleFor(x => x.IdKonto).GreaterThan(0).WithMessage("IdKonto nije validan"),
				v => v.RuleFor(x => x).Must(x => (x.Duguje != 0 && x.Potrazuje == 0) || (x.Duguje == 0 || x.Potrazuje != 0))
					.WithMessage("Dugovna ili potražna strana stavke mora biti definisana")
			});
		}
	}
}
