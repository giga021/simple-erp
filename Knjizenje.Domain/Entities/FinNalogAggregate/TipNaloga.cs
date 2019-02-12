using Knjizenje.Domain.Seedwork;

namespace Knjizenje.Domain.Entities.FinNalogAggregate
{
	public class TipNaloga : Enumeration<TipNaloga>
	{
		public static readonly TipNaloga UlazneFakture = new TipNaloga(1, "Ulazne fakture");
		public static readonly TipNaloga Izvodi = new TipNaloga(2, "Izvodi");

		public TipNaloga(long id, string name) : base(id, name) { }
	}
}
