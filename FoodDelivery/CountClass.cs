using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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


