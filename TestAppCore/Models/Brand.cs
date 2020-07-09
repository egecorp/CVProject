using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestAppCore.Models
{
    [JsonObject]
    public class Brand
    {
        [JsonProperty]
        public int Id { set; get; }

        [JsonProperty]
        public string Name { set; get; }

    }
}
