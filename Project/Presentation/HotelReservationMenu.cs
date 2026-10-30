
public class HotelReservationMenu
{
    private static HotelReservationLogic _reservationLogic = new();
    private static RoomLogic _roomLogic = new();

    public static void Start(UserModel guest)
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("=== Hotel Reservations ===");
            Console.WriteLine("1. View available rooms");
            Console.WriteLine("2. Create reservation");
            Console.WriteLine("0. Return to guest menu");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ViewAvailableRooms();
                    break;

                case "2":
                    CreateReservation(guest);
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void ViewAvailableRooms()
    {
        Console.Clear();

        Console.WriteLine("=== Available Rooms ===");

        if (!GetDates(out DateTime startDate, out DateTime endDate))
            return;

        List<RoomModel> availableRooms = GetAvailableRooms(startDate, endDate);

        if (availableRooms.Count == 0)
        {
            Console.WriteLine("No rooms available for these dates.");
        }
        else
        {
            DisplayRooms(availableRooms);
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    private static void CreateReservation(UserModel guest)
    {
        Console.Clear();

        Console.WriteLine("=== Create Reservation ===");

        if (!GetDates(out DateTime startDate, out DateTime endDate))
            return;

        List<RoomModel> availableRooms = GetAvailableRooms(startDate, endDate);

        if (availableRooms.Count == 0)
        {
            Console.WriteLine("No rooms available for these dates.");
            Console.ReadKey();
            return;
        }

        DisplayRooms(availableRooms);

        Console.Write("\nChoose a room number: ");

        if (!int.TryParse(Console.ReadLine(), out int choice) ||
            choice < 1 || choice > availableRooms.Count)
        {
            Console.WriteLine("Invalid room selection.");
            Console.ReadKey();
            return;
        }

        RoomModel selectedRoom = availableRooms[choice - 1];

        int nights = (endDate.Date - startDate.Date).Days;

        double price = Convert.ToDouble(selectedRoom.Price) * nights;

        Console.WriteLine("\n=== Reservation Details ===");
        Console.WriteLine($"Room: {selectedRoom.Name}");
        Console.WriteLine($"Type: {selectedRoom.Type}");
        Console.WriteLine($"Check-in: {startDate:yyyy-MM-dd}");
        Console.WriteLine($"Check-out: {endDate:yyyy-MM-dd}");
        Console.WriteLine($"Nights: {nights}");
        Console.WriteLine($"Price per night: {selectedRoom.Price:C}");
        Console.WriteLine($"Total price: {price:C}");

        Console.Write("\nConfirm reservation? (y/n): ");

        if (Console.ReadLine()?.ToLower() != "y")
            return;

        bool success = _reservationLogic.WriteHotelReservation(
            guest.Id,
            selectedRoom.ID,
            startDate,
            endDate,
            price);

        Console.WriteLine(success
            ? "\nReservation successfully created!"
            : "\nCould not create reservation.");

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    private static List<RoomModel> GetAvailableRooms(
        DateTime startDate, DateTime endDate)
    {
        List<RoomModel> allRooms = _roomLogic.GetAllRooms();

        return allRooms.Where(room =>
            _reservationLogic.IsRoomAvailable(
                room.ID, startDate, endDate)).ToList();
    }

    private static void DisplayRooms(List<RoomModel> rooms)
    {
        Console.WriteLine("\nAvailable rooms:\n");

        for (int i = 0; i < rooms.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {rooms[i].Name}");
            Console.WriteLine($"    Type: {rooms[i].Type}");
            Console.WriteLine($"    Description: {rooms[i].Description}");
            Console.WriteLine($"    Price per night: {rooms[i].Price:C}");
            Console.WriteLine();
        }
    }

    private static bool GetDates(
        out DateTime startDate, out DateTime endDate)
    {
        startDate = default;
        endDate = default;

        Console.Write("Check-in date (yyyy-MM-dd): ");

        if (!DateTime.TryParse(Console.ReadLine(), out startDate))
        {
            Console.WriteLine("Invalid check-in date.");
            Console.ReadKey();
            return false;
        }

        Console.Write("Check-out date (yyyy-MM-dd): ");

        if (!DateTime.TryParse(Console.ReadLine(), out endDate) ||
            endDate.Date <= startDate.Date)
        {
            Console.WriteLine("Invalid check-out date.");
            Console.ReadKey();
            return false;
        }

        if (startDate.Date < DateTime.Today)
        {
            Console.WriteLine("You cannot book a room in the past.");
            Console.ReadKey();
            return false;
        }

        return true;
    }
}
