using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class Company
    {
        private int _lastItemId;
        private int _lastStoreId;

        private readonly Dictionary<int, Item> _items;
        private readonly Dictionary<int, Store> _stores;

        public Company()
        {
            _lastItemId = 0;
            _lastStoreId = 0;

            _items = new Dictionary<int, Item>();
            _stores = new Dictionary<int, Store>();
        }

        public Item AddItem(string name)
        {
            var id = _lastItemId++;
            var item = new Item(id, name);

            _items.Add(id, item);

            return item;
        }

        public Store AddStore(string name, string address)
        {
            var id = _lastStoreId++;
            var store = new Store(id, name, address);

            _stores.Add(id, store);

            return store;
        }

        public Item[] GetAllItems()
        {
            return _items.Values.ToArray();
        }

        public Store[] GetAllStores()
        {
            return _stores.Values.ToArray();
        }

        public Item GetItem(int id)
        {
            _items.TryGetValue(id, out var item);
            return item;
        }

        public Store GetStore(int id)
        {
            _stores.TryGetValue(id, out var store);
            return store;
        }
    }
}
