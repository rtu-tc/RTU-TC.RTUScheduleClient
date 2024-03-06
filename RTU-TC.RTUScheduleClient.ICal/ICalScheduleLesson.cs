using Ical.Net.CalendarComponents;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RTU_TC.RTUScheduleClient;

public partial record ICalScheduleLesson(Ical.Net.DataTypes.Period Period, CalendarEvent CalendarEvent) : IScheduleLesson
{
    private static readonly Regex audNameRegex = GetAuditoriumValuesRegex();
    private static readonly Regex subgroupRegex = GetSubGroupRegex();

    public DateTimeOffset Start { get; } = Period.StartTime.AsDateTimeOffset;
    public DateTimeOffset End { get; } = Period.EndTime.AsDateTimeOffset;

    public string Discipline { get; } = CalendarEvent.Properties.Get<string>("X-META-DISCIPLINE");
    public string LessonType { get; } = CalendarEvent.Properties.Get<string>("X-META-LESSON_TYPE");
    
    public IReadOnlyCollection<ScheduleGroup> Groups { get; } = CalendarEvent.Properties.AllOf("X-META-GROUP")
        .Select(p =>
        {
            var groupId = long.Parse(p.Parameters.Get("ID"), CultureInfo.InvariantCulture);
            return new ScheduleGroup(groupId, p.Value.ToString()!);
        })
        .ToArray();
    public IReadOnlyCollection<ScheduleAuditorium> Auditoriums { get; } = CalendarEvent.Properties.AllOf("X-META-AUDITORIUM")
        .Select(p =>
        {
            var audMatch = audNameRegex.Match(p.Value.ToString()!);
            var audId = long.Parse(p.Parameters.Get("ID"), CultureInfo.InvariantCulture);
            return new ScheduleAuditorium(
                audId,
                audMatch.Groups["title"].Value,
                audMatch.Groups["campus"].Value
            );
        })
        .ToArray();
    public IReadOnlyCollection<ScheduleTeacher> Teachers { get; } = CalendarEvent.Properties.AllOf("X-META-TEACHER")
        .Select(p =>
        {
            var teacherId = long.Parse(p.Parameters.Get("ID"), CultureInfo.InvariantCulture);
            return new ScheduleTeacher(teacherId, p.Value.ToString()!);
        })
        .ToArray();

    [GeneratedRegex(@"\d\s*п\W*г")]
    private static partial Regex GetSubGroupRegex();
    [GeneratedRegex(@"^(?<title>.+) *\((?<campus>.+)\)$", RegexOptions.Compiled)]
    private static partial Regex GetAuditoriumValuesRegex();
}
