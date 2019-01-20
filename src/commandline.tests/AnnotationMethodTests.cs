using amervitz.commandline.Tests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace amervitz.commandline.Tests
{
    public class AnnotationMethodTests
    {
        [Fact]
        public void OverriddenMethodNameIsUsed()
        {
            var args = new string[] { "OverriddenMethodName", "--param1", "1" };
            var output = Router.Invoke(typeof(Annotations), args);
            Assert.Equal("1", output);
        }

        [Fact]
        public void OriginalMethodNameWhenOverriddenIsNotUsed()
        {
            var args = new string[] { nameof(Annotations.MethodNameOverridden), "--param1", "1" };
            var output = Router.Invoke(typeof(Annotations), args);
            Assert.Null(output);
        }
    }
}
