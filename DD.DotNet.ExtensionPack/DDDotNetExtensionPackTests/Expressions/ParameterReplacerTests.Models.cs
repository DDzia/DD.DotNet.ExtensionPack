namespace DDDotNetExtensionPackTests.Expressions
{
    public partial class ParameterReplacerTests
    {
        private class User
        {
            public int Age;

            public readonly bool Authorized = false;

            public string UserName { get; set; }
            public string Password { get; set; }

            public string GetUserName()
            {
                return UserName;
            }

            public string GetConcatedUserName(string postfix)
            {
                return $"{UserName}{postfix}";
            }

            public string GetConcatedUserName(User u)
            {
                return $"{UserName}{u.UserName}";
            }

            public string GetPassword()
            {
                return Password;
            }
        }

        private class Employee
        {
            public int Age;

            public string UserName { get; set; }

            public string GetUserName()
            {
                return UserName;
            }

            public string GetConcatedUserName(string postfix)
            {
                return $"{UserName}{postfix}";
            }

            public string GetConcatedUserName(Employee e)
            {
                return $"{UserName}{e.UserName}";
            }
        }
    }
}