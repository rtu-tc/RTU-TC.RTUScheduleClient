using FluentAssertions;
using Ical.Net;

namespace RTU_TC.RTUScheduleClient.ICal.Tests.ICalCalendarTests;
public class CorrectGetLessonsTests
{
    [Fact]
    public void ZeroPeriodReturnsEmptyCollection()
    {
        var calendar = Calendar.Load(File.OpenText("./ICalCalendarTests/exampleschedule.ics"));
        var icalScheduleCal = new ICalCalendar(calendar);

        var time = DateTimeOffset.Parse("2024-03-07T00:14:23.0479662+03:00");
        var lessons = icalScheduleCal.GetLessons(time, time);

        lessons.Should().BeEmpty();
    }

    [Fact]
    public void ZeroPeriodInLessonReturnsCorrectCollection()
    {
        var calendar = Calendar.Load(File.OpenText("./ICalCalendarTests/exampleschedule.ics"));
        var icalScheduleCal = new ICalCalendar(calendar);

        var time = DateTimeOffset.Parse("2024-03-15T10:50:23.0479662+03:00");
        var lessons = icalScheduleCal.GetLessons(time, time);

        lessons.Should().HaveCount(2);
    }

    [Fact]
    public void LessonTimePeriodReturnsOneCorrectLesson()
    {
        var calendar = Calendar.Load(File.OpenText("./ICalCalendarTests/exampleschedule.ics"));
        var icalScheduleCal = new ICalCalendar(calendar);

        var timeStart = DateTimeOffset.Parse("2024-03-15T14:20:00+03:00");
        var timeEnd = DateTimeOffset.Parse("2024-03-15T15:50:00+03:00");
        var lessons = icalScheduleCal.GetLessons(timeStart, timeEnd);

        lessons.Should().HaveCount(1);
        var lesson = lessons.Single();
        lesson.Discipline.Should().Be("Электрохимия");
        lesson.LessonType.Should().Be("ЛАБ");
        lesson.Start.Should().Be(timeStart);
        lesson.End.Should().Be(timeEnd);


        lesson.Groups.Should()
            .HaveCount(1)
            .And
            .BeEquivalentTo([new ScheduleGroup(933, "ХХБО-02-20")]);
        lesson.Auditoriums.Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo([new ScheduleAuditorium{
                Id = 643,
                Title = "О-105 (В-86)",
                Number ="О-105",
                Campus = "В-86"
            }, new ScheduleAuditorium
            {
                Id = 740, 
                Title = "О-131-132 (В-86)",
                Number = "О-131-132", 
                Campus = "В-86"
            }]);
        lesson.Teachers.Should()
            .HaveCount(1)
            .And
            .BeEquivalentTo([new ScheduleTeacher(1466, "Лебедева Марина Владимировна")]);
    }

}
