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
	public class IzmenjenoZaglavljeNalogaHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var fakeRepo = new Mock<INalogFormRepository>();
			var fakeLogger = new Mock<ILogger<IzmenjenoZaglavljeNalogaHandler>>();
			var evnt = new IzmenjenoZaglavljeNaloga(Guid.NewGuid(), new DateTime(2018, 10, 21), 1, "opis");
			var handler = new IzmenjenoZaglavljeNalogaHandler(fakeRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task Handle_Korektno()
		{
			var nalogIzBaze = new NalogForm()
			{
				Id = Guid.NewGuid(),
				Datum = new DateTime(2018, 10, 20),
				IdTip = 1,
				Opis = "opis"
			};
			var fakeRepo = new Mock<INalogFormRepository>();
			fakeRepo.Setup(x => x.GetAsync(nalogIzBaze.Id)).ReturnsAsync(nalogIzBaze);
			var fakeLogger = new Mock<ILogger<IzmenjenoZaglavljeNalogaHandler>>();
			var evnt = new IzmenjenoZaglavljeNaloga(nalogIzBaze.Id, new DateTime(2018, 10, 21), 2, "opis novi")
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new IzmenjenoZaglavljeNalogaHandler(fakeRepo.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			Assert.Equal(new DateTime(2018, 10, 21), nalogIzBaze.Datum);
			Assert.Equal(2, nalogIzBaze.IdTip);
			Assert.Equal("opis novi", nalogIzBaze.Opis);
		}
	}
}
