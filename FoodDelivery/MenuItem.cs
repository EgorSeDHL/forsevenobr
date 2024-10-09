namespace FoodDelivery
{
    public class MenuItem
    {
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int Weight { get; set; }
        public MenuItem(int itemId, int restaurantId, string name, string description, decimal price, bool isAvailable, int weight)
        {
            ItemId = itemId;
            RestaurantId = restaurantId;
            Name = name;
            Description = description;
            Price = price;
            IsAvailable = isAvailable;
            Weight = weight;
        }


    }

}
