using System;
using System.Collections.Generic;

namespace TopCrypto.DataLayer.Services.FiatCurrency.Models
{
    public class FiatJSonResponseDTO : ICloneable
    {
        public bool success { get; set; }
        public int? timestamp { get; set; }
        public string @base { get; set; }
        public string date { get; set; }
        public Dictionary<string, double> rates { get; set; }

        public object Clone()
        {
            return new FiatJSonResponseDTO()
            {
                success = this.success,
                timestamp = this.timestamp,
                @base = this.@base,
                date = this.date,
                rates = this.rates == null ? 
                new Dictionary<string, double>() : new Dictionary<string, double>(this.rates)
        };
    }
}
}
