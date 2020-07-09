using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestAppCore.Data;

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
        [Required]
        [JsonProperty]
        public string Name { set; get; }

        [JsonProperty]
        public DateTime CreateStamp { set; get; }

        [JsonProperty]
        public int BodyTypeId { set; get; }

        /*[JsonIgnore]
        public virtual BodyType BodyType { get; set; }
        */

        [JsonProperty]
        [NotMapped]
        public string BodyTypeName
        {
            get
            {
                BodyType bt = DataSource.GetBodyType(this.BodyTypeId);
                return (bt == null) ? null : bt.Name;
                //return (this.BodyType == null) ? null : this.BodyType.Name;
            }
            set
            {

            }
        }
        
        /*
        [JsonIgnore]
        public virtual Brand Brand { get; set; }
        */

        [JsonProperty]
        [NotMapped]
        public string BrandName
        {
            get
            {
                Brand br = DataSource.GetBrand(this.BrandId);
                return (br == null) ? null : br.Name;
                //return (this.Brand == null) ? null : this.Brand.Name;
            }
            set
            {

            }
        }


        [JsonProperty]
        public int SeatsCount { set; get; }

        [StringLength(1000)]
        [JsonProperty]
        public string Url { set; get; }

        [JsonIgnore]
        public byte CarImageId { set; get; }


    }
}
