public static class MenuManagement
{
    private static readonly MenuLogic menuLogic = new();

    public static void Start()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("     MENU MANAGEMENT DASHBOARD");
            Console.WriteLine("================================");
            Console.WriteLine("1. Add a dish");
            Console.WriteLine("2. View all dishes");
            Console.WriteLine("3. Update a dish");
            Console.WriteLine("4. Delete a dish");
            Console.WriteLine("0. Return to admin menu");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1": AddDish(); break;
                case "2": ListDishes(); break;
                case "3": UpdateDish(); break;
                case "4": DeleteDish(); break;
                case "0":Menu.Start(); break;
                default: ShowMessage("Invalid choice.", false); break;
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to return to menu management...");
                Console.ReadKey();
            }
        }
    }

    private static void AddDish()
    {
        Console.Clear();
        Console.Write("Dish name: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Type/category: ");
        string type = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Description: ");
        string description = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Price: ");
        if (string.IsNullOrWhiteSpace(name) || !TryReadPrice(out double price))
        {
            ShowMessage("Enter a dish name and a valid non-negative price.", false);
            return;
        }

        ShowMessage(menuLogic.WriteMenu(name, type, description, price)
            ? "Dish added successfully!" : "Failed to add dish!", true);
    }

    public static void ListDishes()
    {
        Console.Clear();
        Console.WriteLine("MENU DISHES");
        foreach (MenuModel dish in menuLogic.GetAllMenus())
            PrintDish(dish);
    }

    private static void UpdateDish()
    {
        Console.Clear();
        MenuModel dish = FindDish();
        if (dish == null) return;

        Console.WriteLine($"Name [{dish.Name}]: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        Console.WriteLine($"Type/category [{dish.Type}]: ");
        string type = Console.ReadLine()?.Trim() ?? "";
        Console.WriteLine($"Description [{dish.Description}]: ");
        string description = Console.ReadLine()?.Trim() ?? "";
        Console.WriteLine($"Price [{dish.Price}]: ");
        string priceInput = Console.ReadLine() ?? "";
        double price = dish.Price;
        if (!string.IsNullOrWhiteSpace(priceInput) &&
            (!double.TryParse(priceInput, out price) || price < 0))
        {
            ShowMessage("Enter a valid non-negative price.", false);
            return;
        }

        bool result = menuLogic.UpdateMenu(dish.ID,
            string.IsNullOrWhiteSpace(name) ? dish.Name : name,
            string.IsNullOrWhiteSpace(type) ? dish.Type : type,
            string.IsNullOrWhiteSpace(description) ? dish.Description : description,
            price);
        ShowMessage(result ? "Dish updated successfully!" : "Failed to update dish!", result);
    }

    private static void DeleteDish()
    {
        Console.Clear();
        MenuModel dish = FindDish();
        if (dish == null) return;
        Console.Write($"Delete '{dish.Name}'? (y/n): ");
        if ((Console.ReadLine() ?? "").Equals("y", StringComparison.OrdinalIgnoreCase))
            ShowMessage(menuLogic.DeleteMenuByID(dish.ID)
                ? "Dish deleted successfully!" : "Failed to delete dish!", true);
        else
            Console.WriteLine("Deletion cancelled.");
    }

    private static MenuModel FindDish()
    {
        Console.Write("Dish ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            ShowMessage("Invalid dish ID.", false);
            return null;
        }
        MenuModel dish = menuLogic.SearchMenuByID(id);
        if (dish == null) ShowMessage("Dish not found.", false);
        else PrintDish(dish);
        return dish;
    }

    private static bool TryReadPrice(out double price)
    {
        return double.TryParse(Console.ReadLine(), out price) && price >= 0;
    }

    private static void PrintDish(MenuModel dish)
    {
        Console.WriteLine($"\nID: {dish.ID} | {dish.Name} | {dish.Type} | {dish.Price:C}");
        Console.WriteLine(dish.Description);
    }

    private static void ShowMessage(string message, bool success)
    {
        Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
