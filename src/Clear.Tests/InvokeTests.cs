using App;
using App.Commands;
using Xunit;

namespace Clear.Tests
{
    public class InvokeTests
    {
        [Fact]
        public void CalculatorAddWithUnnamedParameters()
        {
            var args = "add 1 2".Split(' ');
            var output = (int)Router.Invoke(typeof(Calculator), args);
            Assert.Equal(3, output);
        }

        [Fact]
        public void CalculatorAddWithNamedParameters()
        {
            var args = "add --first 1 --second 2".Split(' ');
            var output = (int)Router.Invoke(typeof(Calculator), args);
            Assert.Equal(3, output);
        }

        [Fact]
        public void CalculatorAddDelegateWithNamedParameters()
        {
            var args = "--first 1 --second 2".Split(' ');
            // run a method with 2 int parameters and an int return value
            var output = Router.Invoke<int?, int, int>(Calculator.Add, args);
            Assert.Equal(3, output);
        }

        [Fact]
        public void VoidReturnType()
        {
            var args = "".Split(' ');
            Router.Invoke(Types.Void, args);
        }

        [Fact]
        public void Subcommands()
        {
            var args = "calculator add --first 1 --second 2".Split(' ');
            // run commands and subcommands in a namespace
            var output = Router.Invoke("App.Commands", args, typeof(Program).Assembly);
            Assert.Equal(3, output);
        }
    }
}
