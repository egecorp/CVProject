using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestAppCore.Models
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
        public List<Car> CarList { set; get; }

        [JsonProperty]
        public Car OneCar { set; get; }

    }
}
