namespace RTU_TC.RTUScheduleClient;

public enum ScheduleTarget
{
    /// <summary>
    /// Неизвестный тип - некорректное значение
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// Академическая группа
    /// </summary>
    Group = 1,
    /// <summary>
    /// Преподаватель
    /// </summary>
    Teacher = 2,
    /// <summary>
    /// Пудитория
    /// </summary>
    Auditorium = 3,
}