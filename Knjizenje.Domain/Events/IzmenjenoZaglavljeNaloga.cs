using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knjizenje.Domain.Events
{
    public class IzmenjenoZaglavljeNaloga : EventBase
    {
		public FinNalogId IdNaloga { get; }
		public DateTime DatumNaloga { get; }
		public long IdTip { get; set; }
		public string Opis { get; set; }

		public IzmenjenoZaglavljeNaloga(FinNalogId idNaloga, DateTime datumNaloga, long idTip, string opis)
		{
			this.IdNaloga = idNaloga;
			this.DatumNaloga = datumNaloga;
			this.IdTip = idTip;
			this.Opis = opis;
		}
	}
}
