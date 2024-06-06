using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using RTU_TC.RTUScheduleClient.ICal;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RTU_TC.RTUScheduleClient;

public partial class ICalScheduleLesson : IScheduleLesson
{
    public ICalScheduleLesson(Period period, CalendarEvent calendarEvent)
    {
        Start = period.StartTime.AsDateTimeOffset;
        End = period.EndTime.AsDateTimeOffset;
        Discipline = calendarEvent.Properties.Get<string>("X-META-DISCIPLINE");
        LessonType = calendarEvent.Properties.Get<string>("X-META-LESSON_TYPE");
        ScheduleVersionId = int.Parse(calendarEvent.Properties.Get<string>("X-SCHEDULE_VERSION-ID"));

        Groups = calendarEvent.Properties.AllOf("X-META-GROUP")
        .Select(p =>
        {
            var groupId = long.Parse(p.Parameters.Get("ID"), CultureInfo.InvariantCulture);
            return new ScheduleGroup(groupId, p.Value.ToString()!);
        })
        .ToArray();

        Auditoriums = calendarEvent.Properties.AllOf("X-META-AUDITORIUM")
        .Select(p =>
        {
            return new ScheduleAuditorium{
                Id = long.Parse(p.Parameters.Get("ID"), CultureInfo.InvariantCulture),
                Title = p.Value.ToString()!,
                Number = p.Parameters.Get("NUMBER"),
                Campus = p.Parameters.Get("CAMPUS"),
            };
        })
        .ToArray();

        Teachers = calendarEvent.Properties.AllOf("X-META-TEACHER")
        .Select(p =>
        {
            var teacherId = long.Parse(p.Parameters.Get("ID"), CultureInfo.InvariantCulture);
            return new ScheduleTeacher(teacherId, p.Value.ToString()!);
        })
        .ToArray();

        SubGroups = SubGroupsFromPpsExtractor.ExtractSubGroups(calendarEvent.Properties.Get<string>("SUMMARY"));
    }

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
