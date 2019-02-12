using System;

namespace Pregledi.Application.Events
{
	public class NalogZakljucan : EventBase
	{
		public Guid IdNaloga { get; }

		public NalogZakljucan(Guid idNaloga)
		{
			this.IdNaloga = idNaloga;
		}
	}
}
