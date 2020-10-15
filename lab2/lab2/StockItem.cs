namespace lab2
{
    public class StockItem
    {
        public StockItem(Item item, decimal cost, double quantity)
        {
            Item = item;
            Cost = cost;
            Quantity = quantity;
        }

        public Item Item { get; }

        public decimal Cost { get; set; }

        public double Quantity { get; set; }
    }
}
