﻿namespace EFCoreDemo.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = null!;
        public string SignKey { get; set; } = null!;
    }
}
