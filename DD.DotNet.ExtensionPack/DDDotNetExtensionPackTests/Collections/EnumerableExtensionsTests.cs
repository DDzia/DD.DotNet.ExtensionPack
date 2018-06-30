using System;
using System.Collections.Generic;
using DD.DotNet.ExtensionPack.Collections;
using NUnit.Framework;

namespace DDDotNetExtensionPackTests.Collections
{
    [TestFixture]
    public partial class EnumerableExtensionsTests
    {
        [Test]
        public void ForEach_EnumetareRealCollectionWithIndex_CallbacksCalled()
        {
            // Arrange
            var collection = new[]
            {
                "a", "b", "c"
            };
            var expectedSum = 3;

            // Act
            var result = String.Empty;
            long indexesSum = 0;
            collection.ForEach((x, index) =>
            {
                result += x;
                checked
                {
                    indexesSum += index;
                }
            });

            // Assert
            Assert.AreEqual(String.Join(String.Empty, collection), result);
            Assert.AreEqual(indexesSum, expectedSum);
        }

        [Test]
        public void ForEach_EnumetareRealCollectionWithoutIndex_CallbacksCalled()
        {
            // Arrange
            var collection = new[]
            {
                "a", "b", "c"
            };

            // Act
            var result = String.Empty;
            collection.ForEach(x => result += x);

            // Assert
            Assert.AreEqual(String.Join(String.Empty, collection), result);
        }

        [Test]
        public void ForEach_CollectionArgumentIsNull_ExpectedException()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.ForEach(collection, _ => { }));
        }

        [Test]
        public void ForEach_EnumetareRealCollectionWithNullHandler_ExpectedException()
        {
            // Arrange
            var collection = new[]
            {
                "a", "b", "c"
            };
            Action<string> handler = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => collection.ForEach(handler));
        }
    }
}