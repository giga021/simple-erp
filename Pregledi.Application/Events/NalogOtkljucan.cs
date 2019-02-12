using System;

namespace Pregledi.Application.Events
{
	public class NalogOtkljucan : EventBase
	{
		public Guid IdNaloga { get; }

		public NalogOtkljucan(Guid idNaloga)
		{
			this.IdNaloga = idNaloga;
		}
	}
}
