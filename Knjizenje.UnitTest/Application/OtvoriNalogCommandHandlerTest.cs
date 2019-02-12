using Knjizenje.Application.Commands.DTO;
using Knjizenje.Application.Commands.OtvoriNalog;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Seedwork;
using Knjizenje.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Application
{
	public class OtvoriNalogCommandHandlerTest
	{
		[Fact]
		public async Task VecPostoji_Greska()
		{
			var cmd = new OtvoriNalogCommand(Guid.NewGuid(), "1", TipNaloga.UlazneFakture.Id, new DateTime(2018, 10, 20), "opis", new List<StavkaDTO>());
			var nalogIzBazeId = new Mock<FinNalogId>();
			nalogIzBazeId.CallBase = true;
			nalogIzBazeId.SetupGet(x => x.Id).Returns(Guid.NewGuid());
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetPostojeciAsync(TipNaloga.Get(cmd.IdTip), cmd.DatumNaloga)).ReturnsAsync(nalogIzBazeId.Object);
			var fakeLogger = new Mock<ILogger<OtvoriNalogCommandHandler>>();
			var handler = new OtvoriNalogCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(cmd, default);

			await Assert.ThrowsAsync<KnjizenjeException>(handle);
		}

		[Fact]
		public async Task Otvori_Korektno()
		{
			var cmd = new OtvoriNalogCommand(Guid.NewGuid(), "1", TipNaloga.UlazneFakture.Id, new DateTime(2018, 10, 21), "opis new", new List<StavkaDTO>
			{
				new StavkaDTO(null, 1, 100, 0, "opis stavke 1", false),
			});
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var fakeNalogSvc = new Mock<IFinNalogService>();
			var fakeLogger = new Mock<ILogger<OtvoriNalogCommandHandler>>();
			var handler = new OtvoriNalogCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			fakeNalogRepo.Verify(x => x.SaveAsync(It.IsNotNull<FinNalog>(), cmd.CommandId, cmd.Version, cmd.UserId));
		}
	}
}
