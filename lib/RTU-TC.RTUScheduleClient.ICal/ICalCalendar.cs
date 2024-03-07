using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System.Globalization;

namespace RTU_TC.RTUScheduleClient;
public class ICalCalendar(Ical.Net.Calendar Calendar) : IScheduleCalendar
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
}
