using System;
using System.Collections.Generic;

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

            Console.WriteLine("Магазин успешно добавлен!");
        }

        public static void AddItem(Company company)
        {
            Console.Write("Введите название нового товара: ");
            var name = Console.ReadLine();

            company.AddItem(name);

            Console.WriteLine("Товар успешно добавлен!");
        }

        public static void AddItemsInStock(Company company)
        {
            PrintAllStores(company);
            var store = AskForStore(company);

            Console.WriteLine();

            PrintAllCompanyItems(company);
            var item = AskForCompanyItem(company);
            var cost = AskForCost();
            var quantity = AskForQuantity();

            store.AddStockItems(item, quantity, cost);

            Console.WriteLine("Товар успешно добавлен в магазин!");
        }

        public static void FindLowestItemCost(Company company)
        {
            PrintAllCompanyItems(company);
            var item = AskForCompanyItem(company);

            var stores = company.GetAllStores();
            (Store store, decimal? cost) minCostStore = default;

            foreach (var store in stores)
            {
                var cost = store.GetStockItem(item.Id)?.Cost;

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
                Console.WriteLine($"Товар найден в магазине \"{minCostStore.store.Name}\" ({minCostStore.store.Address}) по цене {minCostStore.cost} у.е. за штуку");
            }
        }

        public static void FindLowestCartCost(Company company)
        {
            PrintAllCompanyItems(company);

            var cart = new Dictionary<int, int>();
            var exit = false;

            while (!exit)
            {
                var item = AskForCompanyItem(company);
                var quantity = AskForQuantity();

                AddOrUpdateItemInCart(cart, item.Id, quantity);

                exit = !AskToAddOneMoreItemToCart();
                Console.WriteLine();
            }

            var stores = company.GetAllStores();
            (Store store, decimal? totalCost) minCostStore = default;

            foreach (var store in stores)
            {
                if (store.TryEstimateStockItems(cart, out var totalCost) 
                    && (minCostStore.totalCost == null || minCostStore.totalCost > totalCost))
                {
                    minCostStore = (store, totalCost);
                }
            }

            if (minCostStore.store == null)
            {
                Console.WriteLine("Партия товаров не найдена ни в одном из магазинов");
            }
            else
            {
                Console.WriteLine($"Партия товаров найдена в магазине \"{minCostStore.store.Name}\" ({minCostStore.store.Address}) по общей стоимости {minCostStore.totalCost} у.е.");
            }
        }

        public static void GetFixedCostItems(Company company)
        {
            PrintAllStores(company);
            var store = AskForStore(company);
            var maxTotal = AskForCost();

            var stockItems = store.GetAllStockItems();

            foreach (var stockItem in stockItems)
            {
                var itemQuantity = Math.Floor(maxTotal / stockItem.Cost);

                if (itemQuantity < 1)
                {
                    continue;
                }

                itemQuantity = Math.Min(itemQuantity, stockItem.Quantity);

                var itemTotalCost = itemQuantity * stockItem.Cost;

                Console.WriteLine($"\"{stockItem.Item.Name}\" в количестве {itemQuantity} шт. по общей стоимости {itemTotalCost} у.е.");
            }
        }

        public static void BuyItems(Company company)
        {
            PrintAllStores(company);
            var store = AskForStore(company);

            PrintAllStoreItems(store);

            var cart = new Dictionary<int, int>();
            var exit = false;

            while (!exit)
            {
                var stockItem = AskForStoreStockItem(store);
                var quantity = AskForQuantity();

                AddOrUpdateItemInCart(cart, stockItem.Item.Id, quantity);

                exit = !AskToAddOneMoreItemToCart();
                Console.WriteLine();
            }

            if (!store.TryRemoveStockItems(cart, out var totalCost))
            {
                Console.WriteLine("Не все товары присутствуют в магазине");
            }
            else
            {
                Console.WriteLine($"Покупка успешно завершена. Общая стоимость: {totalCost} у.е.");
            }
        }

        private static void AddOrUpdateItemInCart(Dictionary<int, int> cart, int itemId, int quantity)
        {
            if (cart.ContainsKey(itemId))
            {
                cart[itemId] += quantity;
            }
            else
            {
                cart.Add(itemId, quantity);
            }
        }

        private static void PrintAllCompanyItems(Company company)
        {
            var items = company.GetAllItems();

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name} (ID {item.Id})");
            }

            Console.WriteLine();
        }

        private static void PrintAllStoreItems(Store store)
        {
            var stockItems = store.GetAllStockItems();

            foreach (var stockItem in stockItems)
            {
                Console.WriteLine($"{stockItem.Item.Name} (ID {stockItem.Item.Id}) стоимостью {stockItem.Cost} у.е. в количестве {stockItem.Quantity} шт.");
            }

            Console.WriteLine();
        }

        private static void PrintAllStores(Company company)
        {
            var stores = company.GetAllStores();

            foreach (var s in stores)
            {
                Console.WriteLine($"{s.Name} (ID {s.Id}); {s.Address}");
            }

            Console.WriteLine();
        }

        private static Item AskForCompanyItem(Company company)
        {
            Console.Write("Введите идентификатор товара: ");
            var itemId = int.Parse(Console.ReadLine());

            var item = company.GetItem(itemId);

            if (item == null)
            {
                throw new Exception($"Товар {itemId} не найден");
            }

            return item;
        }

        private static StockItem AskForStoreStockItem(Store store)
        {
            Console.Write("Введите идентификатор товара: ");
            var itemId = int.Parse(Console.ReadLine());

            var stockItem = store.GetStockItem(itemId);

            if (stockItem == null)
            {
                throw new Exception($"Товар {itemId} не найден");
            }

            return stockItem;
        }

        private static Store AskForStore(Company company)
        {
            Console.Write("Введите идентификатор магазина: ");
            var storeId = int.Parse(Console.ReadLine());

            var store = company.GetStore(storeId);

            if (store == null)
            {
                throw new Exception($"Магазин {storeId} не найден");
            }

            return store;
        }

        private static int AskForQuantity()
        {
            Console.Write("Введите количество товара: ");
            var quantity = int.Parse(Console.ReadLine());

            if (quantity <= 0)
            {
                throw new Exception("Количество должно быть больше 0 шт.");
            }

            return quantity;
        }

        private static decimal AskForCost()
        {
            Console.Write("Введите стоимость товара: ");
            var cost = decimal.Parse(Console.ReadLine());

            if (cost <= 0)
            {
                throw new Exception("Стоимость должна быть больше 0 у.е.");
            }

            return cost;
        }

        private static bool AskToAddOneMoreItemToCart()
        {
            Console.Write("Добавить еще один товар? y/n");
            var keyInfo = Console.ReadKey(true);

            return keyInfo.Key == ConsoleKey.Y;
        }
    }
}
