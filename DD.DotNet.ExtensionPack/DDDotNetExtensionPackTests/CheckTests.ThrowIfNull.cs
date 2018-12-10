using System;
using DD.DotNet.ExtensionPack;
using DD.DotNet.ExtensionPack.Validation;
using NUnit.Framework;

namespace DDDotNetExtensionPackTests
{
    [TestFixture]
    public partial class CheckTests
    {
        [Test]
        public void ThrowIfNullWithoutMessageParameter_Thrown_ExpectedException()
        {
            // Arrange
            object o = null;
            var oName = nameof(o);
            var expectedMsg = BuildExpectedMessage(oName);

            // Act
            var exOfVariable = Assert.Throws<ArgumentNullException>(() => Argument.ThrowIfNull(o));
            var exOfLambda = Assert.Throws<ArgumentNullException>(() => Argument.ThrowIfNull(() => o));

            // Begin Assert
            Assert.IsNull(exOfVariable.ParamName);
            Assert.AreEqual(exOfVariable.Message, _defaultParamNullRefExMsg);

            Assert.AreEqual(exOfLambda.ParamName, oName);
            Assert.AreEqual(exOfLambda.Message, expectedMsg);
            // End Assert
        }

        [Test]
        public void ThrowIfNullWithMessageParameter_Thrown_ExpectedException()
        {
            // Arrange
            object o = null;
            var oName = nameof(o);
            var msg = "err";
            var expectedMsg = BuildExpectedMessage(oName, msg);

            // Act
            var exOfVariable = Assert.Throws<ArgumentNullException>(() => Argument.ThrowIfNull(o, oName, msg));
            var exOfLambda =  Assert.Throws<ArgumentNullException>(() => Argument.ThrowIfNull(() => o, msg));

            // Begin Assert
            Assert.AreEqual(exOfVariable.ParamName, oName);
            Assert.AreEqual(exOfVariable.Message, expectedMsg);

            Assert.AreEqual(exOfLambda.ParamName, oName);
            Assert.AreEqual(exOfLambda.Message, expectedMsg);
            // End Assert
        }

        [Test]
        public void ThrowIfNullWithoutMessageParameter_Invoked_DoNotThrown()
        {
            // Arrange
            var o = new Object();

            // Assert
            Assert.DoesNotThrow(() => Argument.ThrowIfNull(o));
            Assert.DoesNotThrow(() => Argument.ThrowIfNull(() => o));
        }
    }
}
