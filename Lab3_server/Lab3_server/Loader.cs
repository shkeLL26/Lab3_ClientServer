using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_server
{
    internal class Loader
    {
        static int prog;
        public static async Task<string> load(string[] recievedModels, int progElem, int num)
        {
            string[][] ModelArray = (from p in recievedModels select p.Split('!')).ToArray();
            string result;
            int index;
            Random random = new Random();
            index = random.Next(0, ModelArray.Count());
            await Task.Delay(random.Next(0, 501));
            prog = (progElem + 1) * 100 / num;
            if (ModelArray[index][1] == "Легковой")
            {
                result = ModelArray[index][0] + '!' + ModelArray[index][1];
                //currentCar = modelList[index] as PassengerCar;
                int mult = random.Next(0, 2);
                if (mult == 0) result = result + '!' + ModelArray[index][0] + " multimedia";
                else result = result + '!' + "none";
                mult = random.Next(0, 2);
                if (mult == 0) result = result + '!' + '2';
                else result = result + '!' + '0';
                result = result + '!' + (char)random.Next('А', 'Я' + 1) + random.Next(0, 10).ToString() + random.Next(0, 10).ToString() + random.Next(0, 10).ToString() + (char)random.Next('А', 'Я' + 1)
                    + (char)random.Next('А', 'Я' + 1) + random.Next(0, 10).ToString() + random.Next(0, 10).ToString() + random.Next(0, 10).ToString();
            }
            else
            {
                result = ModelArray[index][0] + '!' + ModelArray[index][1];
                int mult = random.Next(8, 15);
                int mult10 = random.Next(0, 10);
                result = result + '!' + (mult + (float)(mult10 / 10)).ToString();
                mult = random.Next(0, 2);
                if (mult == 0) result = result + '!' + '4';
                else result = result + '!' + '6';
                result = result + '!' + (char)random.Next('А', 'Я' + 1) + random.Next(0, 10).ToString() + random.Next(0, 10).ToString() + random.Next(0, 10).ToString() + (char)random.Next('А', 'Я' + 1)
                    + (char)random.Next('А', 'Я' + 1) + random.Next(0, 10).ToString() + random.Next(0, 10).ToString() + random.Next(0, 10).ToString();
 
            }
            result = result + '!' + prog.ToString();
            return result;
        }
        public static int getProgress()
        {
            return prog;
        }
    }
}
