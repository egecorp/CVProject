using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CVProject.Data;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace CVProject.Models
{
    [JsonObject]
    public class VCar : Car
    {
        [JsonProperty]
        [NotMapped]
        public string BodyTypeName
        {
            get
            {
                BodyType bt = DataSource.GetBodyType(this.BodyTypeId);
                return (bt == null) ? null : bt.Name;
            }
            set
            {

            }
        }
        
        [JsonProperty]
        [NotMapped]
        public string BrandName
        {
            get
            {
                Brand br = DataSource.GetBrand(this.BrandId);
                return (br == null) ? null : br.Name;
            }
            set
            {

            }
        }


        public VCar()
            :base()
        {

        }


        public VCar(Car m)
            : base(m)
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
                Regex urlCheck = new Regex(@"^((http|https|)\://)?([a-zA-Z0-9\-]?[a-zA-Z0-9\-]*\.)*ru$");
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
