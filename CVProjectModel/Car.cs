using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace CVProject.Models
{
    [JsonObject]
    public class Car
    {

        [JsonProperty]
        public int Id { set; get; }

        [Required]
        [JsonProperty]
        public int BrandId { set; get; }

        [StringLength(1000)]
        [Required]
        [JsonProperty]
        public string Name { set; get; }

        [Required]
        [JsonProperty]
        public DateTime CreateStamp { set; get; }

        [Required]
        [JsonProperty]
        public int BodyTypeId { set; get; }

        [Required]
        [JsonProperty]
        public int SeatsCount { set; get; }

        [StringLength(1000)]
        [JsonProperty]
        public string Url { set; get; }

        [Required]
        [JsonProperty]
        public int CarImageId { set; get; }

        public Car()
        {

        }

        public Car(Car m)
        {
            this.Id = m.Id;
            this.BrandId = m.BrandId;
            this.Name = m.Name;
            this.CreateStamp = m.CreateStamp;
            this.BodyTypeId = m.BodyTypeId;
            this.SeatsCount = m.SeatsCount;
            this.Url = m.Url;
            this.CarImageId = m.CarImageId;
        }

    }

}
