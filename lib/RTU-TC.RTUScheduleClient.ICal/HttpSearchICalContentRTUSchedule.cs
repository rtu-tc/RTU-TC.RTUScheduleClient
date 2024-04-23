using System.Diagnostics;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json.Serialization;
using RTU_TC.RTUScheduleClient.ICal;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace RTU_TC.RTUScheduleClient;

public class HttpSearchICalContentRTUSchedule : IRTUScheduleClient
{
    public const string ClientNameHeaderKey = "Client-Name";
    private readonly HttpClient _httpClient;

    public HttpSearchICalContentRTUSchedule(HttpClient httpClient)
    {
        if (!httpClient.DefaultRequestHeaders.Contains(ClientNameHeaderKey))
        {
            throw new NoClientNameProvidedException();
        }
        _httpClient = httpClient;
    }

    public IAsyncEnumerable<ISchedule> GetAllGroupSchedulesAsync(string? match = default, CancellationToken cancellationToken = default)
        => GetAllSchedulesAsync(match, cancellationToken).WhereAsync(g => g.ScheduleTarget == ScheduleTarget.Group);
    public IAsyncEnumerable<ISchedule> GetAllAuditoriumSchedulesAsync(string? match = default, CancellationToken cancellationToken = default)
        => GetAllSchedulesAsync(match, cancellationToken).WhereAsync(g => g.ScheduleTarget == ScheduleTarget.Auditorium);
    public IAsyncEnumerable<ISchedule> GetAllTeacherSchedulesAsync(string? match = default, CancellationToken cancellationToken = default)
        => GetAllSchedulesAsync(match, cancellationToken).WhereAsync(g => g.ScheduleTarget == ScheduleTarget.Teacher);

    public async IAsyncEnumerable<ISchedule> GetAllSchedulesAsync(
        string? match = default,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string? pageToken = null;
        do
        {
            var url = $"/schedule/api/search?limit=50&pageToken={WebUtility.UrlEncode(pageToken)}";
            if (!string.IsNullOrEmpty(match))
            {
                url += $"&match={WebUtility.UrlEncode(match)}";
            }
            var currentList = await _httpClient.GetFromJsonAsync<PaginationResponse>(url, cancellationToken: cancellationToken)
                ?? throw new UnreachableException("Can't retrieve schedule");
            foreach (var item in currentList.Schedules)
            {
                yield return new Schedule(_httpClient, item);
            }
            pageToken = currentList.NextPageToken;
        } while (pageToken is not null && !cancellationToken.IsCancellationRequested);
    }

    private class PaginationResponse
    {
        [JsonPropertyName("data")]
        public List<ScheduleDto> Schedules { get; set; } = [];
        [JsonPropertyName("nextPageToken")]
        public string? NextPageToken { get; set; }
    }

    private class Schedule(HttpClient httpClient, ScheduleDto dto) : ISchedule
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ScheduleDto _dto = dto;

        public int Id => _dto.Id;
        public string TargetTitle => _dto.TargetTitle;
        public ScheduleTarget ScheduleTarget => _dto.ScheduleTarget;

        public async Task<IScheduleCalendar> GetCalendarAsync(CancellationToken cancellationToken)
        {
            var delimiter = _dto.ICalLink.Contains('?') ? '&' : '?';
            var uri = $"{_dto.ICalLink}{delimiter}includeMeta=true";
            var calendar = Ical.Net.Calendar.Load(await _httpClient.GetStreamAsync(uri, cancellationToken));
            return new ICalCalendar(calendar);
        }
    }
    private class ScheduleDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("targetTitle")]
        public string TargetTitle { get; set; } = default!;
        [JsonPropertyName("scheduleTarget")]
        public ScheduleTarget ScheduleTarget { get; set; }
        [JsonPropertyName("iCalLink")]
        public string ICalLink { get; set; } = default!;
    }
}
public class NoClientNameProvidedException : Exception
{
    public NoClientNameProvidedException() : base($"You must setup default request header {HttpSearchICalContentRTUSchedule.ClientNameHeaderKey} in you own http client")
    { }
}
