public class OrderedItem
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }

    public OrderedItem(int itemId, int quantity)
    {
        ItemId = itemId;
        Quantity = quantity;
    }
}


