using System.Collections.Generic;

namespace CommonLib.Rabbit.Utility
{
    public class ExchangeOptions
    {
        
        public string ExchangeName { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public Dictionary<string,object> Arguments { get; set; }

        public string ExchangeType { get; set; }
    }
}