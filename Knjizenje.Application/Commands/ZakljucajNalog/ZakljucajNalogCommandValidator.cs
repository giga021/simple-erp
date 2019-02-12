using FluentValidation;
using System;

namespace Knjizenje.Application.Commands.ZakljucajNalog
{
	public class ZakljucajNalogCommandValidator : AbstractValidator<ZakljucajNalogCommand>
	{
		public ZakljucajNalogCommandValidator()
		{
			RuleFor(x => x.IdNaloga).NotEqual(Guid.Empty).WithMessage("IdNaloga je obavezan podatak");
		}
	}
}
