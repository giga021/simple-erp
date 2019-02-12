using FluentAssertions;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Events;
using Knjizenje.Domain.Exceptions;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Knjizenje.UnitTest.Domain.Entities.FinNalogAggregate
{
	public class FinNalogTest
	{
		[Fact]
		public void NoviNalog_TipNull_Greska()
		{
			Action noviNalog = () => new FinNalog(null, new DateTime(2018, 9, 1), "opis", new[]
			{
				new FinStavka(34, 500, 0, "opis stavke")
			});

			Assert.Throws<ArgumentNullException>(noviNalog);
		}

		[Fact]
		public void NoviNalog_StavkeNull_Greska()
		{
			Action noviNalog = () => new FinNalog(TipNaloga.UlazneFakture, new DateTime(2018, 9, 1), "opis", null);

			Assert.Throws<ArgumentNullException>(noviNalog);
		}

		[Fact]
		public void NoviNalog_StavkePrazno_Greska()
		{
			Action noviNalog = () => new FinNalog(TipNaloga.UlazneFakture, new DateTime(2018, 9, 1), "opis", Enumerable.Empty<FinStavka>());

			Assert.Throws<KnjizenjeException>(noviNalog);
		}

		[Fact]
		public void NoviNalog_JednaStavka_DodajeEvente()
		{
			var noviNalog = new FinNalog(TipNaloga.UlazneFakture, new DateTime(2018, 10, 1), "opis", new[]
			{
				new FinStavka(1, 100, 0, "opis stavke")
			});

			Assert.Equal(2, noviNalog.UncommittedEvents.Count);
			Assert.Collection(noviNalog.UncommittedEvents.OfType<NalogOtvoren>(), e =>
			{
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.Equal(noviNalog.Id, e.IdNaloga);
				Assert.Equal(new DateTime(2018, 10, 1), e.DatumNaloga);
				Assert.Equal(TipNaloga.UlazneFakture.Id, e.IdTip);
				Assert.Equal("opis", e.Opis);
			});
			Assert.Collection(noviNalog.UncommittedEvents.OfType<StavkaProknjizena>(), e =>
			{
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.Equal(noviNalog.Id, e.IdNaloga);
				Assert.NotEqual(Guid.Empty, e.IdStavke);
				Assert.Equal(DateTime.Today, e.DatumKnjizenja);
				Assert.Equal(1, e.IdKonto);
				Assert.Equal(100, e.Duguje);
				Assert.Equal(0, e.Potrazuje);
				Assert.Equal("opis stavke", e.Opis);
			});
		}

		[Fact]
		public void NoviNalog_ViseStavki_DodajeEvente()
		{
			var noviNalog = new FinNalog(TipNaloga.UlazneFakture, new DateTime(2018, 10, 1), "opis", new[]
			{
				new FinStavka(1, 100, 0, "opis stavke 1"),
				new FinStavka(2, 0, 200, "opis stavke 2")
			});

			Assert.Equal(3, noviNalog.UncommittedEvents.Count);
			Assert.Collection(noviNalog.UncommittedEvents.OfType<NalogOtvoren>(), e =>
			{
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.Equal(noviNalog.Id, e.IdNaloga);
				Assert.Equal(new DateTime(2018, 10, 1), e.DatumNaloga);
				Assert.Equal(TipNaloga.UlazneFakture.Id, e.IdTip);
				Assert.Equal("opis", e.Opis);
			});
			Assert.Collection(noviNalog.UncommittedEvents.OfType<StavkaProknjizena>(), e =>
			{
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.Equal(noviNalog.Id, e.IdNaloga);
				Assert.NotEqual(Guid.Empty, e.IdStavke);
				Assert.Equal(DateTime.Today, e.DatumKnjizenja);
				Assert.Equal(1, e.IdKonto);
				Assert.Equal(100, e.Duguje);
				Assert.Equal(0, e.Potrazuje);
				Assert.Equal("opis stavke 1", e.Opis);
			}, e =>
			{
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.Equal(noviNalog.Id, e.IdNaloga);
				Assert.NotEqual(Guid.Empty, e.IdStavke);
				Assert.Equal(DateTime.Today, e.DatumKnjizenja);
				Assert.Equal(2, e.IdKonto);
				Assert.Equal(0, e.Duguje);
				Assert.Equal(200, e.Potrazuje);
				Assert.Equal("opis stavke 2", e.Opis);
			});
		}

		[Fact]
		public void Zakljucaj_DodajeEvent()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Zakljucan).Returns(false);

			nalog.Object.Zakljucaj();

			Assert.Equal(1, nalog.Object.UncommittedEvents.Count);
			Assert.Collection(nalog.Object.UncommittedEvents.OfType<NalogZakljucan>(), e =>
			{
				Assert.Equal(nalog.Object.Id, e.IdNaloga);
			});
		}

		[Fact]
		public void Zakljucaj_VecZakljucan()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Zakljucan).Returns(true);

			nalog.Object.Zakljucaj();

			Assert.Empty(nalog.Object.UncommittedEvents);
		}

		[Fact]
		public void Otkljucaj_DodajeEvent()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Zakljucan).Returns(true);

			nalog.Object.Otkljucaj();

			Assert.Equal(1, nalog.Object.UncommittedEvents.Count);
			Assert.Collection(nalog.Object.UncommittedEvents.OfType<NalogOtkljucan>(), e =>
			{
				Assert.Equal(nalog.Object.Id, e.IdNaloga);
			});
		}

		[Fact]
		public void Otkljucaj_VecOtkljucan()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Zakljucan).Returns(false);

			nalog.Object.Otkljucaj();

			Assert.Empty(nalog.Object.UncommittedEvents);
		}

		[Fact]
		public void Obrisi_DodajeEvent()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Obrisan).Returns(false);

			nalog.Object.Obrisi();

			Assert.Equal(1, nalog.Object.UncommittedEvents.Count);
			Assert.Collection(nalog.Object.UncommittedEvents.OfType<NalogObrisan>(), e =>
			{
				Assert.Equal(nalog.Object.Id, e.IdNaloga);
			});
		}

		[Fact]
		public void Obrisi_VecObrisan()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Obrisan).Returns(true);

			nalog.Object.Obrisi();

			Assert.Empty(nalog.Object.UncommittedEvents);
		}

		[Fact]
		public void Obrisi_NalogZakljucan_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Obrisan).Returns(false);
			nalog.SetupGet(x => x.Zakljucan).Returns(true);

			Action obrisi = () => nalog.Object.Obrisi();

			Assert.Throws<KnjizenjeException>(obrisi);
		}

		[Fact]
		public void Primeni_NalogOtvoren()
		{
			var nalogId = new Mock<FinNalogId>
			{
				CallBase = true
			};
			nalogId.SetupGet(x => x.Id).Returns(Guid.NewGuid());
			var nalog = new Mock<FinNalog>
			{
				CallBase = true
			};
			var evnt = new NalogOtvoren(nalogId.Object, new DateTime(2018, 10, 12), 1, "opis naloga");

			nalog.Object.Primeni(evnt);

			Assert.Equal(evnt.IdNaloga, nalog.Object.Id);
			Assert.Equal(evnt.DatumNaloga, nalog.Object.DatumNaloga);
			Assert.Equal(evnt.IdTip, nalog.Object.Tip.Id);
			Assert.Equal(evnt.Opis, nalog.Object.Opis);
		}

		[Fact]
		public void Primeni_NalogZakljucan()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupProperty(x => x.Zakljucan, false);

			nalog.Object.Primeni(new NalogZakljucan(nalog.Object.Id));

			Assert.True(nalog.Object.Zakljucan);
		}

		[Fact]
		public void Primeni_NalogOtkljucan()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupProperty(x => x.Zakljucan, true);

			nalog.Object.Primeni(new NalogOtkljucan(nalog.Object.Id));

			Assert.False(nalog.Object.Zakljucan);
		}

		[Fact]
		public void Primeni_NalogObrisan()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupProperty(x => x.Obrisan, false);

			nalog.Object.Primeni(new NalogObrisan(nalog.Object.Id));

			Assert.True(nalog.Object.Obrisan);
		}

		[Fact]
		public void Primeni_StavkaProknjizena()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			var evnt = new StavkaProknjizena(nalog.Object.Id, Guid.NewGuid(), new DateTime(2018, 10, 3), 4, 789, 0, "opis");

			nalog.Object.Primeni(evnt);

			Assert.Equal(1, nalog.Object.Stavke.Count);
			nalog.Object.Stavke.Should().BeEquivalentTo(new[]
			{
				new
				{
					Id = evnt.IdStavke,
					IdKonto = evnt.IdKonto,
					DatumKnjizenja = evnt.DatumKnjizenja,
					Iznos = new IznosStavke(evnt.Duguje, evnt.Potrazuje),
					Opis = evnt.Opis
				}
			});
		}

		[Fact]
		public void Primeni_StavkaUklonjena()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var stavka = nalog.Object.Stavke.First();
			var evnt = new StavkaUklonjena(nalog.Object.Id, stavka.Id, stavka.DatumKnjizenja,
				stavka.IdKonto, stavka.Iznos.Duguje, stavka.Iznos.Potrazuje, stavka.Opis);

			nalog.Object.Primeni(evnt);

			Assert.Equal(1, nalog.Object.Stavke.Count);
			Assert.DoesNotContain(nalog.Object.Stavke, s => s.Id == evnt.IdStavke);
		}

		[Fact]
		public void ProknjiziStavku_StavkaNull_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);

			Action proknjizi = () => nalog.Object.ProknjiziStavku(null);

			Assert.Throws<ArgumentNullException>(proknjizi);
		}

		[Fact]
		public void ProknjiziStavku_IdStavkeDefault_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			var stavka = new Mock<FinStavka>();
			stavka.SetupGet(x => x.Id).Returns(default(Guid));

			Action proknjizi = () => nalog.Object.ProknjiziStavku(stavka.Object);

			Assert.Throws<KnjizenjeException>(proknjizi);
		}

		[Fact]
		public void ProknjiziStavku_StavkaVecProknjizena_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30))
			});
			var stavka = nalog.Object.Stavke.First();

			Action proknjizi = () => nalog.Object.ProknjiziStavku(stavka);

			Assert.Throws<KnjizenjeException>(proknjizi);
		}

		[Fact]
		public void ProknjiziStavku_NalogZakljucan_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Zakljucan).Returns(true);

			Action proknjizi = () => nalog.Object.ProknjiziStavku(new FinStavka(3, 400, 0, "opis"));

			Assert.Throws<KnjizenjeException>(proknjizi);
		}

		[Theory]
		[InlineData(587.5, 0)]
		[InlineData(-36.7, 0)]
		[InlineData(0, 986.2)]
		[InlineData(0, -520089)]
		public void ProknjiziStavku_DodajeEvent(decimal dug, decimal pot)
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);

			var stavka = new FinStavka(56, dug, pot, "opis proknjizene stavke");
			nalog.Object.ProknjiziStavku(stavka);

			Assert.Equal(1, nalog.Object.UncommittedEvents.Count);
			Assert.Collection(nalog.Object.UncommittedEvents.OfType<StavkaProknjizena>(), e =>
			{
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.Equal(nalog.Object.Id, e.IdNaloga);
				Assert.NotEqual(Guid.Empty, e.IdStavke);
				Assert.Equal(DateTime.Today, e.DatumKnjizenja);
				Assert.Equal(stavka.IdKonto, e.IdKonto);
				Assert.Equal(stavka.Iznos.Duguje, e.Duguje);
				Assert.Equal(stavka.Iznos.Potrazuje, e.Potrazuje);
				Assert.Equal(stavka.Opis, e.Opis);
			});
		}

		[Fact]
		public void UkloniStavku_StavkaNull_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);

			Action ukloni = () => nalog.Object.UkloniStavku(null);

			Assert.Throws<ArgumentNullException>(ukloni);
		}

		[Fact]
		public void UkloniStavku_NalogZakljucan_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Zakljucan).Returns(true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var stavka = nalog.Object.Stavke.First();

			Action ukloni = () => nalog.Object.UkloniStavku(stavka);

			Assert.Throws<KnjizenjeException>(ukloni);
		}

		[Fact]
		public void UkloniStavku_NalogImaJednuStavku_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30))
			});
			var stavka = nalog.Object.Stavke.First();

			Action ukloni = () => nalog.Object.UkloniStavku(stavka);

			Assert.Throws<KnjizenjeException>(ukloni);
		}

		[Fact]
		public void UkloniStavku_StavkaNePripadaNalogu_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var stavka = new FinStavka(Guid.NewGuid(), 3, 300, 0, "opis stavke 3", new DateTime(2018, 10, 30));

			Action ukloni = () => nalog.Object.UkloniStavku(stavka);

			Assert.Throws<KnjizenjeException>(ukloni);
		}

		[Fact]
		public void UkloniStavku_DodajeEvent()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var stavka = nalog.Object.Stavke.First();

			nalog.Object.UkloniStavku(stavka);

			Assert.Equal(1, nalog.Object.UncommittedEvents.Count);
			Assert.Collection(nalog.Object.UncommittedEvents.OfType<StavkaUklonjena>(), e =>
			{
				Assert.Equal(nalog.Object.Id, e.IdNaloga);
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.Equal(stavka.Id, e.IdStavke);
				Assert.NotEqual(Guid.Empty, e.IdStavke);
				Assert.Equal(stavka.DatumKnjizenja, e.DatumKnjizenja);
				Assert.Equal(stavka.IdKonto, e.IdKonto);
				Assert.Equal(stavka.Iznos.Duguje, e.Duguje);
				Assert.Equal(stavka.Iznos.Potrazuje, e.Potrazuje);
				Assert.Equal(stavka.Opis, e.Opis);
			});
		}

		[Fact]
		public void StornirajStavku_StavkaNull_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);

			Action storniraj = () => nalog.Object.StornirajStavku(null);

			Assert.Throws<ArgumentNullException>(storniraj);
		}

		[Fact]
		public void StornirajStavku_NalogZakljucan_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Zakljucan).Returns(true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var stavka = nalog.Object.Stavke.First();

			Action storniraj = () => nalog.Object.StornirajStavku(stavka);

			Assert.Throws<KnjizenjeException>(storniraj);
		}

		[Fact]
		public void StornirajStavku_StavkaNePripadaNalogu_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var stavka = new FinStavka(Guid.NewGuid(), 3, 300, 0, "opis stavke 3", new DateTime(2018, 10, 30));

			Action storniraj = () => nalog.Object.StornirajStavku(stavka);

			Assert.Throws<KnjizenjeException>(storniraj);
		}

		[Fact]
		public void StornirajStavku_DodajeEvent()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.Protected().SetupGet<List<FinStavka>>("_stavke").Returns(new List<FinStavka>
			{
				new FinStavka(Guid.NewGuid(), 1, 100, 0, "opis stavke 1", new DateTime(2018, 10, 30)),
				new FinStavka(Guid.NewGuid(), 2, 200, 0, "opis stavke 2", new DateTime(2018, 10, 30))
			});
			var stavka = nalog.Object.Stavke.First();

			nalog.Object.StornirajStavku(stavka);

			Assert.Equal(1, nalog.Object.UncommittedEvents.Count);
			Assert.Collection(nalog.Object.UncommittedEvents.OfType<StavkaProknjizena>(), e =>
			{
				Assert.Equal(nalog.Object.Id, e.IdNaloga);
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.NotEqual(stavka.Id, e.IdStavke);
				Assert.NotEqual(Guid.Empty, e.IdStavke);
				Assert.Equal(-stavka.Iznos.Duguje, e.Duguje);
				Assert.Equal(-stavka.Iznos.Potrazuje, e.Potrazuje);
				Assert.Null(e.Opis);
			});
		}

		[Fact]
		public void IzmeniZaglavlje_TipNull_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);

			Action izmeni = () => nalog.Object.IzmeniZaglavlje(null, new DateTime(1987, 5, 2), "promenjen opis");

			Assert.Throws<ArgumentNullException>(izmeni);
		}

		[Fact]
		public void IzmeniZaglavlje_NalogZakljucan_Greska()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Zakljucan).Returns(true);

			Action izmeni = () => nalog.Object.IzmeniZaglavlje(TipNaloga.Izvodi, new DateTime(1987, 5, 2), "promenjen opis");

			Assert.Throws<KnjizenjeException>(izmeni);
		}

		[Fact]
		public void IzmeniZaglavlje_NistaPromenjeno()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Tip).Returns(TipNaloga.Izvodi);
			nalog.SetupGet(x => x.DatumNaloga).Returns(new DateTime(1987, 5, 2));
			nalog.SetupGet(x => x.Opis).Returns("promenjen opis");

			nalog.Object.IzmeniZaglavlje(TipNaloga.Izvodi, new DateTime(1987, 5, 2), "promenjen opis");

			Assert.Empty(nalog.Object.UncommittedEvents);
		}

		[Fact]
		public void IzmeniZaglavlje_DodajeEvent()
		{
			var nalog = FinNalogHelper.NalogFromDb(callBase: true);
			nalog.SetupGet(x => x.Tip).Returns(TipNaloga.UlazneFakture);
			nalog.SetupGet(x => x.DatumNaloga).Returns(new DateTime(1987, 6, 2));
			nalog.SetupGet(x => x.Opis).Returns("promenjen opis");

			nalog.Object.IzmeniZaglavlje(TipNaloga.Izvodi, new DateTime(1987, 5, 2), "promenjen opis");

			Assert.Equal(1, nalog.Object.UncommittedEvents.Count);
			Assert.Collection(nalog.Object.UncommittedEvents.OfType<IzmenjenoZaglavljeNaloga>(), e =>
			{
				Assert.Equal(nalog.Object.Id, e.IdNaloga);
				Assert.NotEqual(Guid.Empty, e.IdNaloga.Id);
				Assert.Equal(new DateTime(1987, 5, 2), e.DatumNaloga);
				Assert.Equal(TipNaloga.Izvodi.Id, e.IdTip);
				Assert.Equal("promenjen opis", e.Opis);
			});
		}
	}
}
