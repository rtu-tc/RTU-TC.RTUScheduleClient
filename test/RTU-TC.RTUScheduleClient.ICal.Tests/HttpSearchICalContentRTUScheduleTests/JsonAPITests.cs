using FluentAssertions;
using RichardSzalay.MockHttp;

namespace RTU_TC.RTUScheduleClient.ICal.Tests.HttpSearchICalContentRTUScheduleTests;
public class JsonAPITests
{
    [Fact]
    public async Task NoSchedulesIsEmptyCollection()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://localhost/schedule/api/search")
                .Respond("application/json", """{ "data" : [] }""");

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("http://localhost");
        client.DefaultRequestHeaders.Add(HttpSearchICalContentRTUSchedule.ClientNameHeaderKey, "tests");
        var scheduleClient = new HttpSearchICalContentRTUSchedule(client);

        var schedules = await scheduleClient.GetAllSchedulesAsync().ToArrayAsync();

        schedules.Should().BeEmpty();
    }

    [Fact]
    public async Task OneSchedulesIsOneScheduleReturns()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://localhost/schedule/api/search")
                .Respond("application/json", """
                { "data" : [ {
                    "id": 1,
                    "targetTitle": "2 (С-20)",
                    "scheduleTarget": 3,
                    "iCalLink": "ignore-it",
                    "scheduleImageLink": "ignore-it",
                    "scheduleUpdateImageLink": "ignore-it"
                } ] }
                """);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("http://localhost");
        client.DefaultRequestHeaders.Add(HttpSearchICalContentRTUSchedule.ClientNameHeaderKey, "tests");
        var scheduleClient = new HttpSearchICalContentRTUSchedule(client);

        var schedules = await scheduleClient.GetAllSchedulesAsync().ToArrayAsync();

        schedules.Should().HaveCount(1);
        var schedule = schedules[0];
        schedule.Id.Should().Be(1);
        schedule.TargetTitle.Should().Be("2 (С-20)");
        schedule.ScheduleTarget.Should().Be(ScheduleTarget.Auditorium);
    }


    [Fact]
    public async Task TwoPagesHandleCorrect()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://localhost/schedule/api/search")
                .WithQueryString("pageToken", "")
                .Respond("application/json", """
                { "data" : [ {
                    "id": 1,
                    "targetTitle": "2 (С-20)",
                    "scheduleTarget": 3,
                    "iCalLink": "ignore-it",
                    "scheduleImageLink": "ignore-it",
                    "scheduleUpdateImageLink": "ignore-it"
                } ],
                "nextPageToken": "tokenValue"
                }
                """);

        mockHttp.When("http://localhost/schedule/api/search")
                .WithQueryString("pageToken", "tokenValue")
                .Respond("application/json", """
                { "data" : [ {
                    "id": 2,
                    "targetTitle": "Группа",
                    "scheduleTarget": 1,
                    "iCalLink": "ignore-it",
                    "scheduleImageLink": "ignore-it",
                    "scheduleUpdateImageLink": "ignore-it"
                } ],
                "nextPageToken": null
                }
                """);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("http://localhost");
        client.DefaultRequestHeaders.Add(HttpSearchICalContentRTUSchedule.ClientNameHeaderKey, "tests");
        var scheduleClient = new HttpSearchICalContentRTUSchedule(client);

        var schedules = await scheduleClient.GetAllSchedulesAsync().ToArrayAsync();

        schedules.Should().HaveCount(2);
    }


}
