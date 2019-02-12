using Knjizenje.Application.Commands.StornirajStavku;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Application
{
	public class StornirajStavkuCommandHandlerTest
	{
		[Fact]
		public async Task NePostoji_Greska()
		{
			var cmd = new StornirajStavkuCommand(Guid.NewGuid(), 0, "1", Guid.NewGuid(), Guid.NewGuid());
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var fakeLogger = new Mock<ILogger<StornirajStavkuCommandHandler>>();
			var handler = new StornirajStavkuCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(cmd, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task Storniraj_Korektno()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			var cmd = new StornirajStavkuCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id, Guid.NewGuid());
			nalogIzBaze.SetupGet(x => x.Stavke).Returns(new List<FinStavka>
			{
				new FinStavka(cmd.IdStavke, 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeLogger = new Mock<ILogger<StornirajStavkuCommandHandler>>();
			var handler = new StornirajStavkuCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			nalogIzBaze.Verify(x => x.StornirajStavku(It.IsNotNull<FinStavka>()), Times.Once);
			nalogIzBaze.Verify(x => x.StornirajStavku(It.Is<FinStavka>(s => s.Id == cmd.IdStavke)));
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}
	}
}
