namespace RTU_TC.RTUScheduleClient;

/// <summary>
/// Расписание сущности
/// </summary>
public interface ISchedule
{
    /// <summary>
    /// Уникальный идентификатор в рамках <see cref="ScheduleTarget"/>
    /// </summary>
    int Id { get; }
    /// <summary>
    /// Тип расписания
    /// </summary>
    ScheduleTarget ScheduleTarget { get; }
    /// <summary>
    /// Наименование расписания - конкретной сущности
    /// </summary>
    string TargetTitle { get; }
    Task<IScheduleCalendar> GetCalendarAsync(CancellationToken cancellationToken = default);
}
