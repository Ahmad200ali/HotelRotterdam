
using System;

public class ActivityManagement
{
    private static ActivityLogic _activityLogic = new ActivityLogic();

    public static void Start()
    {
        string choice;

        do
        {
            Console.Clear();
            Console.WriteLine("===== Activity Management =====");
            Console.WriteLine("1. Add activity");
            Console.WriteLine("2. Manage activities");
            Console.WriteLine("0. Back to admin menu");
            Console.Write("Choose an option: ");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddActivity();
                    break;

                case "2":
                    ManageActivities();
                    break;

                case "0":
                    break;

                default:
                    Console.WriteLine("Invalid option!");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
            }

        } while (choice != "0");
    }

    private static void AddActivity()
    {
        Console.Clear();
        Console.WriteLine("===== Add Activity =====");

        Console.Write("Activity name: ");
        string name = Console.ReadLine()?.Trim();

        Console.Write("Description: ");
        string description = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Name and description are required!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Price: ");

        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
        {
            Console.WriteLine("Invalid price!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        ActivityModel activity = new ActivityModel(
            0, name, description, price
        );

        _activityLogic.Add(activity);

        Console.WriteLine("\nActivity added successfully!");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void ManageActivities()
    {
        Console.Clear();
        Console.WriteLine("===== Select an Activity =====");

        var activities = _activityLogic.GetAll();

        if (activities.Count == 0)
        {
            Console.WriteLine("No activities available.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        foreach (var activity in activities)
        {
            Console.WriteLine($"{activity.ID}. {activity.Name}");
        }

        Console.WriteLine("0. Back");
        Console.Write("\nSelect activity ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        if (id == 0)
            return;

        ActivityModel selectedActivity = _activityLogic.GetByID(id);

        if (selectedActivity == null)
        {
            Console.WriteLine("Activity not found!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        ActivityOptions(selectedActivity);
    }

    private static void ActivityOptions(ActivityModel activity)
    {
        string choice;

        do
        {
            Console.Clear();
            Console.WriteLine($"===== {activity.Name} =====");
            Console.WriteLine("1. View activity");
            Console.WriteLine("2. Update activity");
            Console.WriteLine("3. Delete activity");
            Console.WriteLine("0. Back");
            Console.Write("\nChoose an option: ");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewActivity(activity);
                    break;

                case "2":
                    UpdateActivity(activity);
                    break;

                case "3":
                    DeleteActivity(activity);
                    return;

                case "0":
                    break;

                default:
                    Console.WriteLine("Invalid option!");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
            }

        } while (choice != "0");
    }

    private static void ViewActivity(ActivityModel activity)
    {
        Console.Clear();
        Console.WriteLine("===== Activity Details =====");
        Console.WriteLine($"ID: {activity.ID}");
        Console.WriteLine($"Name: {activity.Name}");
        Console.WriteLine($"Description: {activity.Description}");
        Console.WriteLine($"Price: {activity.Price:C}");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void UpdateActivity(ActivityModel activity)
    {
        Console.Clear();
        Console.WriteLine("===== Update Activity =====");

        Console.Write($"New name ({activity.Name}): ");
        string name = Console.ReadLine()?.Trim();

        Console.Write($"New description ({activity.Description}): ");
        string description = Console.ReadLine()?.Trim();

        Console.Write($"New price ({activity.Price}): ");
        string priceInput = Console.ReadLine();

        decimal updatedPrice = activity.Price;
        if (!string.IsNullOrWhiteSpace(priceInput))
        {
            if (decimal.TryParse(priceInput, out decimal price) && price >= 0)
                updatedPrice = price;
            else
            {
                Console.WriteLine("Invalid price!");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }
        }

        activity.Name = string.IsNullOrWhiteSpace(name) ? activity.Name : name;
        activity.Description = string.IsNullOrWhiteSpace(description) ? activity.Description : description;
        activity.Price = updatedPrice;

        _activityLogic.Update(activity);

        Console.WriteLine("\nActivity updated successfully!");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void DeleteActivity(ActivityModel activity)
    {
        Console.Write($"Are you sure you want to delete {activity.Name}? (y/n): ");

        string confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "y")
        {
            bool deleted = _activityLogic.Delete(activity.ID);

            Console.WriteLine(deleted
                ? "Activity deleted successfully!"
                : "Activity could not be deleted.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}

