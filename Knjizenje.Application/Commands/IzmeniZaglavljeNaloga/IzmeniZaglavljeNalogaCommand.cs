using System;

namespace Knjizenje.Application.Commands.IzmeniZaglavljeNaloga
{
	public class IzmeniZaglavljeNalogaCommand : ICommand
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }
		public long IdTip { get; }
		public DateTime DatumNaloga { get; }
		public string Opis { get; }

		public IzmeniZaglavljeNalogaCommand(Guid commandId, long? version, string userId, Guid idNaloga,
			long idTip, DateTime datumNaloga, string opis)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.IdNaloga = idNaloga;
			this.IdTip = idTip;
			this.DatumNaloga = datumNaloga;
			this.Opis = opis;
		}
	}
}
