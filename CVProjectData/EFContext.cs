using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CVProject.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


using System.IO;

namespace CVProject.Data
{
    public class EFContext : DbContext
    {
        public EFContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectString());
        }


        public DbSet<BodyType> BDSetBodyType { set; get; }

        public DbSet<Brand> BDSetBrand { set; get; }

        public DbSet<Car> BDSetCar { set; get; }

        public DbSet<CarImage> BDSetCarImage { set; get; }


        private static bool IsFirstConnectionSolved = false;

        private static object mLock = new object();

        public static EFContext Get()
        {
            lock (mLock)
            {
                if (!IsFirstConnectionSolved)
                {
                    IsFirstConnectionSolved = true;
                    InitDB();
                    //"TestAppDB"
                }
            }
            return new EFContext();
        }



        private static string myConnectString = null;

        private static string GetConnectString()
        {
            if (myConnectString != null) return myConnectString;

            DBConfig conf = DBConfig.Get();

            return myConnectString = conf.ConnectionString;
            //return @"Server=(localdb)\MSSQLLocalDB;Database=CBProjectDB;Trusted_Connection=True;";
        }


        private static void InitDB()
        {
            using (EFContext ec = new EFContext())
            {
                bool NeedSave = false;


                if (ec.BDSetBrand.Count() < 1)
                {
                    DBConfig conf = DBConfig.Get();

                    List<Brand> defaultBrands = conf.DefaultBrand;

                    if (defaultBrands != null)
                    {
                        ec.BDSetBrand.AddRange(defaultBrands);
                    }
                    NeedSave = true;
                }

                if (ec.BDSetBodyType.Count() < 1)
                {
                    DBConfig conf = DBConfig.Get();

                    List<BodyType> defaultBodyTypes = conf.DefaultBodyType; 

                    if (defaultBodyTypes != null)
                    {
                        ec.BDSetBodyType.AddRange(defaultBodyTypes);
                    }
                    NeedSave = true;
                }


                if (NeedSave) ec.SaveChanges();

            }
        }

    }
}
