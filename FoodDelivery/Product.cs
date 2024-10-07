﻿namespace FoodDelivery
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Price:C}"; // Форматирование для вывода
        }
    }
}