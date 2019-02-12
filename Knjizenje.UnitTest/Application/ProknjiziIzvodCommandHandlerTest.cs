using Knjizenje.Application.Commands.ProknjiziIzvod;
using Knjizenje.Domain.DTO;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Application
{
	public class ProknjiziIzvodCommandHandlerTest
	{
		[Fact]
		public async Task NePostoji_OtvaraNovi()
		{
			var cmd = new ProknjiziIzvodCommand(Guid.NewGuid(), null, "1", new DateTime(2018, 10, 20), new List<StavkaIzvodaDTO>());
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetPostojeciAsync(TipNaloga.Izvodi, cmd.Datum)).ReturnsAsync(null as FinNalogId);
			var fakeLogger = new Mock<ILogger<ProknjiziIzvodCommandHandler>>();
			var fakeIzvodSvc = new Mock<IIzvodService>();
			fakeIzvodSvc.Setup(x => x.FormirajStavkeNalogaAsync(It.IsAny<IEnumerable<StavkaIzvoda>>())).ReturnsAsync(new List<FinStavka>
			{
				new FinStavka(1, 100, 0, "opis stavke 1")
			});
			var handler = new ProknjiziIzvodCommandHandler(fakeNalogRepo.Object, fakeIzvodSvc.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			fakeNalogRepo.Verify(x => x.SaveAsync(It.IsNotNull<FinNalog>(), cmd.CommandId, cmd.Version, cmd.UserId));
		}

		[Fact]
		public async Task VecPostoji_DodajeStavke()
		{
			var cmd = new ProknjiziIzvodCommand(Guid.NewGuid(), null, "1", new DateTime(2018, 10, 20), new List<StavkaIzvodaDTO>());
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			nalogIzBaze.SetupGet(x => x.Version).Returns(17);
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetPostojeciAsync(TipNaloga.Izvodi, cmd.Datum)).ReturnsAsync(nalogIzBaze.Object.Id);
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeLogger = new Mock<ILogger<ProknjiziIzvodCommandHandler>>();
			var fakeIzvodSvc = new Mock<IIzvodService>();
			fakeIzvodSvc.Setup(x => x.FormirajStavkeNalogaAsync(It.IsAny<IEnumerable<StavkaIzvoda>>())).ReturnsAsync(new List<FinStavka>
			{
				new FinStavka(1, 100, 0, "opis stavke 1"),
				new FinStavka(2, 200, 0, "opis stavke 2")
			});
			var handler = new ProknjiziIzvodCommandHandler(fakeNalogRepo.Object, fakeIzvodSvc.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			nalogIzBaze.Verify(x => x.ProknjiziStavku(It.IsNotNull<FinStavka>()), Times.Exactly(2));
			nalogIzBaze.Verify(x => x.ProknjiziStavku(It.Is<FinStavka>(s =>
				s.IdKonto == 1 && s.Iznos.Duguje == 100 && s.Iznos.Potrazuje == 0 && s.Opis == "opis stavke 1")));
			nalogIzBaze.Verify(x => x.ProknjiziStavku(It.Is<FinStavka>(s =>
				s.IdKonto == 2 && s.Iznos.Duguje == 200 && s.Iznos.Potrazuje == 0 && s.Opis == "opis stavke 2")));
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, nalogIzBaze.Object.Version, cmd.UserId));
		}
	}
}
