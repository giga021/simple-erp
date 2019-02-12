using Knjizenje.Application.Commands.DTO;
using Knjizenje.Application.Commands.IzmeniNalog;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Application
{
	public class IzmeniNalogCommandHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var cmd = new IzmeniNalogCommand(Guid.NewGuid(), 0, "1", Guid.NewGuid(), TipNaloga.Izvodi.Id,
				new DateTime(2018, 10, 20), "opis", new List<StavkaDTO>());
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var fakeNalogSvc = new Mock<IFinNalogService>();
			var fakeLogger = new Mock<ILogger<IzmeniNalogCommandHandler>>();
			var handler = new IzmeniNalogCommandHandler(fakeNalogRepo.Object, fakeNalogSvc.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(cmd, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task IzmeniZaglavlje_Korektno()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			nalogIzBaze.SetupGet(x => x.Tip).Returns(TipNaloga.Izvodi);
			nalogIzBaze.SetupGet(x => x.DatumNaloga).Returns(new DateTime(2018, 10, 20));
			nalogIzBaze.SetupGet(x => x.Opis).Returns("opis");
			nalogIzBaze.SetupGet(x => x.Stavke).Returns(new List<FinStavka>().AsReadOnly());
			var cmd = new IzmeniNalogCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id, TipNaloga.UlazneFakture.Id,
				new DateTime(2018, 10, 21), "opis new", new List<StavkaDTO>());
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeNalogSvc = new Mock<IFinNalogService>();
			var fakeLogger = new Mock<ILogger<IzmeniNalogCommandHandler>>();
			var handler = new IzmeniNalogCommandHandler(fakeNalogRepo.Object, fakeNalogSvc.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			fakeNalogSvc.Verify(x => x.IzmeniZaglavljeAsync(nalogIzBaze.Object, TipNaloga.Get(cmd.IdTip), cmd.DatumNaloga, cmd.Opis));
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}

		[Fact]
		public async Task ProknjiziStavku_Korektno()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			nalogIzBaze.SetupGet(x => x.Stavke).Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30))
			});
			var cmd = new IzmeniNalogCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id, TipNaloga.UlazneFakture.Id,
				new DateTime(2018, 10, 21), "opis new", new List<StavkaDTO>
				{
					new StavkaDTO(nalogIzBaze.Object.Stavke.ElementAt(0).Id, 1, 100, 0, "opis stavke 1", false),
					new StavkaDTO(null, 2, 200, 0, "opis stavke 2", false)
				});
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeNalogSvc = new Mock<IFinNalogService>();
			var fakeLogger = new Mock<ILogger<IzmeniNalogCommandHandler>>();
			var handler = new IzmeniNalogCommandHandler(fakeNalogRepo.Object, fakeNalogSvc.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			nalogIzBaze.Verify(x => x.ProknjiziStavku(It.IsNotNull<FinStavka>()), Times.Once);
			nalogIzBaze.Verify(x => x.ProknjiziStavku(It.Is<FinStavka>(s =>
				s.IdKonto == 2 && s.Iznos.Duguje == 200 && s.Iznos.Potrazuje == 0 && s.Opis == "opis stavke 2")));
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}

		[Fact]
		public async Task StornirajStavku_Korektno()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			nalogIzBaze.SetupGet(x => x.Stavke).Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var cmd = new IzmeniNalogCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id, TipNaloga.UlazneFakture.Id,
				new DateTime(2018, 10, 21), "opis new", new List<StavkaDTO>
				{
					new StavkaDTO(nalogIzBaze.Object.Stavke.ElementAt(0).Id, 1, 100, 0, "opis stavke 1", true),
					new StavkaDTO(nalogIzBaze.Object.Stavke.ElementAt(1).Id, 2, 200, 0, "opis stavke 2", false),
					new StavkaDTO(Guid.NewGuid(), 3, 300, 0, "opis stavke 3", true)
				});
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeNalogSvc = new Mock<IFinNalogService>();
			var fakeLogger = new Mock<ILogger<IzmeniNalogCommandHandler>>();
			var handler = new IzmeniNalogCommandHandler(fakeNalogRepo.Object, fakeNalogSvc.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			nalogIzBaze.Verify(x => x.StornirajStavku(It.IsNotNull<FinStavka>()), Times.Once);
			nalogIzBaze.Verify(x => x.StornirajStavku(It.Is<FinStavka>(s => s.Id == nalogIzBaze.Object.Stavke.ElementAt(0).Id)));
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}

		[Fact]
		public async Task UkloniStavku_Korektno()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			nalogIzBaze.SetupGet(x => x.Stavke).Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var cmd = new IzmeniNalogCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id, TipNaloga.UlazneFakture.Id,
				new DateTime(2018, 10, 21), "opis new", new List<StavkaDTO>
				{
					new StavkaDTO(nalogIzBaze.Object.Stavke.ElementAt(0).Id, 1, 100, 0, "opis stavke 1", false)
				});
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeNalogSvc = new Mock<IFinNalogService>();
			var fakeLogger = new Mock<ILogger<IzmeniNalogCommandHandler>>();
			var handler = new IzmeniNalogCommandHandler(fakeNalogRepo.Object, fakeNalogSvc.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			nalogIzBaze.Verify(x => x.UkloniStavku(It.IsNotNull<FinStavka>()), Times.Once);
			nalogIzBaze.Verify(x => x.UkloniStavku(It.Is<FinStavka>(s => s.Id == nalogIzBaze.Object.Stavke.ElementAt(1).Id)));
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}
	}
}
