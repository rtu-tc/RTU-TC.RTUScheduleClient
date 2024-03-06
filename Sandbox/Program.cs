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

