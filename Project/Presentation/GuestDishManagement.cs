
using System;

public class GuestDishManagement
{
    private static DishLogic _dishLogic = new DishLogic();
    private static UserDishLogic _userDishLogic = new UserDishLogic();

    public static void Start(UserModel guest)
    {
        string choice;

        do
        {
            Console.Clear();
            Console.WriteLine($"===== Dishes - Welcome {guest.FullName} =====");
            Console.WriteLine("1. View available dishes");
            Console.WriteLine("2. View my ordered dishes");
            Console.WriteLine("0. Back to guest menu");
            Console.Write("\nChoose an option: ");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SelectDish(guest);
                    break;

                case "2":
                    ViewMyDishes(guest);
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

    private static void SelectDish(UserModel guest)
    {
        Console.Clear();
        Console.WriteLine("===== Available Dishes =====");

        var dishes = _dishLogic.GetAvailable();

        if (dishes.Count == 0)
        {
            Console.WriteLine("No available dishes.");
            Pause();
            return;
        }

        foreach (var dish in dishes)
        {
            Console.WriteLine($"{dish.ID}. {dish.Name} - {dish.Price:C}");
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

        if (selectedDish == null || !selectedDish.IsAvailable)
        {
            Console.WriteLine("Dish not found or unavailable!");
            Pause();
            return;
        }

        DishOptions(selectedDish, guest);
    }

    private static void DishOptions(DishModel dish, UserModel guest)
    {
        string choice;

        do
        {
            Console.Clear();
            Console.WriteLine($"===== {dish.Name} =====");
            Console.WriteLine("1. View dish");
            Console.WriteLine("2. Order dish");
            Console.WriteLine("0. Back");
            Console.Write("\nChoose an option: ");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewDish(dish);
                    break;

                case "2":
                    OrderDish(dish, guest);
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

        Pause();
    }

    private static void OrderDish(DishModel dish, UserModel guest)
    {
        var orders = _userDishLogic.GetByUserID(guest.Id);

        if (orders.Exists(o => o.Dish_Id == dish.ID))
        {
            Console.WriteLine("\nYou have already ordered this dish!");
            Pause();
            return;
        }

        DishModel currentDish = _dishLogic.GetByID(dish.ID);

        if (currentDish == null || !currentDish.IsAvailable)
        {
            Console.WriteLine("\nThis dish is no longer available!");
            Pause();
            return;
        }

        UserDishModel userDish = new UserDishModel(
            0,
            guest.Id,
            dish.ID
        );

        _userDishLogic.Add(userDish);

        Console.WriteLine("\nDish ordered successfully!");
        Pause();
    }

    private static void ViewMyDishes(UserModel guest)
    {
        Console.Clear();
        Console.WriteLine("===== My Ordered Dishes =====");

        var orders = _userDishLogic.GetByUserID(guest.Id);

        if (orders.Count == 0)
        {
            Console.WriteLine("You haven't ordered any dishes yet.");
            Pause();
            return;
        }

        foreach (var order in orders)
        {
            DishModel dish = _dishLogic.GetByID(order.Dish_Id);

            if (dish != null)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine($"ID: {dish.ID}");
                Console.WriteLine($"Name: {dish.Name}");
                Console.WriteLine($"Type: {dish.Type}");
                Console.WriteLine($"Description: {dish.Description}");
                Console.WriteLine($"Price: {dish.Price:C}");
            }
        }

        Pause();
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
