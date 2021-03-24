using System;

namespace LegacyApp
{
    public record BirthDate(DateTime Value)
    {
        public bool IsOlderOrEqualTo(int expectedAge)
        {
            DateTime now = DateTimeService.GetCurrentTime();
            int age = now.Year - this.Value.Year;

            if (now.Month < this.Value.Month || now.Month == this.Value.Month && now.Day < this.Value.Day)
            {
                age--;
            }
            return age >= expectedAge;
        }
    }
}