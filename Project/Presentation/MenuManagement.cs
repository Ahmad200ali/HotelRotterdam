public static class MenuManagement
{
    private static readonly MenuLogic menuLogic = new();
    private static readonly DishTypeLogic dishTypeLogic = new();

    public static void Start()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("     MENU MANAGEMENT DASHBOARD");
            Console.WriteLine("================================");
            Console.WriteLine("1. View all dishes");
            Console.WriteLine("2. Add a dish");
            Console.WriteLine("3. Manage dish types");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1": ViewDishes(); break;
                case "2": AddDish(); break;
                case "3": ManageDishTypes(); break;
                case "4": running = false; break;
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
        string type = ChooseDishType(null);
        if (type == null) return;
        Console.Write("Dish name: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Description: ");
        string description = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Price: ");
        if (string.IsNullOrWhiteSpace(name) || !TryReadPrice(out double price))
        {
            ShowMessage("Enter a dish name and a valid non-negative price.", false);
            return;
        }

        bool result = menuLogic.WriteMenu(name, type, description, price);
        ShowMessage(result ? "Dish added successfully!" : "Failed to add dish!", result);
    }

    public static void ListDishes()
    {
        Console.Clear();
        Console.WriteLine("MENU DISHES");
        foreach (MenuModel dish in menuLogic.GetAllMenus())
            PrintDish(dish);
    }

    private static void ViewDishes()
    {
        ListDishes();
        Console.Write("\nEnter a dish ID to update or delete it (or press Enter to go back): ");
        string input = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(input)) return;

        if (!int.TryParse(input, out int id) || id <= 0)
        {
            ShowMessage("Invalid dish ID.", false);
            return;
        }
        MenuModel dish = menuLogic.SearchMenuByID(id);
        if (dish == null)
        {
            ShowMessage("Dish not found.", false);
            return;
        }

        Console.Clear();
        PrintDish(dish);
        Console.WriteLine("\n1. Update this dish");
        Console.WriteLine("2. Delete this dish");
        Console.WriteLine("3. Back");
        Console.Write("Choose an option: ");

        switch (Console.ReadLine())
        {
            case "1": UpdateDish(dish); break;
            case "2": DeleteDish(dish); break;
        }
    }

    public static void ListAvailableDishes()
    {
        Console.Clear();
        Console.WriteLine("AVAILABLE MENU DISHES\n");
        List<MenuModel> dishes = menuLogic.GetAvailableMenus();
        if (dishes.Count == 0)
        {
            Console.WriteLine("There are currently no available dishes.");
            return;
        }

        foreach (MenuModel dish in dishes)
        {
            Console.WriteLine($"{dish.Name} | {dish.Price:C}");
        }
    }

    private static void UpdateDish(MenuModel dish)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press Enter without typing anything to keep the current value.\n");
        Console.ResetColor();

        string type = ChooseDishType(dish.Type);
        if (type == null) return;
        Console.WriteLine($"Name [{dish.Name}]: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        Console.WriteLine($"Description [{dish.Description}]: ");
        string description = Console.ReadLine()?.Trim() ?? "";
        Console.WriteLine($"Available [{(dish.IsAvailable ? "y" : "n")}]? (y/n, Enter to keep current): ");
        string availabilityInput = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(availabilityInput) &&
            !availabilityInput.Equals("y", StringComparison.OrdinalIgnoreCase) &&
            !availabilityInput.Equals("n", StringComparison.OrdinalIgnoreCase))
        {
            ShowMessage("Enter y or n to set availability.", false);
            return;
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"The price has to be entered again! (current price: {dish.Price:C})");
        Console.ResetColor();
        Console.Write("Price: ");
        if (!TryReadPrice(out double price))
        {
            ShowMessage("Enter a valid non-negative price.", false);
            return;
        }

        dish.Name = string.IsNullOrWhiteSpace(name) ? dish.Name : name;
        dish.Type = type;
        dish.Description = string.IsNullOrWhiteSpace(description) ? dish.Description : description;
        dish.Price = price;
        if (!string.IsNullOrWhiteSpace(availabilityInput))
            dish.IsAvailable = availabilityInput.Equals("y", StringComparison.OrdinalIgnoreCase);

        bool result = menuLogic.UpdateMenu(dish);
        ShowMessage(result ? "Dish updated successfully!" : "Failed to update dish!", result);
    }

    private static void DeleteDish(MenuModel dish)
    {
        Console.Write($"Delete '{dish.Name}'? (y/n): ");
        if ((Console.ReadLine() ?? "").Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            bool result = menuLogic.DeleteMenuByID(dish.ID);
            ShowMessage(result ? "Dish deleted successfully!" : "Failed to delete dish!", result);
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    // currentType is null when adding a new dish
    private static string ChooseDishType(string currentType)
    {
        List<DishTypeModel> types = dishTypeLogic.GetAllDishTypes();
        if (types.Count == 0)
        {
            ShowMessage("There are no dish types yet, add one first under 'Manage dish types'.", false);
            return null;
        }

        Console.WriteLine("Dish types:");
        for (int i = 0; i < types.Count; i++)
            Console.WriteLine($"{i + 1}. {types[i].Name}");

        if (currentType != null)
            Console.Write($"Choose a type [{currentType}]: ");
        else
            Console.Write("Choose a type: ");

        string input = Console.ReadLine() ?? "";
        if (currentType != null && string.IsNullOrWhiteSpace(input))
            return currentType;

        if (!int.TryParse(input, out int choice) || choice < 1 || choice > types.Count)
        {
            ShowMessage("Invalid dish type.", false);
            return null;
        }
        return types[choice - 1].Name;
    }

    private static void ManageDishTypes()
    {
        Console.Clear();
        Console.WriteLine("DISH TYPES\n");
        List<DishTypeModel> types = dishTypeLogic.GetAllDishTypes();
        foreach (DishTypeModel type in types)
            Console.WriteLine($"ID: {type.ID} | {type.Name}");

        Console.WriteLine("\n1. Add a dish type");
        Console.WriteLine("2. Delete a dish type");
        Console.WriteLine("3. Back");
        Console.Write("Choose an option: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.Write("Name of the new dish type: ");
                bool added = dishTypeLogic.AddDishType(Console.ReadLine() ?? "");
                ShowMessage(added ? "Dish type added!" : "Dish type is empty or already exists.", added);
                break;
            case "2":
                Console.Write("Dish type ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    ShowMessage("Invalid ID.", false);
                    break;
                }
                if (dishTypeLogic.IsInUse(id))
                {
                    ShowMessage("This dish type is still used by a dish, so it can not be deleted.", false);
                    break;
                }
                bool deleted = dishTypeLogic.DeleteDishType(id);
                ShowMessage(deleted ? "Dish type deleted!" : "Dish type not found.", deleted);
                break;
        }
    }

    private static bool TryReadPrice(out double price)
    {
        bool valid = Check.TryParsePrice(Console.ReadLine() ?? "", out decimal parsedPrice);
        price = (double)parsedPrice;

        return valid && price >= 0;
    }

    private static void PrintDish(MenuModel dish)
    {
        Console.WriteLine($"\nID: {dish.ID} | {dish.Name} | {dish.Type} | {dish.Price:C} | {(dish.IsAvailable ? "Available" : "Unavailable")}");
        Console.WriteLine(dish.Description);
    }

    private static void ShowMessage(string message, bool success)
    {
        Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
