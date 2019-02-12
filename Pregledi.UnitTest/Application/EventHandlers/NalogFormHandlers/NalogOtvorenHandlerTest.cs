using Microsoft.Extensions.Logging;
using Moq;
using Pregledi.Application.Events;
using Pregledi.Application.EventHandlers.NalogFormHandlers;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.EventHandlers.NalogFormHandlers
{
	public class NalogOtvorenHandlerTest
	{
		[Fact]
		public async Task Handle_Korektno()
		{
			var fakeRepo = new Mock<INalogFormRepository>();
			var fakeLogger = new Mock<ILogger<NalogOtvorenHandler>>();
			var evnt = new NalogOtvoren(Guid.NewGuid(), new DateTime(2018, 10, 21), 2, "opis novi")
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new NalogOtvorenHandler(fakeRepo.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			fakeRepo.Verify(x => x.Add(It.Is<NalogForm>(n => n.Datum == new DateTime(2018, 10, 21) &&
				  n.IdTip == 2 && n.Opis == "opis novi")));
		}
	}
}
