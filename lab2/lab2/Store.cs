using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class Store
    {
        private readonly Dictionary<int, StockItem> _stock;

        public Store(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;

            _stock = new Dictionary<int, StockItem>();
        }

        public int Id { get; }

        public string Name { get; }

        public string Address { get; }

        public void AddStockItems(Item item, double quantity, decimal cost)
        {
            if (_stock.ContainsKey(item.Id))
            {
                _stock[item.Id].Cost = cost;
                _stock[item.Id].Quantity += quantity;
            }
            else
            {
                _stock.Add(item.Id, new StockItem(item, cost, quantity));
            }
        }

        public StockItem GetStockItem(int itemId)
        {
            _stock.TryGetValue(itemId, out var item);
            return item;
        }

        public StockItem[] GetAllStockItems()
        {
            return _stock.Values.ToArray();
        }
    }
}
