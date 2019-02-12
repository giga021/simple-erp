using FluentValidation;
using System;

namespace Knjizenje.Application.Commands.UkloniStavku
{
	public class UkloniStavkuCommandValidator : AbstractValidator<UkloniStavkuCommand>
	{
		public UkloniStavkuCommandValidator()
		{
			RuleFor(x => x.IdNaloga).NotEqual(Guid.Empty).WithMessage("IdNaloga je obavezan podatak");
			RuleFor(x => x.IdStavke).NotEqual(Guid.Empty).WithMessage("IdStavke je obavezan podatak");
		}
	}
}
