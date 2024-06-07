using RTU_TC.RTUScheduleClient;

using var client = new HttpClient
{
    BaseAddress = new Uri("https://schedule-of.mirea.ru"),
};
client.DefaultRequestHeaders.Add(HttpSearchICalContentRTUSchedule.ClientNameHeaderKey, "schedule-client-sandbox");
var scheduleClient = new HttpSearchICalContentRTUSchedule(client);

await foreach (var item in scheduleClient.GetAllAuditoriumSchedulesAsync("А-18"))
{
    Console.WriteLine(item.TargetTitle);
    break;
}

await foreach (var item in scheduleClient.GetAllSchedulesAsync())
{
    Console.WriteLine(item.TargetTitle);
    var cal = await item.GetCalendarAsync();
    foreach (var lesson in cal.GetAllLessons())
    {
        Console.WriteLine(lesson.Discipline);
        foreach (var auditorium in lesson.Auditoriums)
        {
            Console.WriteLine($"- {auditorium.Title}|{auditorium.Number}|{auditorium.Campus ?? "NULL"}");
        }
    }
    break;
}
