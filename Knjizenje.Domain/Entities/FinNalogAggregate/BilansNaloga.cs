using Knjizenje.Domain.Seedwork;
using System.Collections.Generic;

namespace Knjizenje.Domain.Entities.FinNalogAggregate
{
	public class BilansNaloga : ValueObject
	{
		public decimal Duguje { get; }
		public decimal Potrazuje { get; }

		public BilansNaloga(decimal duguje, decimal potrazuje)
		{
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
