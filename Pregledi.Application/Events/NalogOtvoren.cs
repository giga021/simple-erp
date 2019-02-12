using System;

namespace Pregledi.Application.Events
{
	public class NalogOtvoren : EventBase
	{
		public Guid IdNaloga { get; }
		public DateTime DatumNaloga { get; }
		public long IdTip { get; set; }
		public string Opis { get; set; }

		public NalogOtvoren(Guid idNaloga, DateTime datumNaloga, long idTip, string opis)
		{
			this.IdNaloga = idNaloga;
			this.DatumNaloga = datumNaloga;
			this.IdTip = idTip;
			this.Opis = opis;
		}
	}
}
