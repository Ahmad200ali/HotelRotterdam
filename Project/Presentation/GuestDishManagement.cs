
using System;

public class GuestDishManagement
{
    private static DishLogic _dishLogic = new DishLogic();
    private static UserDishLogic _userDishLogic = new UserDishLogic();

    public static void Start(UserModel guest)
    {
        List<string> options = new List<string>
        {
            "Order a dish",
            "My orders",
            "Back"
        };

        while (true)
        {
            int choice = ArrowMenu.Show("ROOM SERVICE", options);

            switch (choice)
            {
                case 0:
                    SelectDish(guest);
                    break;
                case 1:
                    ViewMyDishes(guest);
                    break;
                default:
                    // Back or Esc
                    return;
            }
        }
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
            Console.WriteLine($"{dish.ID}. {dish.Name}");
            Console.WriteLine($"   Description: {dish.Description}");
            Console.WriteLine($"   Price: € {dish.Price:0.00}");
        }

        Console.WriteLine("0. Back");
        Console.Write("\nSelect number: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid number!");
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
        Console.WriteLine($"Price: € {dish.Price:0.00}");

        Pause();
    }

    private static void OrderDish(DishModel dish, UserModel guest)
    {
        int count = 1;

        DishModel currentDish = _dishLogic.GetByID(dish.ID);

        if (currentDish == null || !currentDish.IsAvailable)
        {
            Console.WriteLine("\nThis dish is no longer available!");
            Pause();
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"===== Order {dish.Name} =====");
            Console.WriteLine($"Count: {count}");
            Console.WriteLine("1. Change count");
            Console.WriteLine("2. Place order");
            Console.WriteLine("0. Cancel");
            Console.Write("\nChoose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Enter count (at least 1): ");
                    if (int.TryParse(Console.ReadLine(), out int newCount) && newCount > 0)
                        count = newCount;
                    else
                    {
                        Console.WriteLine("Please enter a valid count greater than zero.");
                        Pause();
                    }
                    break;

                case "2":
                    Console.WriteLine(_userDishLogic.PlaceOrder(guest.Id, currentDish, count)
                        ? "\nDish order placed successfully!"
                        : "\nCould not place the order.");
                    Pause();
                    return;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option!");
                    Pause();
                    break;
            }
        }
    }

    private static void ViewMyDishes(UserModel guest)
    {
        // after cancelling an order the list is shown again
        while (true)
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

            for (int i = 0; i < orders.Count; i++)
            {
                DishModel dish = _dishLogic.GetByID(orders[i].Dish_Id);
                string dishName = dish != null ? dish.Name : "Dish no longer exists";

                // old orders have no saved price, then the current price is used
                double price = orders[i].Price ?? dish?.Price ?? 0;
                string orderedAt = orders[i].OrderedAt.HasValue
                    ? orders[i].OrderedAt.Value.ToString("dd-MM-yyyy HH:mm")
                    : "unknown";

                Console.WriteLine("-------------------------");
                Console.WriteLine($"[{i + 1}] {dishName}");
                Console.WriteLine($"    Ordered on: {orderedAt}");
                Console.WriteLine($"    Count: {orders[i].Count} x € {price:0.00}");
                Console.WriteLine($"    Total: € {orders[i].Count * price:0.00}");
                Console.WriteLine($"    Status: {orders[i].Status}");
            }

            Console.WriteLine("-------------------------");
            Console.Write("\nEnter a number to cancel that order (or press Enter to go back): ");
            string input = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > orders.Count)
            {
                Console.WriteLine("Invalid choice.");
                Pause();
                continue;
            }

            UserDishModel selected = orders[choice - 1];

            if (!string.IsNullOrEmpty(selected.Status) && selected.Status != UserDishLogic.StatusReceived)
            {
                Console.WriteLine($"This order can't be cancelled anymore (status: {selected.Status}).");
                Pause();
                continue;
            }

            Console.Write("Are you sure you want to cancel this order? (y/n): ");

            if (Console.ReadLine()?.ToLower() != "y")
                continue;

            bool cancelled = _userDishLogic.CancelOrder(selected.Id, guest.Id);

            Console.WriteLine(cancelled
                ? "\nOrder cancelled."
                : "\nCould not cancel the order.");
            Pause();
        }
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
