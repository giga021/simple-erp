using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Domain.Services
{
	public class FinNalogServiceTest
	{
		[Fact]
		public async Task IzmeniZaglavlje_NalogNull_Greska()
		{
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var nalogSvc = new FinNalogService(fakeNalogRepo.Object);

			Func<Task> izmeni = async () => await nalogSvc.IzmeniZaglavljeAsync(null, TipNaloga.Izvodi, new DateTime(2018, 10, 20), "opis");

			await Assert.ThrowsAsync<ArgumentNullException>(izmeni);
		}

		[Fact]
		public async Task IzmeniZaglavlje_TipNalogaNull_Greska()
		{
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var nalogSvc = new FinNalogService(fakeNalogRepo.Object);
			var nalogIzBaze = new Mock<FinNalog>();

			Func<Task> izmeni = async () => await nalogSvc.IzmeniZaglavljeAsync(nalogIzBaze.Object, null, new DateTime(2018, 10, 21), "opis");

			await Assert.ThrowsAsync<ArgumentNullException>(izmeni);
		}

		[Fact]
		public async Task VecPostoji_Greska()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			var nalogIzBazeIdRandom = new Mock<FinNalogId>
			{
				CallBase = true
			};
			nalogIzBazeIdRandom.SetupGet(x => x.Id).Returns(Guid.NewGuid());
			fakeNalogRepo.Setup(x => x.GetPostojeciAsync(TipNaloga.UlazneFakture, new DateTime(2018, 10, 21))).ReturnsAsync(nalogIzBazeIdRandom.Object);

			var nalogSvc = new FinNalogService(fakeNalogRepo.Object);

			Func<Task> izmeni = async () => await nalogSvc.IzmeniZaglavljeAsync(nalogIzBaze.Object, TipNaloga.UlazneFakture, new DateTime(2018, 10, 21), "opis");

			await Assert.ThrowsAsync<KnjizenjeException>(izmeni);
		}

		[Fact]
		public async Task Postoji_KorektnoMenja()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetPostojeciAsync(TipNaloga.Izvodi, new DateTime(2018, 10, 20))).ReturnsAsync(nalogIzBaze.Object.Id);
			var nalogSvc = new FinNalogService(fakeNalogRepo.Object);

			await nalogSvc.IzmeniZaglavljeAsync(nalogIzBaze.Object, TipNaloga.Izvodi, new DateTime(2018, 10, 20), "opis new");

			nalogIzBaze.Verify(x => x.IzmeniZaglavlje(TipNaloga.Izvodi, new DateTime(2018, 10, 20), "opis new"));
		}

		[Fact]
		public async Task NePostoji_KorektnoMenja()
		{
			var nalogIzBaze = FinNalogHelper.NalogFromDb(callBase: false);
			var fakeNalogRepo = new Mock<IFinNalogRepository>();
			fakeNalogRepo.Setup(x => x.GetPostojeciAsync(TipNaloga.Izvodi, new DateTime(2018, 10, 20))).ReturnsAsync(null as FinNalogId);
			var nalogSvc = new FinNalogService(fakeNalogRepo.Object);

			await nalogSvc.IzmeniZaglavljeAsync(nalogIzBaze.Object, TipNaloga.Izvodi, new DateTime(2018, 10, 20), "opis new");

			nalogIzBaze.Verify(x => x.IzmeniZaglavlje(TipNaloga.Izvodi, new DateTime(2018, 10, 20), "opis new"));
		}
	}
}
