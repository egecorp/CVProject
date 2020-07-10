using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestAppCore.Models;
using System.Data.Entity;

namespace TestAppCore.Data
{
    public class EFContext : DbContext
    {
        public EFContext() : base("TestAppDB")
        { }

        public DbSet<BodyType> BDSetBodyType { set; get; }

        public DbSet<Brand> BDSetBrand { set; get; }

        public DbSet<Car> BDSetCar { set; get; }

        public DbSet<CarImage> BDSetCarImage { set; get; }
    }
}
