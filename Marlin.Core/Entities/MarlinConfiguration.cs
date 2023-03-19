namespace Marlin.Core.Entities
{
    public class MarlinConfiguration
    {
        public MarlinJwtConfiguration JwtConfiguration { get; set; }
        public bool PropagateApplicationError { get; set; }
        public string CorsOrigins { get; set; }
    }
}
