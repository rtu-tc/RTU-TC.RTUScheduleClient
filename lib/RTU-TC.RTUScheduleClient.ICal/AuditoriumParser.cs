using System.Text.RegularExpressions;

namespace RTU_TC.RTUScheduleClient.ICal;
public static partial class AuditoriumParser
{
    private static readonly Regex _audNameRegex = GetAuditoriumValuesRegex();

    public static ScheduleAuditorium ParseAuditorium(long audId, string audTitle)
    {
        var audCampus = "";
        var audMatch = _audNameRegex.Match(audTitle);
        if (audMatch.Success)
        {
            audTitle = audMatch.Groups["title"].Value;
            audCampus = audMatch.Groups["campus"].Value;
        }

        return new ScheduleAuditorium(
            audId,
            audTitle,
            audCampus
        );
    }

    [GeneratedRegex(@"^(?<title>.+)\s+\((?<campus>.*)\)$")]
    private static partial Regex GetAuditoriumValuesRegex();
}
