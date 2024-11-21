using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class PassengerCar : ICarBrand
    {
        //свойства автомобиля
        public string Brand { get; set; }
        public string Model { get; set; }
        public int HorsePower { get; set; }
        public int MaxSpeed { get; set; }
        public string RegistrationNumber { get; set; }
        public string Type { get; set; }
        //свойства легковой машины
        public string Multimedia { get; set; }
        public int Airbags { get; set; }

        public PassengerCar(string mBrand, string mModel, int mHorsePower, int mMaxSpeed)
        {
            Brand = mBrand;
            Model = mModel;
            HorsePower = mHorsePower;
            MaxSpeed = mMaxSpeed;
            Type = "Легковой";
        }
        //public PassengerCar() {};
        public PassengerCar()
        { }
    }
}
