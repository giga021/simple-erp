using Microsoft.Extensions.Logging;
using Moq;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Application.EventHandlers.NalogFormHandlers;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.EventHandlers.NalogFormHandlers
{
	public class NalogObrisanHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var fakeRepo = new Mock<INalogFormRepository>();
			var fakeLogger = new Mock<ILogger<NalogObrisanHandler>>();
			var evnt = new NalogObrisan(Guid.NewGuid());
			var handler = new NalogObrisanHandler(fakeRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task Handle_Korektno()
		{
			var nalogIzBaze = new NalogForm()
			{
				Id = Guid.NewGuid(),
			};
			var fakeRepo = new Mock<INalogFormRepository>();
			fakeRepo.Setup(x => x.GetAsync(nalogIzBaze.Id)).ReturnsAsync(nalogIzBaze);
			var fakeLogger = new Mock<ILogger<NalogObrisanHandler>>();
			var evnt = new NalogObrisan(nalogIzBaze.Id)
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new NalogObrisanHandler(fakeRepo.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			fakeRepo.Verify(x => x.Remove(nalogIzBaze));
		}
	}
}
