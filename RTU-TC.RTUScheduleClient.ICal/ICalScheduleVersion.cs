
namespace RTU_TC.RTUScheduleClient.ICal;
public partial class ICalScheduleVersion(
        int SvId,
        DateTimeOffset SvStart,
        DateTimeOffset SvEnd,
        SchedulePeriodType SvPeriodType
    ) : IScheduleVersion
{
    public int Id { get; } = SvId;
    public DateTimeOffset Start { get; } = SvStart;
    public DateTimeOffset End { get; } = SvEnd;
    public SchedulePeriodType PeriodType { get; } = SvPeriodType;
}
