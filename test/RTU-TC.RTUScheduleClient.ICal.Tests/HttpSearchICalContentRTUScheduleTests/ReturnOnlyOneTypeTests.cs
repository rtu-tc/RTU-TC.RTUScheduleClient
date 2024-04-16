using FluentAssertions;
using RichardSzalay.MockHttp;

namespace RTU_TC.RTUScheduleClient.ICal.Tests.HttpSearchICalContentRTUScheduleTests;
public class ReturnOnlyOneTypeTests
{
    [Fact]
    public async Task OnlyGroupSchedulesCorrect()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://localhost/schedule/api/search")
                .Respond("application/json", ExampleSearchResponse);
        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("http://localhost");
        client.DefaultRequestHeaders.Add(HttpSearchICalContentRTUSchedule.ClientNameHeaderKey, "tests");
        var scheduleClient = new HttpSearchICalContentRTUSchedule(client);

        var schedules = await scheduleClient.GetAllGroupSchedulesAsync().ToArrayAsync();

        schedules.Should().HaveCount(1);
        var schedule = schedules[0];
        schedule.TargetTitle.Should().Be("Группа");
        schedule.ScheduleTarget.Should().Be(ScheduleTarget.Group);
    }

    [Fact]
    public async Task OnlyTeacherSchedulesCorrect()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://localhost/schedule/api/search")
                .Respond("application/json", ExampleSearchResponse);
        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("http://localhost");
        client.DefaultRequestHeaders.Add(HttpSearchICalContentRTUSchedule.ClientNameHeaderKey, "tests");
        var scheduleClient = new HttpSearchICalContentRTUSchedule(client);

        var schedules = await scheduleClient.GetAllTeacherSchedulesAsync().ToArrayAsync();

        schedules.Should().HaveCount(1);
        var schedule = schedules[0];
        schedule.TargetTitle.Should().Be("Преподаватель");
        schedule.ScheduleTarget.Should().Be(ScheduleTarget.Teacher);
    }

    [Fact]
    public async Task OnlyAuditoriumSchedulesCorrect()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://localhost/schedule/api/search")
                .Respond("application/json", ExampleSearchResponse);
        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("http://localhost");
        client.DefaultRequestHeaders.Add(HttpSearchICalContentRTUSchedule.ClientNameHeaderKey, "tests");
        var scheduleClient = new HttpSearchICalContentRTUSchedule(client);

        var schedules = await scheduleClient.GetAllAuditoriumSchedulesAsync().ToArrayAsync();

        schedules.Should().HaveCount(1);
        var schedule = schedules[0];
        schedule.TargetTitle.Should().Be("Аудитория");
        schedule.ScheduleTarget.Should().Be(ScheduleTarget.Auditorium);
    }

    private const string ExampleSearchResponse = """
        { "data" : [
        {
            "id": 1,
            "targetTitle": "Аудитория",
            "scheduleTarget": 3,
            "iCalLink": "ignore-it",
            "scheduleImageLink": "ignore-it",
            "scheduleUpdateImageLink": "ignore-it"
        },
        {
            "id": 1,
            "targetTitle": "Группа",
            "scheduleTarget": 1,
            "iCalLink": "ignore-it",
            "scheduleImageLink": "ignore-it",
            "scheduleUpdateImageLink": "ignore-it"
        },
        {
            "id": 1,
            "targetTitle": "Преподаватель",
            "scheduleTarget": 2,
            "iCalLink": "ignore-it",
            "scheduleImageLink": "ignore-it",
            "scheduleUpdateImageLink": "ignore-it"
        }
        ] }
        """;
}
