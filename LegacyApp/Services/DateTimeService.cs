using System;

namespace LegacyApp
{
    public static class DateTimeService
    {
        public static Func<DateTime> GetCurrentTime { get; set; } = () => DateTime.Now;
    }
}