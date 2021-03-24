namespace LegacyApp
{
    public record EmailAddress(string Value)
    {
        public bool HasValidFormat() => !this.Value.Contains("@") || this.Value.Contains(".");
    }
}