namespace RTU_TC.RTUScheduleClient;

/// <summary>
/// Учебная аудитория
/// </summary>
public record ScheduleAuditorium
{
    /// <summary>
    /// Уникальный идентификатор среди аудиторий
    /// </summary>
    public required long Id { get; set; }
    /// <summary>
    /// Название аудитории, номер и кампус
    /// <example>
    /// А-1 (В-78)
    /// </example>
    /// </summary>
    public required string Title { get; set; }
    /// <summary>
    /// Номер аудитории
    /// <example>
    /// А-1
    /// </example>
    /// </summary>
    public required string Number { get; set; }
    /// <summary>
    /// Кампус аудитории
    /// <example>
    /// В-78
    /// </example>
    /// </summary>
    public required string? Campus { get; set; }
}
