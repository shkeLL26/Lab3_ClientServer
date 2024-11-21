using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public interface ICarBrand
    {
        string Brand { get; set; }
        string Model { get; set; }
        int HorsePower { get; set; }
        int MaxSpeed { get; set; }
        string RegistrationNumber { get; set; }
        string Type { get; set; }
    }
}
