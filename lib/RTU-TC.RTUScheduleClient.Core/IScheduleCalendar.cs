
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
    /// <summary>
    /// Получение всех версий расписания из календаря (может быть несколько, как пример, и сессия и семестр одновременно)
    /// </summary>
    /// <returns></returns>
    IEnumerable<IScheduleVersion> GetScheduleVersions();
    /// <summary>
    /// Получение всех пар по всем версиям расписания
    /// </summary>
    /// <returns></returns>
    IEnumerable<IScheduleLesson> GetAllLessons();
    /// <summary>
    /// Получение расписания по типу расписания
    /// </summary>
    /// <param name="periodType">Тип расписания (пр. сессия/семестр) </param>
    /// <returns></returns>
    IEnumerable<IScheduleLesson> GetSchedulePeriodTypeLessons(SchedulePeriodType periodType);
}