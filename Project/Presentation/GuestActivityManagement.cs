using System;

public class GuestActivityManagement
{
    private static ActivityLogic _activityLogic = new ActivityLogic();
    private static UserActivityLogic _userActivityLogic = new UserActivityLogic();

    public static void Start(UserModel guest)
    {
        string choice;

        do
        {
            Console.Clear();
            Console.WriteLine($"===== Activities - Welcome {guest.FullName} =====");
            Console.WriteLine("1. View activities");
            Console.WriteLine("2. View my booked activities");
            Console.WriteLine("0. Back to guest menu");
            Console.Write("\nChoose an option: ");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SelectActivity(guest);
                    break;

                case "2":
                    ViewMyActivities(guest);
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

    private static void SelectActivity(UserModel guest)
    {
        Console.Clear();
        Console.WriteLine("===== Select an Activity =====");

        var activities = _activityLogic.GetAll();

        if (activities.Count == 0)
        {
            Console.WriteLine("No activities available.");
            Pause();
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
            Pause();
            return;
        }

        if (id == 0)
            return;

        ActivityModel selectedActivity = _activityLogic.GetByID(id);

        if (selectedActivity == null)
        {
            Console.WriteLine("Activity not found!");
            Pause();
            return;
        }

        ActivityOptions(selectedActivity, guest);
    }

    private static void ActivityOptions(ActivityModel activity, UserModel guest)
    {
        string choice;

        do
        {
            Console.Clear();
            Console.WriteLine($"===== {activity.Name} =====");
            Console.WriteLine("1. View activity");
            Console.WriteLine("2. Book activity");
            Console.WriteLine("0. Back");
            Console.Write("\nChoose an option: ");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewActivity(activity);
                    break;

                case "2":
                    BookActivity(activity, guest);
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

    private static void ViewActivity(ActivityModel activity)
    {
        Console.Clear();
        Console.WriteLine("===== Activity Details =====");
        Console.WriteLine($"ID: {activity.ID}");
        Console.WriteLine($"Name: {activity.Name}");
        Console.WriteLine($"Description: {activity.Description}");
        Console.WriteLine($"Price: {activity.Price:C}");

        Pause();
    }

    private static void BookActivity(ActivityModel activity, UserModel guest)
    {
        var bookings = _userActivityLogic.GetByUserID(guest.Id);

        if (bookings.Exists(a => a.Activity_Id == activity.ID))
        {
            Console.WriteLine("\nYou have already booked this activity!");
            Pause();
            return;
        }

        UserActivityModel userActivity = new UserActivityModel(
            0,
            guest.Id,
            activity.ID
        );

        _userActivityLogic.Add(userActivity);

        Console.WriteLine("\nActivity booked successfully!");
        Pause();
    }

    private static void ViewMyActivities(UserModel guest)
    {
        Console.Clear();
        Console.WriteLine("===== My Booked Activities =====");

        var bookings = _userActivityLogic.GetByUserID(guest.Id);

        if (bookings.Count == 0)
        {
            Console.WriteLine("You haven't booked any activities yet.");
            Pause();
            return;
        }

        foreach (var booking in bookings)
        {
            ActivityModel activity =
                _activityLogic.GetByID(booking.Activity_Id);

            if (activity != null)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine($"ID: {activity.ID}");
                Console.WriteLine($"Name: {activity.Name}");
                Console.WriteLine($"Description: {activity.Description}");
                Console.WriteLine($"Price: {activity.Price:C}");
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