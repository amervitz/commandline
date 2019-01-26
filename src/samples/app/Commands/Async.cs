using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Commands
{
    public class Async
    {
        public static async Task SleepAsync(int seconds)
        {
            await Task.Delay(seconds * 1000);
        }

        public static async Task<int> SleepAsyncReturnMilliseconds(int seconds)
        {
            await Task.Delay(seconds * 1000);
            return seconds * 1000;
        }
    }
}
