namespace Marlin.Core.Entities
{
    public class MarlinJwtConfiguration
    {
        public string JwtSecret { get; set; }
        public string JwtAudience { get; set; }
        public int JwtDurationHours { get; set; }
        public string JwtIssuer { get; set; }
    }
}
