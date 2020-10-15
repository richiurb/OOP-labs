using System;

namespace lab2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var company = new Company();

            var store1 = company.AddStore("s1", string.Empty);
            var store2 = company.AddStore("s2", string.Empty);
            var item1 = company.AddItem("i1");

            store1.AddStockItems(item1, 100, 10);

            var menu = new Menu();

            while (true)
            {
                Console.Clear();
                menu.Show();

                var keyInfo = Console.ReadKey(true);
                var actionResult = menu.RunAction(keyInfo.Key, company);

                if (!actionResult)
                {
                    break;
                }
            }
        }
    }
}
