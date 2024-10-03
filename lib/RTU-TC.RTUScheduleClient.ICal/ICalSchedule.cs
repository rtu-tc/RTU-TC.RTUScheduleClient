namespace RTU_TC.RTUScheduleClient.ICal;

public interface ICalSchedule
{
    Task<Ical.Net.Calendar> GetCalendarRawAsync(CancellationToken cancellationToken = default);
}