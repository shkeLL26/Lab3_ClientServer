using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class Truck : ICarBrand
    {
        //свойства автомобиля
        public string Brand { get; set; }
        public string Model { get; set; }
        public int HorsePower { get; set; }
        public int MaxSpeed { get; set; }
        public string RegistrationNumber { get; set; }
        public string Type { get; set; }
        //свойства грузовика
        public int Wheels { get; set; }
        public float Volume { get; set; }

        public Truck(string mBrand, string mModel, int mHorsePower, int mMaxSpeed)
        {
            Brand = mBrand;
            Model = mModel;
            HorsePower = mHorsePower;
            MaxSpeed = mMaxSpeed;
            Type = "Грузовик";
        }

       public Truck()
       { }
    }
}
