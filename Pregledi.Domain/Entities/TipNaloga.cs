using Pregledi.Domain.Seedwork;

namespace Pregledi.Domain.Entities
{
	public class TipNaloga : Entity<long>
	{
		public string Naziv { get; set; }
	}
}
