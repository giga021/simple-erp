using Knjizenje.Application.Commands.ObrisiNalog;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Application
{
	public class ObrisiNalogCommandHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var cmd = new ObrisiNalogCommand(Guid.NewGuid(), 0, "1", Guid.NewGuid());
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var fakeLogger = new Mock<ILogger<ObrisiNalogCommandHandler>>();
			var handler = new ObrisiNalogCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(cmd, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task Obrisi_Korektno()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			var cmd = new ObrisiNalogCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id);
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeLogger = new Mock<ILogger<ObrisiNalogCommandHandler>>();
			var handler = new ObrisiNalogCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			nalogIzBaze.Verify(x => x.Obrisi());
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}
	}
}
