namespace Project10pm.Recognizers
{
    public class TimeShift
    {
        private readonly DateTime _locaDateTime;

        /// <summary>
        /// The UTC offset when this object instance was origially created.
        /// </summary>
        private readonly TimeSpan _initialUtcOffset;
        private readonly TimeZoneInfo _localTimeZone;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <param name="localTimeZone"></param>
        public TimeShift(DateTime localDateTime, TimeZoneInfo localTimeZone)
        {
            _initialUtcOffset = localTimeZone.GetUtcOffset(localDateTime);
            _localTimeZone = localTimeZone;
            _locaDateTime = localDateTime;
        }

        public DateTimeOffset ToCurrentDateTimeOffset()
        {
            var utcOffset = _localTimeZone.GetUtcOffset(_locaDateTime);
            return new DateTimeOffset(_locaDateTime, utcOffset);
        }
    }
}
