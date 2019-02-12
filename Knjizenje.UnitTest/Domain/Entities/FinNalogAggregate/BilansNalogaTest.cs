using Knjizenje.Domain.Entities.FinNalogAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Knjizenje.UnitTest.Domain.Entities.FinNalogAggregate
{
	public class BilansNalogaTest
    {
		[Theory]
		[InlineData(128, 0, 128, 0)]
		[InlineData(0, 45, 0, 45)]
		[InlineData(-52, 20, -52, 20)]
		[InlineData(68, -44, 68, -44)]
		[InlineData(0, 0, 0, 0)]
		public void RazliciteInstance_Equals(decimal dug1, decimal pot1, decimal dug2, decimal pot2)
		{
			BilansNaloga bilans1 = new BilansNaloga(dug1, pot1);
			BilansNaloga bilans2 = new BilansNaloga(dug2, pot2);

			bool equals = bilans1.Equals(bilans2);

			Assert.True(equals);
		}

		[Theory]
		[InlineData(128, 0, 128, 1)]
		[InlineData(1, 45, 0, 45)]
		[InlineData(17, 84, -5, 3)]
		[InlineData(-8, 15, 89, -4)]
		public void RazliciteInstance_NotEquals(decimal dug1, decimal pot1, decimal dug2, decimal pot2)
		{
			BilansNaloga bilans1 = new BilansNaloga(dug1, pot1);
			BilansNaloga bilans2 = new BilansNaloga(dug2, pot2);

			bool equals = bilans1.Equals(bilans2);

			Assert.False(equals);
		}

		[Theory]
		[InlineData(128, 0, 128, 0)]
		[InlineData(0, 45, 0, 45)]
		[InlineData(-52, 20, -52, 20)]
		[InlineData(68, -44, 68, -44)]
		[InlineData(0, 0, 0, 0)]
		public void RazliciteInstance_AreEqual(decimal dug1, decimal pot1, decimal dug2, decimal pot2)
		{
			BilansNaloga bilans1 = new BilansNaloga(dug1, pot1);
			BilansNaloga bilans2 = new BilansNaloga(dug2, pot2);

			bool equal = bilans1 == bilans2;

			Assert.True(equal);
		}

		[Theory]
		[InlineData(128, 0, 128, 1)]
		[InlineData(1, 45, 0, 45)]
		[InlineData(17, 84, -5, 3)]
		[InlineData(-8, 15, 89, -4)]
		public void RazliciteInstance_AreNotEqual(decimal dug1, decimal pot1, decimal dug2, decimal pot2)
		{
			BilansNaloga bilans1 = new BilansNaloga(dug1, pot1);
			BilansNaloga bilans2 = new BilansNaloga(dug2, pot2);

			bool equal = bilans1 != bilans2;

			Assert.True(equal);
		}
	}
}
