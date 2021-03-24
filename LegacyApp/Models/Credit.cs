namespace LegacyApp
{
    public record Credit
    {
        public Credit(bool hasLimit, int limit)
        {
            this.HasLimit = hasLimit;
            this.Limit = limit;
        }
        
        public bool HasLimit { get; }
        public int Limit { get; }

        public bool IsHigherOrEqualTo(int expectedLimit) => !this.HasLimit || this.Limit >= expectedLimit;
    }
}