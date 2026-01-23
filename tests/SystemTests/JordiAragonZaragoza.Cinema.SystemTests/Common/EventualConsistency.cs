namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System;
    using System.Threading.Tasks;

    public static class EventualConsistency
    {
        public static async Task WaitUntilAsync(
            Func<Task<bool>> condition,
            TimeSpan? timeout = null)
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            var limit = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(10));

            while (DateTime.UtcNow < limit)
            {
                if (await condition())
                {
                    return;
                }

                await Task.Delay(200);
            }

            throw new TimeoutException("Eventual consistency condition not satisfied");
        }
    }
}