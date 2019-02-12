using Knjizenje.Application.Commands.IzmeniZaglavljeNaloga;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Application
{
	public class IzmeniZaglavljeNalogaCommandHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var cmd = new IzmeniZaglavljeNalogaCommand(Guid.NewGuid(), 0, "1", Guid.NewGuid(), TipNaloga.Izvodi.Id,
				new DateTime(2018, 10, 20), "opis");
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var fakeNalogSvc = new Mock<IFinNalogService>();
			var fakeLogger = new Mock<ILogger<IzmeniZaglavljeNalogaCommandHandler>>();
			var handler = new IzmeniZaglavljeNalogaCommandHandler(fakeNalogRepo.Object, fakeNalogSvc.Object, fakeLogger.Object);

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
			var cmd = new IzmeniZaglavljeNalogaCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id, TipNaloga.UlazneFakture.Id, new DateTime(2018, 10, 21), "opis new");
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeNalogSvc = new Mock<IFinNalogService>();
			var fakeLogger = new Mock<ILogger<IzmeniZaglavljeNalogaCommandHandler>>();
			var handler = new IzmeniZaglavljeNalogaCommandHandler(fakeNalogRepo.Object, fakeNalogSvc.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			fakeNalogSvc.Verify(x => x.IzmeniZaglavljeAsync(nalogIzBaze.Object, TipNaloga.Get(cmd.IdTip), cmd.DatumNaloga, cmd.Opis));
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}
	}
}
