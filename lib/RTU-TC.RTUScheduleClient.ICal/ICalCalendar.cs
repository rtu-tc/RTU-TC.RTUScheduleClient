using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System.Globalization;


namespace RTU_TC.RTUScheduleClient.ICal;

public partial class ICalCalendar(Ical.Net.Calendar Calendar) : IScheduleCalendar, IICalScheduleCalendar
{
    private readonly string _tzId = Calendar.TimeZones.Single().TzId
        ?? throw new InvalidDataException("Not found timezone id in calendar");
    public Ical.Net.Calendar ICalCalendarRaw => Calendar;

    public IEnumerable<IScheduleLesson> GetLessons(DateTimeOffset from, DateTimeOffset to)
    {
        var fromTime = new CalDateTime(from.UtcDateTime);
        var toTime = new CalDateTime(to.UtcDateTime);
        return Calendar
            // FIXME: GetOccurrences выдает значения вне интервала, некорректно обрабатывая границы
            .GetOccurrences<CalendarEvent>(fromTime)
            .TakeWhileBefore(toTime)
            .Select(occ => (occ.Period, Source: (occ.Source as CalendarEvent)!))
            .Where(t => t.Source.Transparency == TransparencyType.Opaque) // занятые = занятия. Не занятые - недели и т.д.
            .Select(occ => new ICalScheduleLesson(occ.Period, occ.Source, _tzId) as IScheduleLesson);
    }

    public IEnumerable<IScheduleLesson> GetAllLessons()
    {
        var scheduleVersions = GetScheduleVersions();
        if (!scheduleVersions.Any())
        {
            return [];
        }
        return GetLessons(scheduleVersions.Select(s => s.Start).Min(), scheduleVersions.Select(s => s.End).Max());
    }

    public IEnumerable<IScheduleVersion> GetScheduleVersions()
    {
        var scheduleVersions = Calendar.Children.AllOf("X-SCHEDULE-VERSION");
        var versions = new List<IScheduleVersion>();

        foreach (var version in scheduleVersions)
        {
            var properties = (version as ICalendarPropertyListContainer)!.Properties;
            SchedulePeriodType schedulePeriodType = SchedulePeriodType.Unknown;
            DateTimeOffset start = DateTimeOffset.MinValue;
            DateTimeOffset end = DateTimeOffset.MaxValue;
            int SvId = 0;
            foreach (var property in properties)
            {
                if (property.Value is null)
                {
                    throw new Exception($"Value of given property {property.Name} of schedule version is null");
                }
                switch (property.Name)
                {
                    case "SVID":
                        SvId = int.Parse(property.Value.ToString()!);
                        break;
                    case "X-SV-END":
                        end = DateTimeOffset.Parse(property.Value.ToString()!); // Мы на нулевость проверили, нулевыми не дойдем
                        break;
                    case "X-SV-START":
                        start = DateTimeOffset.Parse(property.Value.ToString()!);
                        break;
                    case "X-SV-TYPE":
                        schedulePeriodType = property.Value.ToString() switch
                        {
                            "SEMESTER" => SchedulePeriodType.Semester,
                            "SESSION" => SchedulePeriodType.Session,
                            "HOLIDAYS" => SchedulePeriodType.Holidays,
                            _ => throw new Exception("Was given not correct x-sv-type in schedule version"),
                        };
                        break;
                }
            }
            versions.Add(new ICalScheduleVersion(
                SvId, start, end, schedulePeriodType
            ));
        }
        return versions;
    }

    public IEnumerable<IScheduleLesson> GetSchedulePeriodTypeLessons(SchedulePeriodType periodType)
    {
        var scheduleVersions = GetScheduleVersions();
        var requiredScheduleVersions = scheduleVersions.Where(v => v.PeriodType == periodType).ToArray();

        if (requiredScheduleVersions.Length == 0)
        {
            return [];
        }

        var minStart = requiredScheduleVersions.Min(v => v.Start);
        var maxEnd = requiredScheduleVersions.Max(v => v.End);

        return GetLessons(minStart, maxEnd).Where(l => requiredScheduleVersions.Any(v => v.Id == l.ScheduleVersionId));
    }
}