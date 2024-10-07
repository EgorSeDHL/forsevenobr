using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name; // Для отображения только названия в ComboBox
        }
    }

}
