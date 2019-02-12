using Knjizenje.Application.Commands.ProknjiziStavku;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Application
{
	public class ProknjiziStavkuCommandHandlerTest
	{
		[Fact]
		public async Task NePostoji_Greska()
		{
			var cmd = new ProknjiziStavkuCommand(Guid.NewGuid(), 0, "1", Guid.NewGuid(), 1, 100, 0, "opis stavke 1");
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var fakeLogger = new Mock<ILogger<ProknjiziStavkuCommandHandler>>();
			var handler = new ProknjiziStavkuCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(cmd, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task Proknjizi_Korektno()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			var cmd = new ProknjiziStavkuCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id, 1, 100, 0, "opis stavke 1");
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeLogger = new Mock<ILogger<ProknjiziStavkuCommandHandler>>();
			var handler = new ProknjiziStavkuCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			nalogIzBaze.Verify(x => x.ProknjiziStavku(It.Is<FinStavka>(s =>
				s.IdKonto == 1 && s.Iznos.Duguje == 100 && s.Iznos.Potrazuje == 0 && s.Opis == "opis stavke 1")));
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}
	}
}
