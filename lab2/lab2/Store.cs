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

        public void AddStockItems(Item item, int quantity, decimal cost)
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

        public bool TryRemoveStockItems(Dictionary<int, int> stockItems, out decimal totalCost)
        {
            totalCost = 0;

            if (stockItems.Any(item => !ValidateItem(item.Key, item.Value)))
            {
                return false;
            }

            foreach (var item in stockItems)
            {
                var stockItem = _stock[item.Key];

                totalCost += stockItem.Cost * item.Value;

                if (item.Value == stockItem.Quantity)
                {
                    _stock.Remove(item.Key);
                }
                else
                {
                    stockItem.Quantity -= item.Value;
                }
            }

            return true;
        }

        public bool TryEstimateStockItems(Dictionary<int, int> stockItems, out decimal totalCost)
        {
            totalCost = 0;

            foreach (var item in stockItems)
            {
                if (!ValidateItem(item.Key, item.Value))
                {
                    return false;
                }

                totalCost += _stock[item.Key].Cost * item.Value;
            }

            return true;
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

        private bool ValidateItem(int itemId, int quantity)
        {
            return _stock.TryGetValue(itemId, out var stockItem) && stockItem.Quantity >= quantity;
        }
    }
}
