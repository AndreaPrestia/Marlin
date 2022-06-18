﻿namespace Marlin.Core.Entities
{
    public class MarlinConfiguration
    {
        public MarlinJwtConfiguration JwtConfiguration { get; set; }
        public bool PropagateApplicationError { get; set; }
        public bool EventLoggerEnabled { get; set; }
        public string CorsOrigins { get; set; }
        public string CertificateName { get; set; }
        public string CertificatePassword { get; set; }
        public int Port { get; set; }
    }
}
