using System;
using System.Linq;

namespace lab2
{
    public static class Management
    {
        public static void AddStore(Company company)
        {
            Console.Write("Введите название нового магазина: ");
            var name = Console.ReadLine();

            Console.Write("Введите адрес нового магазина: ");
            var address = Console.ReadLine();

            company.AddStore(name, address);
        }

        public static void AddItem(Company company)
        {
            Console.Write("Введите название нового товара: ");
            var name = Console.ReadLine();

            company.AddItem(name);
        }

        public static void AddItemsInStock(Company company)
        {
            var storeId = AskForStore(company);
            var store = company.GetStore(storeId);

            if (store == null)
            {
                throw new Exception($"Магазин {storeId} не найден");
            }

            Console.WriteLine();

            var itemId = AskForItem(company);
            var item = company.GetItem(itemId);

            if (item == null)
            {
                throw new Exception($"Товар {itemId} не найден");
            }

            Console.Write("Введите стоимость товара: ");
            var cost = decimal.Parse(Console.ReadLine());

            Console.Write("Введите количество товара: ");
            var quantity = double.Parse(Console.ReadLine());

            store.AddStockItems(item, quantity, cost);
        }

        public static void FindLowestCost(Company company)
        {
            var itemId = AskForItem(company);
            var stores = company.GetAllStores();

            (Store store, decimal? cost) minCostStore = default;

            foreach (var store in stores)
            {
                var cost = store.GetStockItem(itemId)?.Cost;

                if (cost != null && (minCostStore.cost == null || minCostStore.cost > cost))
                {
                    minCostStore = (store, cost);
                }
            }

            if (minCostStore.store == null)
            {
                Console.WriteLine("Товар не найден ни в одном из магазинов");
            }
            else
            {
                Console.WriteLine($"Товар найден в магазине \"{minCostStore.store.Name}\" по цене {minCostStore.cost} за штуку");
            }

            Console.ReadKey();
        }

        private static int AskForStore(Company company)
        {
            var stores = company.GetAllStores();

            foreach (var s in stores)
            {
                Console.WriteLine($"{s.Name} (ID {s.Id}); {s.Address}");
            }

            Console.WriteLine();

            Console.Write("Введите идентификатор магазина: ");
            return int.Parse(Console.ReadLine());
        }

        private static int AskForItem(Company company)
        {
            var items = company.GetAllItems();

            foreach (var i in items)
            {
                Console.WriteLine($"{i.Name} (ID {i.Id})");
            }

            Console.WriteLine();

            Console.Write("Введите идентификатор товара: ");
            return int.Parse(Console.ReadLine());
        }
    }
}
