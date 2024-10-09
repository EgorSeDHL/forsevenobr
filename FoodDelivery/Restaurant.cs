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
