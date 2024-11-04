using System;

namespace FoodDelivery
{
    public class Order
    {
        public String address { get; set; }
        public int id { get; set; }
        public double weight { get; set; }
        public override string ToString()
        {
            return "Адрес: " + address + " Вес: " + weight; // Для отображения только названия в ComboBox
        }
    }

}

