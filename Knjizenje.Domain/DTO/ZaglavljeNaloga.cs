using Knjizenje.Domain.Entities.FinNalogAggregate;
using System;

namespace Knjizenje.Domain.DTO
{
	public class ZaglavljeNaloga
	{
		public FinNalogId IdNaloga { get; set; }
		public long IdTip { get; set; }
		public DateTime DatumNaloga { get; set; }
		public string Opis { get; set; }
	}
}
