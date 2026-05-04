using System.Globalization;

namespace ShopEvents;

public class Program
{
    public static void Main(string[] args)
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        Console.WriteLine("--- Setting up the shop simulation ---");

        var laptop = new Product("Laptop", 1200.00m, 5);
        var mouse = new Product("Mouse", 25.50m, 10);

        var cart = new ShoppingCart();
        var display = new Display();
        var notifier = new Notifier();

        display.Subscribe(cart);
        notifier.Subscribe(laptop);
        notifier.Subscribe(mouse);

        Console.WriteLine("\n--- Simulation starts ---\n");

        Console.WriteLine(">>> ACTION: Adding 1 Laptop and 2 Mice to the cart.");
        cart.AddItem(laptop);
        cart.AddItem(mouse, 2);

        Console.WriteLine("\n>>> ACTION: Laptop price is increasing to $1350.50.");
        laptop.Price = 1350.50m;

        Console.WriteLine("\n>>> ACTION: Trying to buy the last 4 laptops.");
        cart.AddItem(laptop, 4);

        Console.WriteLine("\n>>> ACTION: Trying to buy one more laptop.");
        cart.AddItem(laptop, 1);

        Console.WriteLine("\n>>> ACTION: Restocking laptops (10 units).");
        laptop.StockQuantity += 10;

        Console.WriteLine("\n>>> ACTION: Removing 1 mouse from the cart.");
        cart.RemoveItem(mouse);

        Console.WriteLine("\n--- Simulation finished ---");
    }
}
