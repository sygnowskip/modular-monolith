using System;
using System.Threading.Tasks;

namespace ModularMonolith.Tests.Common
{
    public static class Try
    {
        public static async Task<bool> UntilSuccess(Func<Task<bool>> asyncAction, int limit, TimeSpan delay)
        {
            for (var i = 0; i < limit; i++)
            {
                if (await asyncAction())
                    return true;

                await Task.Delay(delay);
            }

            return false;
        }
    }
}