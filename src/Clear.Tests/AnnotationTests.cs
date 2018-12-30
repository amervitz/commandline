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
            var output = Router.Invoke<int, int, string>(Annotations.ParameterNameOverride, args);
            Assert.Equal("12", output);
        }

        [Fact]
        public void OriginalParameterNamesWhenOverridenAreNotUsed()
        {
            var args = new string[] { "--param1", "1", "--param2", "2" };
            var output = Router.Invoke<int, int, string>(Annotations.ParameterNameOverride, args);
            Assert.Null(output);
        }
    }
}
