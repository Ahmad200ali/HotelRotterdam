
static class Room
{
    private static readonly RoomLogic roomLogic = new RoomLogic();

    public static void Start()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================================");
            Console.WriteLine("     ROOM MANAGEMENT DASHBOARD");
            Console.WriteLine("================================");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("1. Add a room");
            Console.WriteLine("2. Search for a room");
            Console.WriteLine("3. Update a room");
            Console.WriteLine("4. Delete a room");
            Console.WriteLine("5. View all rooms");
            Console.WriteLine("0. Exit");
            Console.Write("\nChoose an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddRoom();
                    break;

                case "2":
                    SearchRoom();
                    break;

                case "3":
                    UpdateRoom();
                    break;

                case "4":
                    DeleteRoom();
                    break;

                case "5":
                    GetAllroom();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please choose 0-5.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }
    }

    private static void AddRoom()
    {
        Console.Clear();

        Console.WriteLine("Enter room name:");
        string name = Console.ReadLine() ?? "";

        Console.WriteLine("Enter room type:");
        string type = Console.ReadLine() ?? "";

        Console.WriteLine("Enter room description:");
        string description = Console.ReadLine() ?? "";

        Console.WriteLine("Enter room price:");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price)
            || price < 0)
        {
            ShowMessage("Invalid price!", false);
            return;
        }

        bool result = roomLogic.WriteRoom(
            name, type, description, price);

        ShowMessage(
            result ? "Room added successfully!" : "Failed to add room!",
            result);
    }

    private static void SearchRoom()
    {
        Console.Clear();

        Console.WriteLine("Enter room ID:");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            ShowMessage("Invalid room ID!", false);
            return;
        }

        RoomModel room = roomLogic.SearchRoomByID(id);

        if (room == null)
        {
            ShowMessage("Room not found!", false);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nRoom found successfully!");
        Console.ResetColor();

        Console.WriteLine($"ID: {room.ID}");
        Console.WriteLine($"Name: {room.Name}");
        Console.WriteLine($"Type: {room.Type}");
        Console.WriteLine($"Description: {room.Description}");
        Console.WriteLine($"Price: {room.Price}");
    }

    private static void UpdateRoom()
    {
        Console.Clear();

        Console.WriteLine("Enter room ID:");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            ShowMessage("Invalid room ID!", false);
            return;
        }

        RoomModel room = roomLogic.SearchRoomByID(id);

        if (room == null)
        {
            ShowMessage("Room not found!", false);
            return;
        }

        Console.WriteLine($"Current name: {room.Name}");
        Console.WriteLine("Enter new room name:");
        room.Name = Console.ReadLine() ?? "";

        Console.WriteLine($"Current type: {room.Type}");
        Console.WriteLine("Enter new room type:");
        room.Type = Console.ReadLine() ?? "";

        Console.WriteLine($"Current description: {room.Description}");
        Console.WriteLine("Enter new room description:");
        room.Description = Console.ReadLine() ?? "";

        Console.WriteLine($"Current price: {room.Price}");
        Console.WriteLine("Enter new room price:");

        if (!decimal.TryParse(Console.ReadLine(), out decimal price)
            || price < 0)
        {
            ShowMessage("Invalid price!", false);
            return;
        }

        room.Price = price;

        bool result = roomLogic.UpdateRoom(room);

        ShowMessage(
            result ? "Room updated successfully!" : "Failed to update room!",
            result);
    }

    private static void DeleteRoom()
    {
        Console.Clear();

        Console.WriteLine("Enter room ID:");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            ShowMessage("Invalid room ID!", false);
            return;
        }

        RoomModel room = roomLogic.SearchRoomByID(id);

        if (room == null)
        {
            ShowMessage("Room not found!", false);
            return;
        }

        Console.WriteLine($"Are you sure you want to delete '{room.Name}'? (y/n)");
        string answer = Console.ReadLine() ?? "";

        if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            bool result = roomLogic.DeleteRoomByID(id);

            ShowMessage(
                result ? "Room deleted successfully!" : "Failed to delete room!",
                result);
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    private static void GetAllroom()
    {
        Console.Clear();

        List<RoomModel> rooms = roomLogic.GetAllRooms();

        if (rooms.Count == 0)
        {
            Console.WriteLine("No rooms found.");
            return;
        }

        Console.WriteLine("All rooms:\n");

        foreach (RoomModel room in rooms)
        {
            Console.WriteLine($"ID: {room.ID}");
            Console.WriteLine($"Name: {room.Name}");
            Console.WriteLine($"Type: {room.Type}");
            Console.WriteLine($"Description: {room.Description}");
            Console.WriteLine($"Price: {room.Price}");
            Console.WriteLine(new string('-', 32));
        }
    }

    private static void ShowMessage(string message, bool success)
    {
        Console.ForegroundColor = success
            ? ConsoleColor.Green
            : ConsoleColor.Red;

        Console.WriteLine(message);
        Console.ResetColor();
    }
}
