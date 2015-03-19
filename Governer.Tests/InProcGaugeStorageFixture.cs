using System;
using NUnit.Framework;
using Governer.Internal;

namespace Governer.Tests
{
	[TestFixture]
	public class InProcGaugeStorageFixture
	{

		private readonly IGaugeStorage Storage  = InProcGaugeStorage.Instance;
		[Test]
		public void FirstIncrementCallShouldInitializeCounterTest()
		{
			var gaugeName = Guid.NewGuid ().ToString ("N");
			Assert.AreEqual (1, this.Storage.Increment (gaugeName, 1));
		}

		[Test]
		public void MultipleCallsToIncrementForSameGaugeAndWindowShouldIncrementCounterTest()
		{
			var gaugeName = Guid.NewGuid ().ToString ("N");
			Assert.AreEqual (1, this.Storage.Increment (gaugeName, 1));
			Assert.AreEqual (2, this.Storage.Increment (gaugeName, 1));
			Assert.AreEqual (3, this.Storage.Increment (gaugeName, 1));
		}

		[Test]
		public void FirstIncrementWithDifferentWindowShouldResetCountTest()
		{
			var gaugeName = Guid.NewGuid ().ToString ("N");
			Assert.AreEqual (1, this.Storage.Increment (gaugeName, 1));
			Assert.AreEqual (2, this.Storage.Increment (gaugeName, 1));
			Assert.AreEqual (1, this.Storage.Increment (gaugeName, 2));
			Assert.AreEqual (2, this.Storage.Increment (gaugeName, 2));
		}
	}
}

