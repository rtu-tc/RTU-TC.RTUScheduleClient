using Ical.Net.CalendarComponents;
using System.Globalization;

namespace RTU_TC.RTUScheduleClient;
public class ICalCalendar(Ical.Net.Calendar Calendar) : IScheduleCalendar
{
    public IEnumerable<IScheduleLesson> GetLessons(DateTimeOffset from, DateTimeOffset to)
    {
        return Calendar
            .GetOccurrences(from.UtcDateTime, to.UtcDateTime)
            .Select(occ => new ICalScheduleLesson(occ.Period, (occ.Source as CalendarEvent)!) as IScheduleLesson);
    }
}
