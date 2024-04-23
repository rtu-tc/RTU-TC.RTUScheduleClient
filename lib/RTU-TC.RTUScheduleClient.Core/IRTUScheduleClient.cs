
namespace RTU_TC.RTUScheduleClient;

/// <summary>
/// Клиент расписания РТУ МИРЭА
/// </summary>
public interface IRTUScheduleClient
{
    /// <summary>
    /// Все расписания
    /// </summary>
    /// <param name="match">Опциональный поиск по названию (группы/аудитории/преподавателя)</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ISchedule> GetAllSchedulesAsync(string? match = default, CancellationToken cancellationToken = default);
    /// <summary>
    /// Расписания групп
    /// </summary>
    /// <param name="match">Опциональный поиск по названию группы</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ISchedule> GetAllGroupSchedulesAsync(string? match = default, CancellationToken cancellationToken = default);
    /// <summary>
    /// Распсиания преподавателей
    /// </summary>
    /// <param name="match">Опциональный поиск по преподавателю</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ISchedule> GetAllTeacherSchedulesAsync(string? match = default, CancellationToken cancellationToken = default);
    /// <summary>
    /// Расписания аудиторий
    /// </summary>
    /// <param name="match">Опциональный поиск по названию аудитории</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ISchedule> GetAllAuditoriumSchedulesAsync(string? match = default, CancellationToken cancellationToken = default);
}