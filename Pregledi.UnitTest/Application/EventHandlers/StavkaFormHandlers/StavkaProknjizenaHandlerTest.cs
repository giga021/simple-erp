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
	public class StavkaProknjizenaHandlerTest
	{
		[Fact]
		public async Task KontoNePostoji_Greska()
		{
			var fakeRepo = new Mock<IStavkaFormRepository>();
			var fakeKontoRepo = new Mock<IKontoRepository>();
			var fakeLogger = new Mock<ILogger<StavkaProknjizenaHandler>>();
			var evnt = new StavkaProknjizena(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 10, 22), 1, 100, 0, "opis stavke");
			var handler = new StavkaProknjizenaHandler(fakeRepo.Object, fakeKontoRepo.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<PreglediException>(handle);
		}

		[Theory]
		[InlineData(50, 0)]
		[InlineData(0, 50)]
		[InlineData(-50, 0)]
		[InlineData(0, -50)]
		public async Task Handle_Korektno(decimal duguje, decimal potrazuje)
		{
			var fakeRepo = new Mock<IStavkaFormRepository>();
			var fakeKontoRepo = new Mock<IKontoRepository>();
			fakeKontoRepo.Setup(x => x.GetAsync(1)).ReturnsAsync(new Konto() { Id = 1, Sifra = "435" });
			var fakeLogger = new Mock<ILogger<StavkaProknjizenaHandler>>();
			var evnt = new StavkaProknjizena(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 10, 22), 1, duguje, potrazuje, "opis stavke")
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new StavkaProknjizenaHandler(fakeRepo.Object, fakeKontoRepo.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			fakeRepo.Verify(x => x.Add(It.Is<StavkaForm>(n => n.Id == evnt.IdStavke &&
				n.IdNaloga == evnt.IdNaloga && n.IdKonto == evnt.IdKonto && n.Konto == "435" &&
				n.Opis == "opis stavke" && n.Duguje == evnt.Duguje && n.Potrazuje == n.Potrazuje &&
				n.DatumKnjizenja == evnt.DatumKnjizenja)));
		}
	}
}
