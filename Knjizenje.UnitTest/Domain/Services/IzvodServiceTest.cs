using FluentAssertions;
using Knjizenje.Domain.DTO;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Entities.Konto;
using Knjizenje.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Knjizenje.UnitTest.Domain.Services
{
	public class IzvodServiceTest
	{
		[Fact]
		public async Task FormirajStavkeNaloga_StavkeIzvodaNull_Greska()
		{
			var fakeKontoRepo = new Mock<IKontoRepository>();
			var izvodSvc = new IzvodService(fakeKontoRepo.Object);

			Func<Task> formiraj = async () => await izvodSvc.FormirajStavkeNalogaAsync(null);

			await Assert.ThrowsAsync<ArgumentNullException>(formiraj);
		}

		[Fact]
		public async Task FormirajStavkeNaloga_StavkeIzvodaPrazno_VracaPrazno()
		{
			var fakeKontoRepo = new Mock<IKontoRepository>();
			var izvodSvc = new IzvodService(fakeKontoRepo.Object);
			var stavkeIzvoda = Enumerable.Empty<StavkaIzvoda>();

			var stavkeNaloga = await izvodSvc.FormirajStavkeNalogaAsync(stavkeIzvoda);

			Assert.Empty(stavkeNaloga);
		}

		[Fact]
		public async Task FormirajStavkeNaloga_Korektno()
		{
			var fakeKontoRepo = new Mock<IKontoRepository>();
			fakeKontoRepo.Setup(x => x.GetIdBySifraAsync("435", "241"))
				.Returns(() =>
				{
					return Task.FromResult(new Dictionary<string, long>
					{
						["435"] = 435,
						["241"] = 241
					});
				});
			var izvodSvc = new IzvodService(fakeKontoRepo.Object);
			var stavkeIzvoda = new[]
			{
				new StavkaIzvoda(221, 200, 0),
				new StavkaIzvoda(221, 0, 143)
			};

			var stavkeNaloga = await izvodSvc.FormirajStavkeNalogaAsync(stavkeIzvoda);

			stavkeNaloga.Should().BeEquivalentTo(new[]
			{
				new
				{
					IdKonto = 435,
					Iznos = new IznosStavke(200, 0)
				},
				new
				{
					IdKonto = 241,
					Iznos = new IznosStavke(0, 200)
				},
				new
				{
					IdKonto = 435,
					Iznos = new IznosStavke(0, 143)
				},
				new
				{
					IdKonto = 241,
					Iznos = new IznosStavke(143, 0)
				}
			});
		}
	}
}
