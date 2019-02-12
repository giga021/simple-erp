using Microsoft.Extensions.Logging;
using Moq;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Application.EventHandlers.StavkaFormHandlers;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.EventHandlers.StavkaFormHandlers
{
	public class StavkaUklonjenaHandlerTest
	{
		[Fact]
		public async Task StavkaNePostoji_Greska()
		{
			var fakeRepo = new Mock<IStavkaFormRepository>();
			var fakeLogger = new Mock<ILogger<StavkaUklonjenaHandler>>();
			var evnt = new StavkaUklonjena(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 10, 22), 1, 100, 0, "opis stavke");
			var handler = new StavkaUklonjenaHandler(fakeRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<PreglediException>(handle);
		}

		[Fact]
		public async Task Handle_Korektno()
		{
			var stavkaIzBaze = new StavkaForm() { Id = Guid.NewGuid() };
			var fakeRepo = new Mock<IStavkaFormRepository>();
			fakeRepo.Setup(x => x.GetAsync(stavkaIzBaze.Id)).ReturnsAsync(stavkaIzBaze);
			var fakeLogger = new Mock<ILogger<StavkaUklonjenaHandler>>();
			var evnt = new StavkaUklonjena(Guid.NewGuid(), stavkaIzBaze.Id, new DateTime(2018, 10, 22), 1, 100, 0, "opis stavke")
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new StavkaUklonjenaHandler(fakeRepo.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			fakeRepo.Verify(x => x.Remove(stavkaIzBaze));
		}
	}
}
