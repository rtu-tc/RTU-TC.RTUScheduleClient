
namespace RTU_TC.RTUScheduleClient;

/// <summary>
/// Клиент расписания РТУ МИРЭА
/// </summary>
public interface IRTUScheduleClient
{
    /// <summary>
    /// Все расписания
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ISchedule> GetAllSchedulesAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Расписания групп
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ISchedule> GetAllGroupSchedulesAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Распсиания преподавателей
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ISchedule> GetAllTeacherSchedulesAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Расписания аудиторий
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ISchedule> GetAllAuditoriumSchedulesAsync(CancellationToken cancellationToken = default);
}