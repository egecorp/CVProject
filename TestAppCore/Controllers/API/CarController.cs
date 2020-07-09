using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAppCore.Models;
using TestAppCore.Data;
using Newtonsoft.Json;

namespace TestAppCore.Controllers
{
    public class CarController : Controller
    {
        private readonly ILogger<CarController> _logger;

        public CarController(ILogger<CarController> logger)
        {
            _logger = logger;
        }

        public string GetPage([FromQuery]string Page)
        {
            int PagesCount = DataSource.GetCarsPagesCount();

            int selectedPage = 0;
            if (Page == "last")
            {
                selectedPage = PagesCount - 1;
            }
            else
            {
                if (!int.TryParse(Page, out selectedPage)) selectedPage = 0;
            }

            if ((selectedPage < 0) || (selectedPage >= PagesCount)) 
            {
                return GetErrorAnswer("Укажите корректный номер страницы в интервале от 1 до " + PagesCount.ToString());
            }

            if (selectedPage > PagesCount - 1) selectedPage = PagesCount - 1;

            List<Car> carList = DataSource.GetCars(selectedPage);

            RequestResult answer = new RequestResult()
            {
                CurrentPage = selectedPage,
                PageCount = PagesCount,
                CarList = carList
            };

            return JsonConvert.SerializeObject(answer);
        }


        public string GetCar([FromQuery] string Id)
        {
            int carId = -1;

            if (!int.TryParse(Id, out carId))
            {
                return GetErrorAnswer("Укажите корректный идентификатор модели автомобиля");
            }

            Car oneCar = DataSource.GetCar(carId);
            if (oneCar == null) 
            {
                return GetErrorAnswer("Не удалось найти модель автомобиля");
            }

            RequestResult answer = new RequestResult()
            {
                OneCar = oneCar
            };

            return JsonConvert.SerializeObject(answer);
        }

        public string DeleteCar([FromQuery] string Id)
        {
            int carId = -1;

            if (!int.TryParse(Id, out carId))
            {
                return GetErrorAnswer("Укажите корректный идентификатор модели автомобиля");
            }

            Car oneCar = DataSource.GetCar(carId);
            if (oneCar == null)
            {
                return GetErrorAnswer("Не удалось найти модель автомобиля");
            }

            DataSource.DeleteCar(oneCar);

            int PagesCount = DataSource.GetCarsPagesCount();

            RequestResult answer = new RequestResult()
            {
                PageCount = PagesCount
            };

            return JsonConvert.SerializeObject(answer);
        }

        // Перенести
        public string GetErrorAnswer(string txt)
        {
            RequestResult answer = new RequestResult()
            {
                Error = txt
            };

            return JsonConvert.SerializeObject(answer);
        }
        
    }
}
