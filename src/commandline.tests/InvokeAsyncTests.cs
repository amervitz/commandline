using App;
using App.Commands;
using System.Threading.Tasks;
using Xunit;

namespace amervitz.commandline.Tests
{
    public class InvokeAsyncTests
    {
        [Fact]
        public void AsyncTask()
        {
            var args = "async sleepasync --seconds 1".Split(' ');
            var output = Router.Invoke("App.Commands", args, typeof(Program).Assembly);
            Assert.Equal("System.Threading.Tasks.VoidTaskResult", output.GetType().FullName);
        }

        [Fact]
        public void AsyncTaskWithResult()
        {
            var args = "async sleepasyncreturnmilliseconds --seconds 2".Split(' ');
            var output = Router.Invoke("App.Commands", args, typeof(Program).Assembly);
            Assert.Equal(2000, output);
        }

        [Fact]
        public async Task AsyncActionTask()
        {
            var args = "sleepasync --seconds 1".Split(' ');
            await Router.InvokeAsync<int>(app.Commands.Async.SleepAsync, args);
        }

        [Fact]
        public void DelegateWithNamedArguments()
        {
            var args = "--first 1 --second 2".Split(' ');
            // run a method with 2 int parameters and an int return value
            var output = Router.Invoke<int?, int, int>(Calculator.Add, args);
            Assert.Equal(3, output);
        }
    }
}
