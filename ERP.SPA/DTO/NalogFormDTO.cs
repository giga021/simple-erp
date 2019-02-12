using System;
using System.Collections.Generic;

namespace ERP.SPA.DTO
{
	public class NalogFormDTO
	{
		public Guid? Id { get; set; }
		public long? Version { get; set; }
		public long IdTip { get; set; }
		public DateTime Datum { get; set; }
		public string Opis { get; set; }
		public List<StavkaFormDTO> Stavke { get; set; }
	}
}
