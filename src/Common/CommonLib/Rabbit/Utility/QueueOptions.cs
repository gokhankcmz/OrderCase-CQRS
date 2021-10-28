using System.Collections.Generic;

namespace CommonLib.Rabbit.Utility
{
    public class QueueOptions
    {
        public string QueueName { get; set; } = "";
        public string RoutingKey { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public bool Exclusive { get; set; }
        public Dictionary<string,object> Arguments { get; set; }
    }
}