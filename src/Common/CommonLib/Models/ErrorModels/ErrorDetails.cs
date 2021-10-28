using System.Collections.Generic;
using Newtonsoft.Json;

namespace CommonLib.Models.ErrorModels
{
    [JsonObject(MemberSerialization.OptIn)]
    
    public class ErrorDetails : System.Exception
    {
        [JsonProperty] public int StatusCode { get; set; }
        [JsonProperty] public string ErrorMessage { get; set; }

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
         public Dictionary<string,string[]> Details { get; set; }

         public override string ToString() => JsonConvert.SerializeObject(this); //her error için ayrı bir tostring olabilir.
    }


}