using System;
using System.Collections.Generic;

namespace lab2
{
    public class CompanyMenu
    {
        private readonly Dictionary<ConsoleKey, (string title, Action<Company> action)> _menuItems = 
            new Dictionary<ConsoleKey, (string, Action<Company>)>
            {
                { ConsoleKey.D1, ("Добавить новый магазин", Management.AddStore) },
                { ConsoleKey.D2, ("Добавить новый товар", Management.AddItem) },
                { ConsoleKey.D3, ("Завезти партию товара в магазин", Management.AddItemsInStock) },
                { ConsoleKey.D4, ("Найти наилучшую цену товара из всех магазинов", Management.FindLowestItemCost) },
                { ConsoleKey.D5, ("Узнать, какой товар и в каком количестве можно купить на определённую сумму", Management.GetFixedCostItems) },
                { ConsoleKey.D6, ("Купить партию товаров в магазине", Management.BuyItems) },
                { ConsoleKey.D7, ("Найти, в каком магазине партия товаров имеет наименьшую сумму", Management.FindLowestCartCost) },
                { ConsoleKey.D8, ("Выход", null) }
            };

        private readonly Company _company;

        public CompanyMenu(Company company)
        {
            _company = company;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                PrintMenuItems();

                var keyInfo = Console.ReadKey(true);

                Console.Clear();

                try
                {
                    var actionResult = RunAction(keyInfo.Key, _company);

                    if (!actionResult)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.ReadKey(true);
            }
        }

        private void PrintMenuItems()
        {
            Console.WriteLine("Добро пожаловать!");

            var index = 0;

            foreach (var (title, _) in _menuItems.Values)
            {
                Console.WriteLine($"{++index}. {title}");
            }
        }

        private bool RunAction(ConsoleKey key, Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            if (!_menuItems.TryGetValue(key, out var item))
            {
                throw new ArgumentException("Выбран несуществующий пункт меню");
            }

            if (item.action == null)
            {
                return false;
            }

            item.action(company);
            return true;
        }
    }
}
