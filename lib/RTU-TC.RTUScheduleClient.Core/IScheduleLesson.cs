namespace RTU_TC.RTUScheduleClient;

/// <summary>
/// Занятие в расписании
/// </summary>
public interface IScheduleLesson
{
    /// <summary>
    /// Идентификатор занятия. Важно - он может быть один для нескольких "повторяющихся" занятий, и уникален только для занятий, проводимых в одно и то же время.
    /// </summary>
    string Id { get; }
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
    /// SvId версии расписания, к которой относится пара
    /// </summary>
    int ScheduleVersionId { get; }
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
    /// <summary>
    /// Подгруппа(ы) занятия 
    /// </summary>
    IReadOnlyCollection<int> SubGroups { get; }
    /// <summary>
    /// Дополнительный тип занятия
    /// </summary>
    string AdditionalLessonType { get; }
}