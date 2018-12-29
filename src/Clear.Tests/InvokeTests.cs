using App;
using App.Commands;
using Xunit;

namespace Clear.Tests
{
    public class InvokeTests
    {
        [Fact]
        public void CommandWithAnonymousArguments()
        {
            var args = "add 1 2".Split(' ');
            var output = (int)Router.Invoke(typeof(Calculator), args);
            Assert.Equal(3, output);
        }

        [Fact]
        public void CommandWithNamedArguments()
        {
            var args = "add --first 1 --second 2".Split(' ');
            var output = (int)Router.Invoke(typeof(Calculator), args);
            Assert.Equal(3, output);
        }

        [Fact]
        public void DelegateWithNamedArguments()
        {
            var args = "--first 1 --second 2".Split(' ');
            // run a method with 2 int parameters and an int return value
            var output = Router.Invoke<int?, int, int>(Calculator.Add, args);
            Assert.Equal(3, output);
        }

        [Fact]
        public void VoidReturnType()
        {
            var args = new string[] { };
            Router.Invoke(Types.Void, args);
        }

        [Fact]
        public void ClassWithCommand()
        {
            var args = "calculator add --first 1 --second 2".Split(' ');
            // run commands and subcommands in a namespace
            var output = Router.Invoke("App.Commands", args, typeof(Program).Assembly);
            Assert.Equal(3, output);
        }

        [Fact]
        public void ExtraUnknownNamedArgumentShouldFail()
        {
            var args = "calculator add --first 1 --second 2 --doesnotexist 3".Split(' ');
            // run commands and subcommands in a namespace
            var output = Router.Invoke("App.Commands", args, typeof(Program).Assembly);

            Assert.Null(output);
        }

        [Fact]
        public void ExtraUnknownAnonymousArgumentShouldFail()
        {
            var args = "calculator add --first 1 --second 2 doesnotexist".Split(' ');
            // run commands and subcommands in a namespace
            var output = Router.Invoke("App.Commands", args, typeof(Program).Assembly);

            Assert.Null(output);
        }

        [Fact]
        public void UnknownArgumentShouldFail()
        {
            var args = "calculator add --doesnotexist 1 --second 2".Split(' ');

            // run commands and subcommands in a namespace
            var output = Router.Invoke("App.Commands", args, typeof(Program).Assembly);

            Assert.Null(output);
        }

        [Fact]
        public void UnknownCommandShouldFail()
        {
            var args = "unknown 1 2".Split(' ');
            var output = Router.Invoke(typeof(Calculator), args);
            Assert.Null(output);
        }

        [Fact]
        public void NamespaceSubcommand()
        {
            var args = "internet web google search --query term".Split(' ');
            var output = Router.Invoke("App.Commands", args, typeof(Program).Assembly);
            Assert.Equal("https://www.google.com/search?q=term", output);
        }
    }
}
