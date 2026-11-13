
using System;

public class DishManagement
{
    private static DishLogic _dishLogic = new DishLogic();

    public static void Start()
    {
        string choice;

        do
        {
            Console.Clear();
            Console.WriteLine("===== Dish Management =====");
            Console.WriteLine("1. Add dish");
            Console.WriteLine("2. Manage dishes");
            Console.WriteLine("0. Back to admin menu");
            Console.Write("\nChoose an option: ");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddDish();
                    break;

                case "2":
                    ManageDishes();
                    break;

                case "0":
                    break;

                default:
                    Console.WriteLine("Invalid option!");
                    Pause();
                    break;
            }

        } while (choice != "0");
    }

    private static void AddDish()
    {
        Console.Clear();
        Console.WriteLine("===== Add Dish =====");

        Console.Write("Dish name: ");
        string name = Console.ReadLine();

        Console.Write("Dish type: ");
        string type = Console.ReadLine();

        Console.Write("Description: ");
        string description = Console.ReadLine();

        Console.Write("Price: ");

        if (!double.TryParse(Console.ReadLine(), out double price) || price < 0)
        {
            Console.WriteLine("Invalid price!");
            Pause();
            return;
        }

        DishModel dish = new DishModel(
            0, name, type, description, price
        );

        _dishLogic.Add(dish);

        Console.WriteLine("\nDish added successfully!");
        Pause();
    }

    private static void ManageDishes()
    {
        Console.Clear();
        Console.WriteLine("===== Select a Dish =====");

        var dishes = _dishLogic.GetAll();

        if (dishes.Count == 0)
        {
            Console.WriteLine("No dishes available.");
            Pause();
            return;
        }

        foreach (var dish in dishes)
        {
            Console.WriteLine($"{dish.ID}. {dish.Name}");
        }

        Console.WriteLine("0. Back");
        Console.Write("\nSelect dish ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            Pause();
            return;
        }

        if (id == 0)
            return;

        DishModel selectedDish = _dishLogic.GetByID(id);

        if (selectedDish == null)
        {
            Console.WriteLine("Dish not found!");
            Pause();
            return;
        }

        DishOptions(selectedDish);
    }

    private static void DishOptions(DishModel dish)
    {
        string choice;

        do
        {
            Console.Clear();
            Console.WriteLine($"===== {dish.Name} =====");
            Console.WriteLine("1. View dish");
            Console.WriteLine("2. Update dish");
            Console.WriteLine("3. Delete dish");
            Console.WriteLine("0. Back");
            Console.Write("\nChoose an option: ");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewDish(dish);
                    break;

                case "2":
                    UpdateDish(dish);
                    break;

                case "3":
                    DeleteDish(dish);
                    return;

                case "0":
                    break;

                default:
                    Console.WriteLine("Invalid option!");
                    Pause();
                    break;
            }

        } while (choice != "0");
    }

    private static void ViewDish(DishModel dish)
    {
        Console.Clear();
        Console.WriteLine("===== Dish Details =====");
        Console.WriteLine($"ID: {dish.ID}");
        Console.WriteLine($"Name: {dish.Name}");
        Console.WriteLine($"Type: {dish.Type}");
        Console.WriteLine($"Description: {dish.Description}");
        Console.WriteLine($"Price: {dish.Price:C}");
        Console.WriteLine($"Available: {(dish.IsAvailable ? "Yes" : "No")}");

        Pause();
    }

    private static void UpdateDish(DishModel dish)
    {
        Console.Clear();
        Console.WriteLine("===== Update Dish =====");

        Console.Write($"New name ({dish.Name}): ");
        string name = Console.ReadLine();

        Console.Write($"New type ({dish.Type}): ");
        string type = Console.ReadLine();

        Console.Write($"New description ({dish.Description}): ");
        string description = Console.ReadLine();

        Console.Write($"New price ({dish.Price}): ");
        string priceInput = Console.ReadLine();

        Console.Write($"Available? (y/n, current: {(dish.IsAvailable ? "Yes" : "No")}): ");
        string availability = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
            dish.Name = name;

        if (!string.IsNullOrWhiteSpace(type))
            dish.Type = type;

        if (!string.IsNullOrWhiteSpace(description))
            dish.Description = description;

        if (!string.IsNullOrWhiteSpace(priceInput))
        {
            if (double.TryParse(priceInput, out double price) && price >= 0)
                dish.Price = price;
            else
            {
                Console.WriteLine("Invalid price!");
                Pause();
                return;
            }
        }

        if (availability?.ToLower() == "y")
            dish.IsAvailable = true;
        else if (availability?.ToLower() == "n")
            dish.IsAvailable = false;

        _dishLogic.Update(dish);

        Console.WriteLine("\nDish updated successfully!");
        Pause();
    }

    private static void DeleteDish(DishModel dish)
    {
        Console.Write($"Are you sure you want to delete {dish.Name}? (y/n): ");

        string confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "y")
        {
            bool deleted = _dishLogic.Delete(dish.ID);

            Console.WriteLine(deleted
                ? "Dish deleted successfully!"
                : "Dish could not be deleted.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }

        Pause();
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
