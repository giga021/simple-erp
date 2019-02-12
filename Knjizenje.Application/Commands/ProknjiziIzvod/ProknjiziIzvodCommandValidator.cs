using FluentValidation;
using System;

namespace Knjizenje.Application.Commands.ProknjiziIzvod
{
	public class ProknjiziIzvodCommandValidator : AbstractValidator<ProknjiziIzvodCommand>
	{
		public ProknjiziIzvodCommandValidator()
		{
			RuleFor(x => x.Datum).GreaterThan(DateTime.MinValue).WithMessage("Datum naloga nije validan");
			RuleFor(x => x.Stavke).NotEmpty().WithMessage("Stavke moraju biti definisane");
			RuleForEach(x => x.Stavke).SetValidator(new InlineValidator<StavkaIzvodaDTO>()
			{
				v => v.RuleFor(x => x.SifraPlacanja).GreaterThan(0).WithMessage("Šifra plaćanja nije validna"),
				v => v.RuleFor(x => x).Must(x => (x.Duguje != 0 && x.Potrazuje == 0) || (x.Duguje == 0 || x.Potrazuje != 0))
					.WithMessage("Dugovna ili potražna strana stavke mora biti definisana")
			});
		}
	}
}
