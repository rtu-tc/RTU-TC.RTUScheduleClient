using FluentAssertions;

namespace RTU_TC.RTUScheduleClient.ICal.Tests.HttpSearchICalContentRTUScheduleTests;
public class ConfigurationTests
{
    [Fact]
    public void CantCreateWithoutClientName()
    {
        Func<IRTUScheduleClient> createScheduleClientWithoutClientName = () => new HttpSearchICalContentRTUSchedule(new HttpClient());
        createScheduleClientWithoutClientName.Should().Throw<NoClientNameProvidedException>();
    }
}
