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
        public void ThrowIfNullOrReturnWithoutMessage_Thrown_ExpectedException()
        {
            // Arrange
            object o = null;
            var oName = nameof(o);
            var expectedExMessage = BuildExpectedMessage(oName);

            // Act
            var exOfVariable = Assert.Throws<ArgumentNullException>(() => Argument.ThrowIfNullOrReturn(o));
            var exOfLambda = Assert.Throws<ArgumentNullException>(() => Argument.ThrowIfNullOrReturn(() => o));

            // Begin Assert
            Assert.IsNull(exOfVariable.ParamName);
            Assert.AreEqual(exOfVariable.Message, _defaultParamNullRefExMsg);

            Assert.AreEqual(exOfLambda.ParamName, oName);
            Assert.AreEqual(exOfLambda.Message, expectedExMessage);
            // End Assert
        }

        [Test]
        public void ThrowIfNullOrReturnWithMessage_Thrown_ExpectedException()
        {
            // Arrange
            object o = null;
            var oName = nameof(o);
            var exMsg = "err";
            var expectedMsg = BuildExpectedMessage(oName, exMsg);

            // Act
            var exOfVariable = Assert.Throws<ArgumentNullException>(() => Argument.ThrowIfNullOrReturn(o, oName, exMsg));
            var exOfLambda = Assert.Throws<ArgumentNullException>(() => Argument.ThrowIfNullOrReturn(() => o, exMsg));

            // Begin Assert
            Assert.AreEqual(exOfVariable.ParamName, oName);
            Assert.AreEqual(exOfVariable.Message, expectedMsg);

            Assert.AreEqual(exOfLambda.ParamName, oName);
            Assert.AreEqual(exOfLambda.Message, expectedMsg);
            // End Assert
        }

        [Test]
        public void ThrowIfNullOrReturn_Invoked_DoNotThrows()
        {
            // Arrange
            var value = new Object();

            // Act
            object returnOfVariable = null;
            Assert.DoesNotThrow(() => returnOfVariable = Argument.ThrowIfNullOrReturn(value));
            object returnOfLambda = null;
            Assert.DoesNotThrow(() => returnOfLambda = Argument.ThrowIfNullOrReturn(() => value));
            
            // Act
            Assert.AreEqual(value, returnOfVariable);
            Assert.AreEqual(value, returnOfLambda);
        }
    }
}
