using Knjizenje.Domain.Entities.FinNalogAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knjizenje.UnitTest
{
	class FinNalogHelper
	{
		public static Mock<FinNalog> NalogFromDb(bool callBase)
		{
			var nalogId = new Mock<FinNalogId>()
			{
				CallBase = true
			};
			nalogId.SetupGet(x => x.Id).Returns(Guid.NewGuid());
			var nalog = new Mock<FinNalog>()
			{
				CallBase = callBase
			};
			nalog.SetupGet(x => x.Id).Returns(nalogId.Object);
			return nalog;
		}
	}
}
