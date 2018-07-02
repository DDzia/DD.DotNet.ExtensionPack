using System.Linq;
using DD.DotNet.ExtensionPack.Collections;
using NUnit.Framework;

namespace DDDotNetExtensionPackTests.Collections
{
    [TestFixture]
    public partial class EnumerableExtensionsTests
    {
        [Test]
        public void Do_HandlerNotInvokedForAggregationOnly_SumCalculatedForFirstCaseOnly()
        {
            // Arrange
            var collectionSize = 500;
            var skipSize = 100;
            var items = Enumerable.Range(0, collectionSize);

            // Act
            var lastIndexFirstCall = -1;
            items.Skip(skipSize)
                .Do((item, index) => lastIndexFirstCall = index);
            var lastIndexSecondCall = -1;
            items.Skip(skipSize)
                .Do((item, index) => lastIndexSecondCall = index)
                .ToArray();

            // Assert
            Assert.AreEqual(lastIndexFirstCall, -1);
            Assert.AreEqual(lastIndexSecondCall, collectionSize - 1 - skipSize);
        }

        [Test]
        public void Do_HandlerInvokedWithAggregation_ReturnedCollectionWithSameItems()
        {
            // Arrange
            var collectionSize = 500;
            var items = Enumerable.Range(0, collectionSize);

            // Act
            var returned = items.Do(() => { }).ToArray();

            // Assert
            Assert.AreEqual(returned.Intersect(items).Count(), collectionSize);
        }
    }
}