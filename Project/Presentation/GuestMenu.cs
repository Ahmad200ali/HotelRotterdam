public static class GuestMenu
{
    private static readonly ActivityLogic activityLogic = new ActivityLogic();
    private static readonly RoomLogic roomLogic = new RoomLogic();

    public static void Start(UserModel guest)
    {
        List<string> options = new List<string>
        {
            "Reservations",
            "Room service",
            "Activities",
            "Hotel floor plan",
            "Log out"
        };

        while (true)
        {
            int choice = ArrowMenu.Show($"WELCOME {guest.FullName}".ToUpper().Trim(), options);

            switch (choice)
            {
                case 0:
                    HotelReservationMenu.Start(guest);
                    break;
                case 1:
                    GuestDishManagement.Start(guest);
                    break;
                case 2:
                    GuestActivityManagement.Start(guest);
                    break;
                case 3:
                    ShowFloorPlan();
                    Console.WriteLine("\nPress any key to return to the guest menu.");
                    Console.ReadKey();
                    break;
                default:
                    // Log out or Esc
                    return;
            }
        }
    }

   private static void ShowFloorPlan()
    {
        Console.Clear();
        List<RoomModel> rooms = roomLogic.GetAllRooms();
        List<ActivityModel> activities = activityLogic.GetAll();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("HOTEL FLOOR PLAN");
        Console.WriteLine("+--------------------+--------------------+--------------------+");
        Console.WriteLine($"|{Center("RESTAURANT", 20)}|{Center("MAIN LOBBY", 20)}|{Center("CONFERENCE ROOM", 20)}|");
        Console.WriteLine("+--------------------+----------+----------+--------------------+");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < rooms.Count; i += 2)
        {
            string leftRoom = $"GUEST ROOM {rooms[i].Name}";
            string rightRoom = i + 1 < rooms.Count ? $"GUEST ROOM {rooms[i + 1].Name}" : "";

            if (i == 0)
            {
                Console.WriteLine($"|{Center(leftRoom, 20)}|{Center("LIFT", 10)}|{Center("RECEPTION", 10)}|{Center(rightRoom, 20)}|");
                Console.WriteLine("+--------------------+--------------------+--------------------+");
            }
            else
            {
                Console.WriteLine($"|{Center(leftRoom, 20)}|{Center("MAIN CORRIDOR", 20)}|{Center(rightRoom, 20)}|");
                Console.WriteLine("+--------------------+--------------------+--------------------+");
            }
        }

        if (rooms.Count == 0)
        {
            Console.WriteLine("|                    |          |          |                    |");
            Console.WriteLine("+--------------------+--------------------+--------------------+");
        }
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("+--------------------+--------------------+--------------------+");
        Console.WriteLine($"|{Center("HOTEL GARDEN", 20)}|{Center("MAIN ENTRANCE", 20)}|{Center("PARKING", 20)}|");
        Console.WriteLine("+--------------------+--------------------+--------------------+");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("+--------------------------------------------------------------+");
        Console.WriteLine($"|{Center("SWIMMING POOL", 62)}|");
        Console.WriteLine("+--------------------------------------------------------------+");
        Console.ResetColor();

        if (activities.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("+--------------------------------------------------------------+");
            Console.WriteLine($"|{Center("ACTIVITIES", 62)}|");
            Console.WriteLine("+--------------------+--------------------+--------------------+");

            for (int i = 0; i < activities.Count; i += 3)
            {
                string first = activities[i].Name;
                string second = i + 1 < activities.Count ? activities[i + 1].Name : "";
                string third = i + 2 < activities.Count ? activities[i + 2].Name : "";
                Console.WriteLine($"|{Center(first, 20)}|{Center(second, 20)}|{Center(third, 20)}|");
                Console.WriteLine("+--------------------+--------------------+--------------------+");
            }
            Console.ResetColor();
        }
    }

    private static string Center(string text, int width)
    {
        if (text.Length > width - 2)
            text = text.Substring(0, width - 2);

        int leftPadding = (width - text.Length) / 2;
        return new string(' ', leftPadding) + text + new string(' ', width - text.Length - leftPadding);
    }

}
