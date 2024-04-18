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

await foreach (var item in scheduleClient.GetAllGroupSchedulesAsync())
{
    var cal = await item.GetCalendarAsync();
    var check = cal.GetAllLessons();
    foreach (var l in check)
    {
        //Console.WriteLine(l.Discipline);
        //Подгруппы
        foreach (var r in l.SubGroups)
        {
            Console.WriteLine(r);
        }
    }
}

