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
            if (!int.TryParse(Page, out selectedPage)) selectedPage = 0;

            if ((selectedPage < 0) || (selectedPage > PagesCount)) 
            {
                return RequestResult.GetErrorAnswer("Укажите корректный номер страницы в интервале от 1 до " + PagesCount.ToString());
            }

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
                return RequestResult.GetErrorAnswer("Укажите корректный идентификатор модели автомобиля");
            }

            Car oneCar = DataSource.GetCar(carId);
            if (oneCar == null) 
            {
                return RequestResult.GetErrorAnswer("Не удалось найти модель автомобиля");
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
                return RequestResult.GetErrorAnswer("Укажите корректный идентификатор модели автомобиля");
            }

            try
            {
                Car oneCar = DataSource.GetCar(carId);
                if (oneCar == null)
                {
                    return RequestResult.GetErrorAnswer("Не удалось найти модель автомобиля");
                }

                DataSource.DeleteCar(oneCar);

                int PagesCount = DataSource.GetCarsPagesCount();

                RequestResult answer = new RequestResult()
                {
                    PageCount = PagesCount
                };

                return JsonConvert.SerializeObject(answer);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message + Environment.NewLine + ee.StackTrace);
                return RequestResult.GetErrorAnswer("При выполнении запроса произошла ошибка");
            }
        }

        
        [HttpPost]
        public string CreateCar([FromBody] Car value)
        {
            if (value == null)
            {
                return RequestResult.GetErrorAnswer("Не передан объект для создания");
            }

            try
            {
                string err = value.PutValidate();
                if (err != null)
                {
                    return RequestResult.GetErrorAnswer(err);
                }

                DataSource.AddCar(value);


                int PagesCount = DataSource.GetCarsPagesCount();

                RequestResult answer = new RequestResult()
                {
                    PageCount = PagesCount
                };

                return JsonConvert.SerializeObject(answer);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message + Environment.NewLine + ee.StackTrace);
                return RequestResult.GetErrorAnswer("При выполнении запроса произошла ошибка");
            }
        }


        [HttpPost]
        public string EditCar([FromBody] Car value)
        {
            if (value == null)
            {
                return RequestResult.GetErrorAnswer("Не передан объект для создания");
            }

            try
            {

                string err = value.PostValidate();
                if (err != null)
                {
                    return RequestResult.GetErrorAnswer(err);
                }

                Car existCar = DataSource.GetCar(value.Id);
                if (existCar == null)
                {
                    return RequestResult.GetErrorAnswer("Не удалось найти модель с данным идентификатором");
                }

                value.LoadUnchangedData(existCar);

                DataSource.EditCar(value);


                int PagesCount = DataSource.GetCarsPagesCount();

                RequestResult answer = new RequestResult()
                {
                    PageCount = PagesCount
                };

                return JsonConvert.SerializeObject(answer);
            }
            catch(Exception ee)
            {
                _logger.LogError(ee.Message + Environment.NewLine + ee.StackTrace);
                return RequestResult.GetErrorAnswer("При выполнении запроса произошла ошибка");
            }
        }





    }
}
