using RTU_TC.RTUScheduleClient;
using RTU_TC.RTUScheduleClient.ICal;

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

await foreach (var item in scheduleClient.GetAllSchedulesAsync("Радус"))
{
    Console.WriteLine(item.TargetTitle);
    var cal = await item.GetCalendarAsync();
    foreach (var lesson in cal.GetSchedulePeriodTypeLessons(SchedulePeriodType.Semester))
    {
        Console.WriteLine($"{lesson.Id} {lesson.Start} {lesson.Discipline}");
        foreach (var auditorium in lesson.Auditoriums)
        {
            Console.WriteLine($"- {auditorium.Title}|{auditorium.Number}|{auditorium.Campus ?? "NULL"}");
        }
    }
    break;
}

// Тестирование слияния расписаний
Ical.Net.Calendar calendar = null;
await foreach (var item in scheduleClient.GetAllGroupSchedulesAsync("ИКМО-01-2"))
{
    var cal = await item.GetCalendarAsync();
    var calRaw = (cal as IICalScheduleCalendar).ICalCalendarRaw;
    if (calendar == null)
    {
        calendar = calRaw;
    }
    else
    {
        calendar.Events.AddRange(calRaw.Events);
    }
}

Console.WriteLine(calendar.Events.Count);