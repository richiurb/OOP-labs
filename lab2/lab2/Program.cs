namespace lab2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var company = CreateFakeCompany();

            var menu = new CompanyMenu(company);
            menu.Show();
        }

        private static Company CreateFakeCompany()
        {
            var company = new Company();

            var store1 = company.AddStore("Магазин на Стачек", "Стачек пр. 103");
            var store2 = company.AddStore("Магазин на Стачек", "Стачек пр. 99");
            var store3 = company.AddStore("Магазин на Ленинском", "Ленинский пр. 116");

            var apple = company.AddItem("Яблоко");
            var orange = company.AddItem("Апельсин");
            var grape = company.AddItem("Виноград 1 кг.");
            var pinapple = company.AddItem("Ананас");
            var milk = company.AddItem("Молоко 1 л.");
            var bread = company.AddItem("Хлеб нарезка");
            var cola05 = company.AddItem("Coca-Cola 0.5 л.");
            var cola10 = company.AddItem("Coca-Cola 1 л.");
            var cola15 = company.AddItem("Coca-Cola 1.5 л.");
            var iphone = company.AddItem("iPhone 12 Pro Max 512 Gb");

            store1.AddStockItems(apple, 100, 25);
            store2.AddStockItems(apple, 150, 24.99m);
            store3.AddStockItems(apple, 250, 30);

            store1.AddStockItems(orange, 75, 30);
            store2.AddStockItems(orange, 100, 29.99m);
            store3.AddStockItems(orange, 125, 36);

            store2.AddStockItems(grape, 5, 150.99m);
            store3.AddStockItems(grape, 6, 140);

            store1.AddStockItems(pinapple, 3, 250);
            store2.AddStockItems(pinapple, 5, 249.99m);

            store1.AddStockItems(milk, 40, 50);
            store2.AddStockItems(milk, 25, 55.99m);
            store3.AddStockItems(milk, 20, 60);

            store1.AddStockItems(bread, 30, 39);
            store2.AddStockItems(bread, 28, 38.99m);
            store3.AddStockItems(bread, 16, 35);

            store1.AddStockItems(cola05, 100, 50);
            store2.AddStockItems(cola05, 110, 55.99m);
            store3.AddStockItems(cola05, 200, 60.01m);

            store1.AddStockItems(cola10, 100, 90);
            store2.AddStockItems(cola10, 110, 100);
            store3.AddStockItems(cola10, 200, 119);

            store1.AddStockItems(cola15, 100, 100);

            store1.AddStockItems(iphone, 20, 139990);
            store2.AddStockItems(iphone, 25, 139990);
            store3.AddStockItems(iphone, 10, 139990);

            return company;
        }
    }
}
