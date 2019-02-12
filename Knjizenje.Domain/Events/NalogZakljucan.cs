using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knjizenje.Domain.Events
{
	public class NalogZakljucan : EventBase
	{
		public FinNalogId IdNaloga { get; }

		public NalogZakljucan(FinNalogId idNaloga)
		{
			this.IdNaloga = idNaloga;
		}
	}
}
