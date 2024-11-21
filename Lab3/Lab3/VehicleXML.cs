using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    [Serializable] public class VehicleXML
    {
        public List<PassengerCar> PassCarList = new List<PassengerCar>();
        public List<Truck> TruckList = new List<Truck>();
        public VehicleXML()
        {

        }
    }
}
