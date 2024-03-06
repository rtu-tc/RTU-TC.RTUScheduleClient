namespace RTU_TC.RTUScheduleClient.ICal;
internal static class Extensions
{
    public static async IAsyncEnumerable<T> WhereAsync<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate)
    {
        await foreach (var item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }
}
