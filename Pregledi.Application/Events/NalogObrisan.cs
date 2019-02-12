using System;

namespace Pregledi.Application.Events
{
	public class NalogObrisan : EventBase
	{
		public Guid IdNaloga { get; }

		public NalogObrisan(Guid idNaloga)
		{
			this.IdNaloga = idNaloga;
		}
	}
}
