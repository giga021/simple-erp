using Pregledi.Domain.Seedwork;
using System;
using System.Collections.Generic;

namespace Pregledi.Domain.Entities
{
	public class NalogForm : Entity<Guid>
	{
		public DateTime Datum { get; set; }
		public long IdTip { get; set; }
		public string Opis { get; set; }
		public long Version { get; set; }

		public IList<StavkaForm> Stavke { get; set; }
	}
}
