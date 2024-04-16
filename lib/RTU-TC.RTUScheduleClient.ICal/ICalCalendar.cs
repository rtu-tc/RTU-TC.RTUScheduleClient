using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using RTU_TC.RTUScheduleClient.ICal;
using System.Globalization;

namespace RTU_TC.RTUScheduleClient;
public partial class ICalCalendar(Ical.Net.Calendar Calendar) : IScheduleCalendar
{
    private readonly TimeSpan _correctOffset = Calendar.TimeZones.Single().TimeZoneInfos.Single().OffsetFrom.Offset;
    private readonly string _tzName = Calendar.TimeZones.Single().Name;

    public IEnumerable<IScheduleLesson> GetLessons(DateTimeOffset from, DateTimeOffset to)
    {
        var fromTime = new CalDateTime(from.ToOffset(_correctOffset).DateTime, _tzName);
        var toTime = new CalDateTime(to.ToOffset(_correctOffset).DateTime, _tzName);
        return Calendar
            .GetOccurrences(fromTime, toTime)
            .Select(occ => (occ.Period, Source: (occ.Source as CalendarEvent)!))
            .Where(t => t.Source.Transparency == TransparencyType.Opaque) // занятие - занятия. Не занятые - недели и т.д.
            .Select(occ => new ICalScheduleLesson(occ.Period, occ.Source) as IScheduleLesson);
    }

    public IEnumerable<IScheduleLesson> GetAllLessons()
    {
        var scheduleVersions = GetScheduleVersions();
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
        var filteredIds = scheduleVersions.Where(sv => sv.PeriodType == periodType).Select(sv => sv.Id);    

        var minStart = scheduleVersions
            .Where(v => v.PeriodType == periodType)
            .Min(v => v.Start);
        var maxEnd = scheduleVersions
            .Where(v => v.PeriodType == periodType)
            .Max(v => v.End);

        return GetLessons(minStart, maxEnd).Where(l => filteredIds.Contains(l.ScheduleVersionId));
    }
}