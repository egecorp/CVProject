using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestAppCore.Data;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

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

        [JsonProperty]
        public int CarImageId { set; get; }

        public Car()
        {

        }

        public void LoadUnchangedData(Car existsCar)
        {
            this.Id = existsCar.Id;
            this.CreateStamp = existsCar.CreateStamp;
        }

        private string CommonValidate()
        {
            this.Name = (this.Name ?? "").Trim();
            if (this.Name == "") return "Необходимо указать корректное имя";
            if (this.Name.Length > 1000) return "Имя не должно быть длиннее 1000 символов";

            this.Url = (this.Url ?? "").Trim();
            if (this.Url == "")
            {
                this.Url = null;
            }
            else
            {
                // Упрощения ради разрешим только адреса сайтов, без правой части адреса и без порта
                Regex urlCheck = new Regex(@"^(http|https|)\://|[a-zA-Z0-9\-\.]+\.(:[a-zA-Z0-9]*)?(\.)ru$");
                if (!urlCheck.IsMatch(this.Url))
                {
                    return "Некорректный формат адреса сайта";
                }
                if (this.Url.Length > 1000) return "Имя не должно быть длиннее 1000 символов";
            }
            

            CarImage ci = DataSource.GetCarImage(this.CarImageId);
            if (ci == null) return "Необходимо загрузить изображение";

            BodyType bt = DataSource.GetBodyType(this.BodyTypeId);
            if (bt == null) return "Необходимо выбрать кузов";

            Brand bd = DataSource.GetBrand(this.BrandId);
            if (bd == null) return "Необходимо выбрать бренд";

            if ((this.SeatsCount < 1) || (this.SeatsCount > 12)) return "Количество сидений должно быть от 1 до 12";

            if (DataSource.IsExistCar(this)) return "Другая модель с переданными параметрами уже существует";

            return null;
        }

        public string PutValidate()
        {
            string res = CommonValidate();
            if (res != null) return res;

            this.CreateStamp = DateTime.Now;

            return null;
        }

        public string PostValidate()
        {
            string res = CommonValidate();
            if (res != null) return res;

            return null;
        }
    }







}
