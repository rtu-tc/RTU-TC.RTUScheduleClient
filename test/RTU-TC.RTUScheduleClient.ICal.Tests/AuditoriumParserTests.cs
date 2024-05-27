namespace RTU_TC.RTUScheduleClient.ICal.Tests;
public class AuditoriumParserTests
{
    [Fact]
    public void AuditoriumWithCampusParsed()
    {
        var parsed = AuditoriumParser.ParseAuditorium(1, "А-234-а (МП-1)");
        Assert.Equal("А-234-а", parsed.Title);
        Assert.Equal("МП-1", parsed.Campus);
    }

    [Fact]
    public void AuditoriumWithoutCampusParsed()
    {
        var parsed = AuditoriumParser.ParseAuditorium(1, "К-7");
        Assert.Equal("К-7", parsed.Title);
        Assert.Equal("", parsed.Campus);
    }
}
