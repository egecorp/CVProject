using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestAppCore.Models
{
    [JsonObject]
    public class Car
    {

        [JsonProperty]
        public int Id { set; get; }

        [JsonProperty]
        public int BrandId { set; get; }

        [StringLength(1000)]
        [JsonProperty]
        public string Name { set; get; }

        [JsonProperty]
        public DateTime CreateStamp { set; get; }

        [JsonProperty]
        public int BodyTypeId { set; get; }

        [JsonProperty]
        public int SeatsCount { set; get; }

        [StringLength(1000)]
        [JsonProperty]
        public string Url { set; get; }

        [JsonIgnore]
        public byte CarImageId { set; get; }

    }
}
