using RTU_TC.RTUScheduleClient;

using var client = new HttpClient
{
    BaseAddress = new Uri("https://schedule-of.mirea.ru"),
};
client.DefaultRequestHeaders.Add(HttpSearchICalContentRTUSchedule.ClientNameHeaderKey, "schedule-client-sandbox");
var scheduleClient = new HttpSearchICalContentRTUSchedule(client);

await foreach (var item in scheduleClient.GetAllTeacherSchedulesAsync())
{
    Console.WriteLine(item.TargetTitle);
    break;
}

await foreach (var item in scheduleClient.GetMatchingGroupScheduleAsync("ИМБО-01-21"))
{
    Console.WriteLine(item.ScheduleTarget);
    Console.WriteLine(item.TargetTitle);
    var target = await item.GetCalendarAsync();
    foreach (var it2 in target.GetAllLessons())
    {
        Console.WriteLine(it2.Discipline);
    }
}

