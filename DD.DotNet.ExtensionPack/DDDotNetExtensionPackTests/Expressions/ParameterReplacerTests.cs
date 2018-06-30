using System;
using System.Linq.Expressions;
using DD.DotNet.ExtensionPack.Expressions;
using NUnit.Framework;

namespace DDDotNetExtensionPackTests.Expressions
{
    [TestFixture]
    public partial class ParameterReplacerTests
    {
        [Test]
        public void ChangeParameterType_SelfReturn_CreatedSucceeded()
        {
            // Arrange
            Expression<Func<object, object>> sourceExp = x => x;

            // Assert
            Assert.DoesNotThrow(() => sourceExp.ChangeParameterType<object, string>()
                .ToLambda<string, string>());
        }

        #region method call

        [Test]
        public void ChangeParameterType_CallMethodWithoutArguments_CreatedSucceeded()
        {
            // Arrange
            Expression<Func<User, string>> sourceExp = x => x.GetUserName();
            var employee = new Employee { UserName = "employee" };

            // Act
            var delegateLambda = sourceExp.ChangeParameterType<User, Employee>()
                .ToLambda<Employee, string>()
                .Compile();

            // Assert
            Assert.AreEqual(delegateLambda(employee), employee.GetUserName());
        }

        [Test]
        public void ChangeParameterType_CallMethodWithoutArgumentsWhichHasIntoSourceTypeOnly_ExpectedException()
        {
            // Arrange
            Expression<Func<User, string>> sourceExp = x => x.GetPassword();
            var expectedExceptionMsg = $"Expression use unsupported method {nameof(User.GetPassword)}.";

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => sourceExp.ChangeParameterType<User, Employee>()
                .ToLambda<Employee, string>()
                .Compile()
            );

            // Assert
            Assert.AreEqual(ex.Message, expectedExceptionMsg);
        }

        [Test]
        public void ChangeParameterType_CallMethodWithArguments_CreatedSucceeded()
        {
            // Arrange
            var postfix = "denis";
            Expression<Func<User, string>> sourceExp = x => x.GetConcatedUserName(postfix);
            var employee = new Employee { UserName = "employee" };

            // Act
            var delegateLambda = sourceExp.ChangeParameterType<User, Employee>()
                .ToLambda<Employee, string>()
                .Compile();

            // Assert
            Assert.AreEqual(delegateLambda(employee), employee.GetConcatedUserName(postfix)
            );
        }

        [Test]
        public void ChangeParameterType_CallMethodWithDifferentArguments_ExpectedException()
        {
            // Arrange
            var user = new User { UserName = "user" };
            Expression<Func<User, string>> sourceExp = x => x.GetConcatedUserName(user);
            var expectedExceptionMsg = $"Expression use unsupported method {nameof(User.GetConcatedUserName)}.";

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => sourceExp.ChangeParameterType<User, Employee>());

            // Assert
            Assert.AreEqual(ex.Message, expectedExceptionMsg);
        }

        #endregion

        #region return property

        [Test]
        public void ChangeParameterType_ReturnPropertyWhichHasTwoIntoTypes_CreatedSucceeded()
        {
            // Arrange
            Expression<Func<User, string>> sourceExp = x => x.UserName;
            var employee = new Employee { UserName = "employee" };

            // Act
            var lambdaDelegate = sourceExp.ChangeParameterType<User, Employee>()
                .ToLambda<Employee, string>()
                .Compile();

            // Assert
            Assert.AreEqual(lambdaDelegate(employee), employee.UserName);
        }

        [Test]
        public void ChangeParameterType_ReturnPropertyWhichHasSourceTypeOnly_ExpectedException()
        {
            // Arrange
            Expression<Func<User, string>> sourceExp = x => x.Password;
            var expectedMsg = $"A property '{nameof(User.Password)}' is not exists into target type.";

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => sourceExp.ChangeParameterType<User, Employee>());

            // Assert
            Assert.AreEqual(ex.Message, expectedMsg);
        }

        #endregion

        #region expressions with field

        [Test]
        public void ChangeParameterType_ReturnBooleanOfFieldOperationWhichHasIntoTwoTypes_CreatedSucceeded()
        {
            // Arrange
            const int rightConst = 18;
            var employee = new Employee { UserName = "employee", Age = 25 };
            Expression<Func<User, bool>> sourceExp = x => x.Age > rightConst;

            // Act
            var lambdaDelegate = sourceExp.ChangeParameterType<User, Employee>()
                .ToLambda<Employee, bool>()
                .Compile();

            // Assert
            Assert.AreEqual(lambdaDelegate(employee), employee.Age > rightConst);
        }

        [Test]
        public void ChangeParameterType_ReturnFieldWhichHasSourceTypeOnly_ExpectedException()
        {
            // Arrange
            Expression<Func<User, bool>> sourceExp = x => x.Authorized;
            var expectedExMsg = $"A field '{nameof(User.Authorized)}' is not exists into target type.";

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => sourceExp.ChangeParameterType<User, Employee>()
                .ToLambda<Employee, bool>()
                .Compile());

            // Assert
            Assert.AreEqual(ex.Message, expectedExMsg);
        }

        #endregion
    }
}