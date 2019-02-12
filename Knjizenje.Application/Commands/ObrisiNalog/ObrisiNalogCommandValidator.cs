using FluentValidation;
using System;

namespace Knjizenje.Application.Commands.ObrisiNalog
{
	public class ObrisiNalogCommandValidator : AbstractValidator<ObrisiNalogCommand>
	{
		public ObrisiNalogCommandValidator()
		{
			RuleFor(x => x.IdNaloga).NotEqual(Guid.Empty).WithMessage("IdNaloga je obavezan podatak");
		}
	}
}
