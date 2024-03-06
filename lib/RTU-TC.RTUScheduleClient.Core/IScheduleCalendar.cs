
namespace RTU_TC.RTUScheduleClient;

/// <summary>
/// Календарь расписания
/// </summary>
public interface IScheduleCalendar
{
    /// <summary>
    /// Получить события занятий внутри промежутка времени
    /// </summary>
    /// <param name="from">Начало промежутка времени</param>
    /// <param name="to">Окончание промежутка времени</param>
    /// <returns></returns>
    IEnumerable<IScheduleLesson> GetLessons(DateTimeOffset from, DateTimeOffset to);
}