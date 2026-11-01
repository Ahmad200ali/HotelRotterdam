
static class Room
{
    private static readonly RoomLogic roomLogic = new RoomLogic();
    private static readonly RoomTypeLogic roomTypeLogic = new RoomTypeLogic();

    public static void Start()
    {
        bool running = true;

        List<string> options = new List<string>
        {
            "1. View all rooms",
            "2. Add a room",
            "3. Manage room types",
            "4. Exit"
        };

        while (running)
        {
            int choice = ArrowMenu.Show("     ROOM MANAGEMENT DASHBOARD", options);

            switch (choice)
            {
                case 0:
                    ViewAllRooms();
                    break;

                case 1:
                    AddRoom();
                    break;

                case 2:
                    ManageRoomTypes();
                    break;

                case 3:
                case -1:
                    running = false;
                    break;
            }
        }
    }

    private static void ViewAllRooms()
    {
        List<RoomModel> rooms = roomLogic.GetAllRooms();

        if (rooms.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No rooms found.");
            WaitForKey();
            return;
        }

        List<string> options = new List<string>();
        foreach (RoomModel room in rooms)
        {
            options.Add($"{room.ID}. {room.Name} | {room.Type} | {room.Price}");
        }
        options.Add("Back");

        int choice = ArrowMenu.Show("All rooms (choose a room to update or delete it)", options);

        if (choice == -1 || choice == rooms.Count)
            return;

        RoomOptions(rooms[choice]);
    }

    private static void RoomOptions(RoomModel room)
    {
        string title = $"ID: {room.ID}\n" +
                       $"Name: {room.Name}\n" +
                       $"Type: {room.Type}\n" +
                       $"Description: {room.Description}\n" +
                       $"Price: {room.Price}";

        int choice = ArrowMenu.Show(title, new List<string> { "Update room", "Delete room", "Back" });

        if (choice == 0)
            UpdateRoom(room);
        else if (choice == 1)
            DeleteRoom(room);
    }

    private static void AddRoom()
    {
        string type = ChooseRoomType(null);
        if (type == null)
            return;

        Console.Clear();
        Console.WriteLine($"Room type: {type}\n");

        Console.WriteLine("Enter room name:");
        string name = Console.ReadLine() ?? "";

        Console.WriteLine("Enter room description:");
        string description = Console.ReadLine() ?? "";

        Console.WriteLine("Enter room price:");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price)
            || price < 0)
        {
            ShowMessage("Invalid price!", false);
            WaitForKey();
            return;
        }

        bool result = roomLogic.WriteRoom(
            name, type, description, price);

        ShowMessage(
            result ? "Room added successfully!" : "Failed to add room!",
            result);
        WaitForKey();
    }

    private static void UpdateRoom(RoomModel room)
    {
        string type = ChooseRoomType(room.Type);
        if (type == null)
            return;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press Enter without typing anything to keep the current value.\n");
        Console.ResetColor();

        Console.WriteLine($"Room type: {type}\n");

        Console.WriteLine($"Name [{room.Name}]:");
        string name = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(name))
            room.Name = name;

        Console.WriteLine($"Description [{room.Description}]:");
        string description = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(description))
            room.Description = description;

        room.Type = type;

        // price is not kept with Enter, the admin has to type it again
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nThe price has to be entered again! (current price: {room.Price})");
        Console.ResetColor();

        decimal price;
        while (true)
        {
            Console.WriteLine("Enter room price:");
            if (decimal.TryParse(Console.ReadLine(), out price) && price >= 0)
                break;

            ShowMessage("Invalid price, try again.", false);
        }

        room.Price = price;

        bool result = roomLogic.UpdateRoom(room);

        ShowMessage(
            result ? "Room updated successfully!" : "Failed to update room!",
            result);
        WaitForKey();
    }

    private static void DeleteRoom(RoomModel room)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete '{room.Name}'? (y/n)");
        string answer = Console.ReadLine() ?? "";

        if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            bool result = roomLogic.DeleteRoomByID(room.ID);

            ShowMessage(
                result ? "Room deleted successfully!" : "Failed to delete room!",
                result);
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
        WaitForKey();
    }

    // currentType is null when adding a new room
    private static string ChooseRoomType(string currentType)
    {
        List<RoomTypeModel> types = roomTypeLogic.GetAllRoomTypes();

        if (types.Count == 0)
        {
            Console.Clear();
            ShowMessage("There are no room types yet, add one first under 'Manage room types'.", false);
            WaitForKey();
            return null;
        }

        List<string> options = new List<string>();
        if (currentType != null)
            options.Add($"Keep current type ({currentType})");

        foreach (RoomTypeModel type in types)
            options.Add(type.Name);

        int choice = ArrowMenu.Show("Choose a room type:", options);

        if (choice == -1)
            return null;

        if (currentType != null)
        {
            if (choice == 0)
                return currentType;
            return types[choice - 1].Name;
        }

        return types[choice].Name;
    }

    private static void ManageRoomTypes()
    {
        while (true)
        {
            List<RoomTypeModel> types = roomTypeLogic.GetAllRoomTypes();

            List<string> options = new List<string>();
            foreach (RoomTypeModel type in types)
                options.Add(type.Name);
            options.Add("+ Add a room type");
            options.Add("Back");

            int choice = ArrowMenu.Show("ROOM TYPES (choose a type to delete it)", options);

            if (choice == -1 || choice == types.Count + 1)
                return;

            Console.Clear();

            if (choice == types.Count)
            {
                Console.WriteLine("Name of the new room type:");
                string name = Console.ReadLine() ?? "";

                bool added = roomTypeLogic.AddRoomType(name);
                ShowMessage(
                    added ? "Room type succesfully added!" : "Room type is empty or already exists.",
                    added);
            }
            else
            {
                RoomTypeModel type = types[choice];
                Console.WriteLine($"Delete room type '{type.Name}'? (y/n)");

                if ((Console.ReadLine() ?? "").Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    bool deleted = roomTypeLogic.DeleteRoomType(type.ID);
                    ShowMessage(
                        deleted ? "Room type deleted!" : "Failed to delete room type!",
                        deleted);
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }
            WaitForKey();
        }
    }

    private static void WaitForKey()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
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
