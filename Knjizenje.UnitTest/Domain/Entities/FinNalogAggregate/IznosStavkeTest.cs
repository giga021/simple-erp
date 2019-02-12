using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using System;
using Xunit;

namespace Knjizenje.UnitTest.Domain.Entities.FinNalogAggregate
{
	public class IznosStavkeTest
	{
		[Fact]
		public void NoviIznos_NijednaStranaDefinisana_Greska()
		{
			Action noviIznos = () => { new IznosStavke(0, 0); };

			Assert.Throws<KnjizenjeException>(noviIznos);
		}

		[Theory]
		[InlineData(128, 12)]
		[InlineData(-15, 45)]
		[InlineData(-68, -88)]
		[InlineData(17, -1525)]
		[InlineData(-8, -8)]
		[InlineData(250, 250)]
		public void NoviIznos_ObeStraneDefinisane_Greska(decimal dug, decimal pot)
		{
			Action noviIznos = () => { new IznosStavke(dug, pot); };

			Assert.Throws<KnjizenjeException>(noviIznos);
		}

		[Theory]
		[InlineData(128, 0)]
		[InlineData(0, 45)]
		[InlineData(-68, 0)]
		[InlineData(0, -1525)]
		public void NoviIznos_Korektno(decimal dug, decimal pot)
		{
			IznosStavke noviIznos = new IznosStavke(dug, pot);

			Assert.Equal(dug, noviIznos.Duguje);
			Assert.Equal(pot, noviIznos.Potrazuje);
		}

		[Theory]
		[InlineData(128, 0, 128, 0)]
		[InlineData(0, 45, 0, 45)]
		[InlineData(-68, 0, -68, 0)]
		[InlineData(0, -1525, 0, -1525)]
		public void RazliciteInstance_Equals(decimal dug1, decimal pot1,
			decimal dug2, decimal pot2)
		{
			IznosStavke iznos1 = new IznosStavke(dug1, pot1);
			IznosStavke iznos2 = new IznosStavke(dug2, pot2);

			bool equals = iznos1.Equals(iznos2);

			Assert.True(equals);
		}

		[Theory]
		[InlineData(21, 0, 128, 0)]
		[InlineData(0, 45, 0, 89)]
		[InlineData(-2, 0, -68, 0)]
		[InlineData(0, -1525, 0, -695)]
		public void RazliciteInstance_NotEquals(decimal dug1, decimal pot1,
			decimal dug2, decimal pot2)
		{
			IznosStavke iznos1 = new IznosStavke(dug1, pot1);
			IznosStavke iznos2 = new IznosStavke(dug2, pot2);

			bool equals = iznos1.Equals(iznos2);

			Assert.False(equals);
		}

		[Theory]
		[InlineData(128, 0, 128, 0)]
		[InlineData(0, 45, 0, 45)]
		[InlineData(-68, 0, -68, 0)]
		[InlineData(0, -1525, 0, -1525)]
		public void RazliciteInstance_AreEqual(decimal dug1, decimal pot1,
			decimal dug2, decimal pot2)
		{
			IznosStavke iznos1 = new IznosStavke(dug1, pot1);
			IznosStavke iznos2 = new IznosStavke(dug2, pot2);

			bool equals = iznos1 == iznos2;

			Assert.True(equals);
		}

		[Theory]
		[InlineData(21, 0, 128, 0)]
		[InlineData(0, 45, 0, 89)]
		[InlineData(-2, 0, -68, 0)]
		[InlineData(0, -1525, 0, -695)]
		public void RazliciteInstance_AreNotEqual(decimal dug1, decimal pot1,
			decimal dug2, decimal pot2)
		{
			IznosStavke iznos1 = new IznosStavke(dug1, pot1);
			IznosStavke iznos2 = new IznosStavke(dug2, pot2);

			bool equals = iznos1 != iznos2;

			Assert.True(equals);
		}
	}
}
