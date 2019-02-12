using Knjizenje.Application.Commands.OtkljucajNalog;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Application
{
	public class OtkljucajNalogCommandHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var cmd = new OtkljucajNalogCommand(Guid.NewGuid(), 0, "1", Guid.NewGuid());
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var fakeLogger = new Mock<ILogger<OtkljucajNalogCommandHandler>>();
			var handler = new OtkljucajNalogCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(cmd, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task Otkljucaj_Korektno()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			var cmd = new OtkljucajNalogCommand(Guid.NewGuid(), 0, "1", nalogIzBaze.Object.Id.Id);
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetAsync(nalogIzBaze.Object.Id)).ReturnsAsync(nalogIzBaze.Object);
			var fakeLogger = new Mock<ILogger<OtkljucajNalogCommandHandler>>();
			var handler = new OtkljucajNalogCommandHandler(fakeNalogRepo.Object, fakeLogger.Object);

			await handler.Handle(cmd, default);

			nalogIzBaze.Verify(x => x.Otkljucaj());
			fakeNalogRepo.Verify(x => x.SaveAsync(nalogIzBaze.Object, cmd.CommandId, cmd.Version, cmd.UserId));
		}
	}
}
