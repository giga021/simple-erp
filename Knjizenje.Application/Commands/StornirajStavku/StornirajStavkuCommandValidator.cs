using FluentValidation;
using System;

namespace Knjizenje.Application.Commands.StornirajStavku
{
	public class StornirajStavkuCommandValidator : AbstractValidator<StornirajStavkuCommand>
	{
		public StornirajStavkuCommandValidator()
		{
			RuleFor(x => x.IdNaloga).NotEqual(Guid.Empty).WithMessage("IdNaloga je obavezan podatak");
			RuleFor(x => x.IdStavke).NotEqual(Guid.Empty).WithMessage("IdStavke je obavezan podatak");
		}
	}
}
