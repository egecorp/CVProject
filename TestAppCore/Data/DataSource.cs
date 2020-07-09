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

namespace TestAppCore.Data
{
    /// <summary>
    /// Источник данных для отображения
    /// </summary>
    public static class DataSource
    {

        public const int CARS_PER_PAGE = 10;

        static DataSource() 
        { 

        }

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

        /// <summary>
        /// Вернуть одну страницу с автомобилями
        /// </summary>
        /// <param name="Page">Номер страницы начиная с 0</param>
        public static List<Car> GetCars(int Page)
        {
            using (EFContext ec = new EFContext())
            {
                List<Car> carList = ec.BDSetCar.OrderBy(x => x.Id).ToPagedList(Page + 1, CARS_PER_PAGE).ToList() ?? (new List<Car>());
                return carList;
            }            
        }

        /// <summary>
        /// Вернуть одну страницу с автомобилями
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public static int GetCarsPagesCount()
        {
            using (EFContext ec = new EFContext())
            {
                int res = (int)(ec.BDSetCar.Count() / CARS_PER_PAGE);
                return (res == 0) ? 1 : res;
            }
        }

        /// <summary>
        /// Вернуть одну модель автомобиля
        /// </summary>
        /// <param name="carId">Идентификатор</param>
        public static Car GetCar(int carId)
        {
            using (EFContext ec = new EFContext())
            {
                Car oneCar = ec.BDSetCar.Find(carId);
                return oneCar;
            }
        }

        /// <summary>
        /// Удалить модель автомобиля
        /// </summary>
        /// <param name="carId">Идентификатор</param>
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
        /// Вернуть все бренды
        /// </summary>
        public static List<Brand> GetBrands()
        {
            using (EFContext ec = new EFContext())
            {
                List<Brand> brandList = ec.BDSetBrand.ToList() ?? (new List<Brand>());
                return brandList;
            }
        }

        /// <summary>
        /// Вернуть бренд по Id
        /// </summary>
        public static Brand GetBrand(int Id)
        {
            using (EFContext ec = new EFContext())
            {
                Brand oneBrand = ec.BDSetBrand.Find(Id);
                return oneBrand;
            }
        }


        /// <summary>
        /// Вернуть все кузовы
        /// </summary>
        public static List<BodyType> GetBodyTypes()
        {
            using (EFContext ec = new EFContext())
            {
                List<BodyType> bodyTypeList = ec.BDSetBodyType.ToList() ?? (new List<BodyType>());
                return bodyTypeList;
            }
        }
        /// <summary>
        /// Вернуть кузов по Id
        /// </summary>
        public static BodyType GetBodyType(int Id)
        {
            using (EFContext ec = new EFContext())
            {
                BodyType oneBodyType = ec.BDSetBodyType.Find(Id);
                return oneBodyType;
            }
        }

    }
}
