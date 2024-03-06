namespace RTU_TC.RTUScheduleClient;

/// <summary>
/// Занятие в расписании
/// </summary>
public interface IScheduleLesson
{
    /// <summary>
    /// Время начала занятия
    /// </summary>
    DateTimeOffset Start { get; }
    /// <summary>
    /// Время окончания занятия
    /// </summary>
    DateTimeOffset End { get; }
    /// <summary>
    /// Подробное описание в человеко-читаемом виде
    /// </summary>
    string Discipline { get; }
    /// <summary>
    /// Тип занятия
    /// </summary>
    string LessonType { get; }
    /// <summary>
    /// Аудитории
    /// </summary>
    IReadOnlyCollection<ScheduleAuditorium> Auditoriums { get; }
    /// <summary>
    /// Группы, принимающие участие в занятии
    /// </summary>
    IReadOnlyCollection<ScheduleGroup> Groups { get; }
    /// <summary>
    /// Преподаватели, ведущие занятие
    /// </summary>
    IReadOnlyCollection<ScheduleTeacher> Teachers { get; }
}