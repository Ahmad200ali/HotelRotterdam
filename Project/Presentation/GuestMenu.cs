public static class GuestMenu
{
    public static void Start(UserModel guest)
    {
        bool loggedIn = true;

        while (loggedIn)
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {guest.FullName}");
            Console.WriteLine("1. View the hotel floor plan");
            Console.WriteLine("2. View the Menu Dishs");
            Console.WriteLine("0. Log out");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowFloorPlan();
                    Console.WriteLine("\nPress any key to return to the guest menu.");
                    Console.ReadKey();
                    break;
                case "2":
                    MenuManagement.ListDishes();
                    Console.WriteLine("\nPress any key to return to the guest menu.");
                    Console.ReadKey();
                    break;
                case "0":
                    loggedIn = false;
                    Menu.Start();
                    break;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void ShowFloorPlan()
    {
        Console.Clear();
        Console.WriteLine("                         HOTEL FLOOR PLAN - GROUND FLOOR");
        Console.WriteLine("+----------------------+----------------------+----------------------+");
        Console.WriteLine("|                      |                      |                      |");
        Console.WriteLine("|      RESTAURANT      |      MAIN LOBBY      |     CONFERENCE ROOM  |");
        Console.WriteLine("|                      |                      |                      |");
        Console.WriteLine("+----------------------+----------+-----------+----------------------+");
        Console.WriteLine("|                      |          |           |                      |");
        Console.WriteLine("|      GUEST ROOM      |   LIFT   | RECEPTION |      GUEST ROOM      |");
        Console.WriteLine("|         101          |          |           |         102          |");
        Console.WriteLine("+----------------------+----------+-----------+----------------------+");
        Console.WriteLine("|                      |                      |                      |");
        Console.WriteLine("|      GUEST ROOM      |     MAIN CORRIDOR    |      GUEST ROOM      |");
        Console.WriteLine("|         103          |                      |         104          |");
        Console.WriteLine("+----------------------+----------------------+----------------------+");
        Console.WriteLine("|                      |                      |                      |");
        Console.WriteLine("|     HOTEL GARDEN     |   MAIN ENTRANCE      |      PARKING         |");
        Console.WriteLine("|                      |                      |                      |");
        Console.WriteLine("+----------------------+----------------------+----------------------+");
    }
}
