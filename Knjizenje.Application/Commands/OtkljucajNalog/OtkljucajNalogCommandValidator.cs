using FluentValidation;
using System;

namespace Knjizenje.Application.Commands.OtkljucajNalog
{
	public class OtkljucajNalogCommandValidator : AbstractValidator<OtkljucajNalogCommand>
	{
		public OtkljucajNalogCommandValidator()
		{
			RuleFor(x => x.IdNaloga).NotEqual(Guid.Empty).WithMessage("IdNaloga je obavezan podatak");
		}
	}
}
