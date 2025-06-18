using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RTU_TC.RTUScheduleClient;

public partial class ICalScheduleLesson : IScheduleLesson
{
    public ICalScheduleLesson(Period period, CalendarEvent calendarEvent, string? timeZoneId)
    {
        Id = calendarEvent.Uid
            ?? throw new InvalidDataException($"No UID in event {period}"); ;

        // FIXME: крайне плохое поведение, нужно или добиться от библиотеки выдачи DateTimeOffset, либо найти иное решение
        Start = DateTimeOffset.Parse(period.StartTime.ToString("O").Replace(timeZoneId ?? "", "").Trim());
        End = DateTimeOffset.Parse(period.EffectiveEndTime?.ToString("O").Replace(timeZoneId ?? "", "").Trim() ?? throw new InvalidDataException($"No end in period {period} of event {calendarEvent.Uid}"));

        Discipline = calendarEvent.Properties.Get<string>("X-META-DISCIPLINE")
            ?? throw new InvalidDataException($"No discipline in event {calendarEvent.Uid}");
        LessonType = calendarEvent.Properties.Get<string>("X-META-LESSON_TYPE")
            ?? throw new InvalidDataException($"No lesson type in event {calendarEvent.Uid}"); ;
        ScheduleVersionId = int.Parse(calendarEvent.Properties.Get<string>("X-SCHEDULE_VERSION-ID")
            ?? throw new InvalidDataException($"No schedule version id in event {calendarEvent.Uid}"));

        Groups = [.. calendarEvent.Properties.AllOf("X-META-GROUP")
        .Select(p =>
        {
            var groupId = long.Parse(p.Parameters.Get("ID")
                ?? throw new InvalidDataException($"No group id in property {p} of event {calendarEvent.Uid}")
                , CultureInfo.InvariantCulture);
            return new ScheduleGroup(groupId, p.Value?.ToString() ?? throw new InvalidDataException($"No group value in property {p} of event {calendarEvent.Uid}"));
        })];

        Auditoriums = [.. calendarEvent.Properties.AllOf("X-META-AUDITORIUM")
        .Select(p =>
        {
            return new ScheduleAuditorium{
                Id = long.Parse(p.Parameters.Get("ID")
                    ?? throw new InvalidDataException($"No auditorium id in property {p} of event {calendarEvent.Uid}")
                , CultureInfo.InvariantCulture),
                Title = p.Value?.ToString() ?? throw new InvalidDataException($"No auditorium value in property {p} of event {calendarEvent.Uid}"),
                Number = p.Parameters.Get("NUMBER") ?? throw new InvalidDataException($"No auditorium number in property {p} of event {calendarEvent.Uid}"),
                Campus = p.Parameters.Get("CAMPUS"),
            };
        })];

        Teachers = [.. calendarEvent.Properties.AllOf("X-META-TEACHER")
        .Select(p =>
        {
            var teacherId = long.Parse(p.Parameters.Get("ID")
                ?? throw new InvalidDataException($"No teacher id in property {p} of event {calendarEvent.Uid}"), CultureInfo.InvariantCulture);
            return new ScheduleTeacher(teacherId, p.Value?.ToString() ?? throw new InvalidDataException($"No teacher value in property {p} of event {calendarEvent.Uid}"));
        })];

        SubGroups = SubGroupsFromPpsExtractor.ExtractSubGroups(calendarEvent.Properties.Get<string>("SUMMARY"));
    }

    public string Id { get; }

    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }

    public string Discipline { get; }
    public string LessonType { get; }
    public int ScheduleVersionId { get; }

    public IReadOnlyCollection<ScheduleGroup> Groups { get; }
    public IReadOnlyCollection<ScheduleAuditorium> Auditoriums { get; }
    public IReadOnlyCollection<ScheduleTeacher> Teachers { get; }
    public IReadOnlyCollection<int> SubGroups { get; }
}

public static partial class SubGroupsFromPpsExtractor
{
    private static readonly Regex _subGroupRegex = GetSubGroupsRegex();

    public static int[] ExtractSubGroups(string? row)
    {
        if (string.IsNullOrEmpty(row))
        {
            return [];
        }
        var match = _subGroupRegex.Matches(row);
        if (match.Count == 0)
        {
            return [];
        }
        return [.. match
            .Select(m => int.Parse(m.Groups["subgroup"].Value, CultureInfo.InvariantCulture))
            .Distinct()
            .Order()];
    }

    public static string CleanSubGroups(string row) => _subGroupRegex.Replace(row, "");

    [GeneratedRegex(@"(?<subgroup>\d+) *п?(\\|\/)*г,?")]
    private static partial Regex GetSubGroupsRegex();
}
