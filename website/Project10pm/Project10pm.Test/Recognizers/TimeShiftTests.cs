using Project10pm.Recognizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10pm.Test.Recognizers
{
    [TestFixture]
    internal class TimeShiftTests
    {
        [Test]
        public void LocalDateTime_ShiftToCurrentUTC_ReturnsCorrectDateTimeOffset()
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
            DateTime dateTime = new DateTime(2023, 01, 01, 0, 0, 0, DateTimeKind.Unspecified);

            var timeShift = new TimeShift(dateTime, timeZoneInfo);
            DateTimeOffset offsetShift = timeShift.ToCurrentDateTimeOffset();

            Assert.That(offsetShift.LocalDateTime, Is.EqualTo(dateTime));
        }
    }
}
