using Clear.Tests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Clear.Tests
{
    public class AnnotationTests
    {
        [Fact]
        public void OverriddenParameterNamesAreUsed()
        {
            var args = new string[] { "--p1", "1", "--p2", "2" };
            var output = Router.Invoke<string, string, string>(Annotations.ParameterNameOverride, args);
            Assert.Equal("12", output);
        }

        [Fact]
        public void OriginalParameterNamesWhenOverridenAreNotUsed()
        {
            var args = new string[] { "--param1", "1", "--param2", "2" };
            var output = Router.Invoke<string, string, string>(Annotations.ParameterNameOverride, args);
            Assert.Null(output);
        }

        [Fact]
        public void OverriddenParameterShortNamesAreUsed()
        {
            var args = new string[] { "-p", "1", "-q", "2" };
            var output = Router.Invoke<string, string, string>(Annotations.ParameterShortNameOverride, args);
            Assert.Equal("12", output);
        }

        [Fact]
        public void ShortNameAndNameParameterOverridesWork()
        {
            var args = new string[] { "-p", "1", "--par2", "2" };
            var output = Router.Invoke<string, string, string>(Annotations.ParameterShortAndLongNameOverride, args);
            Assert.Equal("12", output);
        }

        [Fact]
        public void InvalidShortNameOverrideFails()
        {
            var args = new string[] { "-par1", "1", "--par2", "2" };
            var output = Router.Invoke<string, string, string>(Annotations.ParameterShortAndLongNameOverride, args);
            Assert.Null(output);
        }

        [Fact]
        public void SameParameterMultipleOverridesAreNotUsed()
        {
            var args = new string[] { "-p", "1", "--par1", "2", "--par2", "3" };
            var output = Router.Invoke<string, string, string>(Annotations.ParameterShortAndLongNameOverride, args);
            Assert.Null(output);
        }
    }
}
