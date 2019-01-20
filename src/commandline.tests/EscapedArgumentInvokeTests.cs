using amervitz.commandline.Tests.Commands;
using Xunit;

namespace amervitz.commandline.Tests
{
    public class EscapedArgumentInvokeTests
    {
        [Fact]
        public void LongArgumentValuesAreUnescaped()
        {
            var args = new string[] { "--p1", "----a1", "--p2", "----a2" };
            var output = Router.Invoke<string, string, string>(Annotations.ParameterNameOverride, args);
            Assert.Equal("--a1--a2", output);
        }

        [Fact]
        public void ShortArgumentValuesAreUnescaped()
        {
            var args = new string[] { "-p", "--a", "-q", "--b" };
            var output = Router.Invoke<string, string, string>(Annotations.ParameterShortNameOverride, args);
            Assert.Equal("-a-b", output);
        }
    }
}
