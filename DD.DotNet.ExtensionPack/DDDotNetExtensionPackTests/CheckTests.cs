using System.Text;
using NUnit.Framework;

namespace DDDotNetExtensionPackTests
{
    [TestFixture]
    public partial class CheckTests
    {
        private const string _defaultParamNullRefExMsg = "Value cannot be null.";

        private string BuildExpectedMessage(string paramName, string errorMessgae = null)
        {
            errorMessgae = errorMessgae ?? _defaultParamNullRefExMsg;

            var sb = new StringBuilder();
            sb.AppendLine(errorMessgae);
            sb.Append($"Parameter name: {paramName}");

            return sb.ToString();
        }
    }
}
