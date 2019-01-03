using Clear.Arguments;
using System.Linq;
using Xunit;

namespace Clear.Tests
{
    public class ArgumentParserTests
    {
        [Fact]
        public void ShouldContainNoArguments()
        {
            var args = new string[] { };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.Empty(parsedArguments);
        }

        [Fact]
        public void ShouldContainSingleNamedArgument()
        {
            var args = new string[] { "--name", "value" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.Single(parsedArguments);
            Assert.IsType<NamedArgument>(parsedArguments.First());
        }

        [Fact]
        public void ShouldContainSingleAnonymousArgument()
        {
            var args = new string[] { "value" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.Single(parsedArguments);
            Assert.IsType<AnonymousArgument>(parsedArguments.First());
        }

        [Fact]
        public void DuplicateNamedArgumentShouldBeRemoved()
        {
            var args = new string[] { "--name", "value1", "--name", "value2" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.Single(parsedArguments);

            var parameter = parsedArguments.First();
            Assert.IsType<NamedArgument>(parameter);
            Assert.Equal("value1", parameter.Value);
        }

        [Fact]
        public void DuplicateNamedArgumentShouldBeKept()
        {
            var args = new string[] { "--name", "value1", "--name", "value2" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args, false);

            Assert.True(parsedArguments.Count == 2);
        }

        [Fact]
        public void ShouldContainTwoNamedArguments()
        {
            var args = new string[] { "--name1", "value1", "--name2", "value2" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.True(parsedArguments.Count == 2);
            Assert.All(parsedArguments, p => Assert.IsType<NamedArgument>(p));
        }

        [Fact]
        public void MissingNamedParamterValueShouldBeEmpty()
        {
            var args = new string[] { "--name1", "--name2", "value2" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.True(parsedArguments.Count == 2);
            Assert.All(parsedArguments, p => Assert.IsType<NamedArgument>(p));
        }

        [Fact]
        public void MultipleMissingNamedParamterValuesShouldBeEmpty()
        {
            var args = new string[] { "--name1", "--name2" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.True(parsedArguments.Count == 2);
            Assert.All(parsedArguments, p => Assert.IsType<NamedArgument>(p));
        }

        [Fact]
        public void EscapedArgumentValueShouldBeHandled()
        {
            var args = new string[] { "--name1", "----value1" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.True(parsedArguments.Count == 1);
            Assert.Equal("--value1", parsedArguments[0].Value);
        }

        [Fact]
        public void EscapedAnonymousValueShouldBeHandled()
        {
            var args = new string[] { "----value" };
            var parser = new ArgumentParser();
            var parsedArguments = parser.Parse(args);

            Assert.True(parsedArguments.Count == 1);
            Assert.IsType<AnonymousArgument>(parsedArguments[0]);
            Assert.Equal("--value", parsedArguments[0].Value);
        }

        [Fact]
        public void Escape()
        {
            Assert.Equal("value", ArgumentParser.Escape("value"));
            Assert.Equal("-value", ArgumentParser.Escape("-value"));
            Assert.Equal("----value", ArgumentParser.Escape("--value"));
            Assert.Equal("-----value", ArgumentParser.Escape("---value"));
            Assert.Equal("--------value", ArgumentParser.Escape("----value"));
        }

        [Fact]
        public void Unescape()
        {
            Assert.Equal("value", ArgumentParser.Unescape("value"));
            Assert.Equal("-value", ArgumentParser.Unescape("-value"));
            Assert.Equal("--value", ArgumentParser.Unescape("--value"));
            Assert.Equal("---value", ArgumentParser.Unescape("---value"));
            Assert.Equal("--value", ArgumentParser.Unescape("----value"));
            Assert.Equal("---value", ArgumentParser.Unescape("-----value"));
            Assert.Equal("----value", ArgumentParser.Unescape("------value"));
            Assert.Equal("-----value", ArgumentParser.Unescape("-------value"));
            Assert.Equal("----value", ArgumentParser.Unescape("--------value"));
            Assert.Equal("-----value", ArgumentParser.Unescape("---------value"));
        }

        [Fact]
        public void AllValuesStringArray()
        {
            var args = new string[] { "value1", "value2" };
            var parser = new ArgumentParser();
            var collection = parser.Parse(args);
            Assert.Equal(args, collection.ToStringArray());
        }

        [Fact]
        public void AllEmptyArgumentsStringArray()
        {
            var args = new string[] { "--name1", "--name2" };
            var parser = new ArgumentParser();
            var collection = parser.Parse(args);
            Assert.Equal(args, collection.ToStringArray());
        }

        [Fact]
        public void AllEmptyArgumentsString()
        {
            var args = new string[] { "--name1", "--name2" };
            var parser = new ArgumentParser();
            var collection = parser.Parse(args);
            Assert.Equal("--name1 --name2", collection.ToString());
        }

        [Fact]
        public void NamedArgumentsWithValuesStringArray()
        {
            var args = new string[] { "--name1", "value1", "--name2", "value2" };
            var parser = new ArgumentParser();
            var collection = parser.Parse(args);
            Assert.Equal(args, collection.ToStringArray());
        }

        [Fact]
        public void NamedArgumentsWithValuesString()
        {
            var args = new string[] { "--name1", "value1", "--name2", "value2" };
            var parser = new ArgumentParser();
            var collection = parser.Parse(args);
            Assert.Equal("--name1 value1 --name2 value2", collection.ToString());
        }

        [Fact]
        public void NamedArgumentsWithEscapedValuesStringArray()
        {
            var args = new string[] { "--name1", "----value1", "--name2", "-----value2" };
            var parser = new ArgumentParser();
            var collection = parser.Parse(args);
            Assert.Equal(args, collection.ToStringArray());
        }

        [Fact]
        public void NamedArgumentsWithEscapedValuesString()
        {
            var args = new string[] { "--name1", "----value1", "--name2", "-----value2" };
            var parser = new ArgumentParser();
            var collection = parser.Parse(args);
            Assert.Equal("--name1 ----value1 --name2 -----value2", collection.ToString());
        }
    }
}
