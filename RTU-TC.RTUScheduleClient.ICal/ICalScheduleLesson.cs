using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RTU_TC.RTUScheduleClient;

public partial class ICalScheduleLesson : IScheduleLesson
{
    private static readonly Regex _audNameRegex = GetAuditoriumValuesRegex();
    private static readonly Regex _subgroupRegex = GetSubGroupRegex();

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
            var audMatch = _audNameRegex.Match(p.Value.ToString()!);
            var audId = long.Parse(p.Parameters.Get("ID"), CultureInfo.InvariantCulture);
            return new ScheduleAuditorium(
                audId,
                audMatch.Groups["title"].Value,
                audMatch.Groups["campus"].Value
            );
        })
        .ToArray();

        Teachers = calendarEvent.Properties.AllOf("X-META-TEACHER")
        .Select(p =>
        {
            var teacherId = long.Parse(p.Parameters.Get("ID"), CultureInfo.InvariantCulture);
            return new ScheduleTeacher(teacherId, p.Value.ToString()!);
        })
        .ToArray();
    }

    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }

    public string Discipline { get; }
    public string LessonType { get; }
    public int ScheduleVersionId { get; }

    public IReadOnlyCollection<ScheduleGroup> Groups { get; }
    public IReadOnlyCollection<ScheduleAuditorium> Auditoriums { get; }
    public IReadOnlyCollection<ScheduleTeacher> Teachers { get; }

    [GeneratedRegex(@"\d\s*п\W*г")]
    private static partial Regex GetSubGroupRegex();
    [GeneratedRegex(@"^(?<title>.+)\s+\((?<campus>.+)\)$")]
    private static partial Regex GetAuditoriumValuesRegex();
}
