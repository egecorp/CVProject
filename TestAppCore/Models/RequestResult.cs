using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CVProject.Models
{
    /// <summary>
    /// Универсальный класс для ответов сервера на запросы из фронта
    /// </summary>
    [JsonObject]
    public class RequestResult
    {
        [JsonProperty]
        public string Error { set; get; }

        [JsonProperty]
        public int CurrentPage { set; get; }

        [JsonProperty]
        public int PageCount { set; get; }

        [JsonProperty]
        public List<VCar> CarList { set; get; }

        [JsonProperty]
        public VCar OneCar { set; get; }

        [JsonProperty]
        public int ImageId { set; get; }

        public static string GetErrorAnswer(string txt)
        {
            RequestResult answer = new RequestResult()
            {
                Error = txt
            };

            return JsonConvert.SerializeObject(answer);
        }


    }
}
