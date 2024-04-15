namespace RTU_TC.RTUScheduleClient;

/// <summary>
/// Версия расписания
/// </summary>
public interface IScheduleVersion
{
    /// <summary>
    /// SvId версии расписания
    /// </summary>
    int Id { get; }
    /// <summary>
    /// Время начала периода расписания в данной версии
    /// </summary>
    DateTimeOffset Start { get; }
    /// <summary>
    /// Время конца периода расписания в данной версии
    /// </summary>
    DateTimeOffset End { get; }
    /// <summary>
    /// Тип периода (сессия, семестр, каникулы)
    /// </summary>
    SchedulePeriodType PeriodType { get; }
}

public enum SchedulePeriodType
{
    /// <summary>
    /// Неизвестный тип расписания
    /// </summary>
    Unknown,
    /// <summary>
    /// Семестр
    /// </summary>
    Semester,
    /// <summary>
    /// Сессия
    /// </summary>
    Session,
    /// <summary>
    /// Каникулы
    /// </summary>
    Holidays,
}
