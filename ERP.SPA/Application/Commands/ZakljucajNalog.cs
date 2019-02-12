using Integration.Contracts.Commands;
using System;

namespace ERP.SPA.Application.Commands
{
	public class ZakljucajNalog : IZakljucajNalog
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }

		public ZakljucajNalog(Guid commandId, long? version, string userId, Guid idNaloga)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.IdNaloga = idNaloga;
		}
	}
}
