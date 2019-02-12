using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Seedwork;
using System.Collections.Generic;

namespace Knjizenje.Domain.Entities.FinNalogAggregate
{
	public class IznosStavke : ValueObject
	{
		public decimal Duguje { get; }
		public decimal Potrazuje { get; }

		public IznosStavke(decimal duguje, decimal potrazuje)
		{
			if (duguje == 0 && potrazuje == 0)
				throw new KnjizenjeException("Dugovna ili potražna strana stavke moraju biti različite od 0");
			if (duguje != 0 && potrazuje != 0)
				throw new KnjizenjeException("Samo dugovna ili potražna strana stavke mogu biti različite od 0");

			Duguje = duguje;
			Potrazuje = potrazuje;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Duguje;
			yield return Potrazuje;
		}
	}
}
