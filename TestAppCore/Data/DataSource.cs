using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestAppCore.Models;
using System.Data.Entity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PagedList;
using System.Data.Entity.Core.Mapping;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestAppCore.Data
{
    /// <summary>
    /// Источник данных для отображения
    /// </summary>
    public static class DataSource
    {
        public const int CARS_PER_PAGE = 10;

        /// <summary>
        /// Проинициализировать БД, создать при необходимости, добавить по умолчанию значения в справочники
        /// </summary>
        public static void InitBase()
        {
            using (EFContext ec = new EFContext())
            {
                bool NeedSave = false;
                ec.Database.CreateIfNotExists();
                ec.Database.Initialize(false);

                if (ec.BDSetBrand.Count() < 1)
                {
                    List<Brand> defaultBrands = JsonConvert.DeserializeObject<List<Brand>>(Properties.DBDefault.Brand);

                    if (defaultBrands != null)
                    {
                        ec.BDSetBrand.AddRange(defaultBrands);
                    }
                    NeedSave = true;
                }

                if (ec.BDSetBodyType.Count() < 1)
                {
                    List<BodyType> defaultBodyTypes = JsonConvert.DeserializeObject<List<BodyType>>(Properties.DBDefault.BodyType);

                    if (defaultBodyTypes != null)
                    {
                        ec.BDSetBodyType.AddRange(defaultBodyTypes);
                    }
                    NeedSave = true;
                }

                if (ec.BDSetCar.Count() < 1)
                {
                    ec.BDSetCar.Add(new Car() { BodyTypeId = 1, BrandId = 1, CarImageId = 0, CreateStamp = DateTime.Now, Id = 1, Name = "1", SeatsCount = 13, Url = null });
                    NeedSave = true;
                }

                if (NeedSave) ec.SaveChanges();
                
            }

        }

        #region Car
        /// <summary>
        /// Вернуть одну страницу с автомобилями
        /// </summary>
        /// <param name="Page">Номер страницы начиная с 0</param>
        public static List<Car> GetCars(int Page)
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    List<Car> carList = ec.BDSetCar.OrderBy(x => x.Id).ToPagedList(Page + 1, CARS_PER_PAGE).ToList() ?? (new List<Car>());
                    return carList;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Вернуть одну страницу с автомобилями
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public static int GetCarsPagesCount()
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    int res = (int)((ec.BDSetCar.Count() - 1) / CARS_PER_PAGE);
                    return (res <= 0) ? 0 : res;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Вернуть одну модель автомобиля
        /// </summary>
        /// <param name="carId">Идентификатор</param>
        public static Car GetCar(int carId)
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    Car oneCar = ec.BDSetCar.Find(carId);
                    return oneCar;
                }
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>
        /// Проверить, существует ли дубль
        /// </summary>
        public static bool IsExistCar(Car sampleCar)
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    Car Car = ec.BDSetCar.Where(
                        x =>
                            (x.Id != sampleCar.Id) &&
                            (x.BodyTypeId == sampleCar.BodyTypeId) &&
                            (x.BrandId == sampleCar.BrandId) &&
                            (x.Name.ToLower() == sampleCar.Name.ToLower()) &&
                            (x.SeatsCount == sampleCar.SeatsCount)
                            ).FirstOrDefault();

                    return Car != null;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Удалить модель автомобиля
        /// </summary>
        public static void DeleteCar(Car oneCar)
        {
            using (EFContext ec = new EFContext())
            {

                bool oldValidateOnSaveEnabled = ec.Configuration.ValidateOnSaveEnabled;

                try
                {
                    ec.Configuration.ValidateOnSaveEnabled = false;

                    ec.BDSetCar.Attach(oneCar);
                    ec.Entry(oneCar).State = EntityState.Deleted;
                    ec.SaveChanges();
                }
                finally
                {
                    ec.Configuration.ValidateOnSaveEnabled = oldValidateOnSaveEnabled;
                }

            }
        }

        /// <summary>
        /// Добавить модель автомобиля
        /// </summary>
        public static void AddCar(Car oneCar)
        {
            using (EFContext ec = new EFContext())
            {
                ec.BDSetCar.Add(oneCar);
                ec.SaveChanges();
            }
        }


        /// <summary>
        /// Изменить модель автомобиля
        /// </summary>
        public static void EditCar(Car oneCar)
        {
            using (EFContext ec = new EFContext())
            {

                ec.BDSetCar.Attach(oneCar);
                ec.Entry(oneCar).State = EntityState.Modified;
                ec.SaveChanges();
            }
        }
        #endregion

        #region Brand

        /// <summary>
        /// Вернуть все бренды
        /// </summary>
        public static List<Brand> GetBrands()
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    List<Brand> brandList = ec.BDSetBrand.ToList() ?? (new List<Brand>());
                    return brandList;
                }
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>
        /// Вернуть бренд по Id
        /// </summary>
        public static Brand GetBrand(int Id)
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    Brand oneBrand = ec.BDSetBrand.Find(Id);
                    return oneBrand;
                }
            }
            catch 
            {
                return null;
            }
        }

        #endregion

        #region BodyType

        /// <summary>
        /// Вернуть все кузовы
        /// </summary>
        public static List<BodyType> GetBodyTypes()
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    List<BodyType> bodyTypeList = ec.BDSetBodyType.ToList() ?? (new List<BodyType>());
                    return bodyTypeList;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Вернуть кузов по Id
        /// </summary>
        public static BodyType GetBodyType(int Id)
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    BodyType oneBodyType = ec.BDSetBodyType.Find(Id);
                    return oneBodyType;
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region CarImage
        /// <summary>
        /// Добавить картинку модели
        /// </summary>
        public static void AddCarImage(CarImage oneCarImage)
        {
            using (EFContext ec = new EFContext())
            {
                ec.BDSetCarImage.Add(oneCarImage);
                ec.SaveChanges();
            }
        }

        /// <summary>
        /// Изменить картинку модели
        /// </summary>
        public static void EditCarImage(CarImage oneCarImage)
        {
            using (EFContext ec = new EFContext())
            {
                ec.BDSetCarImage.Attach(oneCarImage);
                ec.Entry(oneCarImage).State = EntityState.Modified;
                ec.SaveChanges();
            }
        }

        /// <summary>
        /// Вернуть картинку модели
        /// </summary>
        public static CarImage GetCarImage(int Id)
        {
            try
            {
                using (EFContext ec = new EFContext())
                {
                    CarImage oneCarImage = ec.BDSetCarImage.Find(Id);
                    return oneCarImage;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }
}
