using System.Globalization;

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
            Console.WriteLine("1. Create reservation");
            Console.WriteLine("2. My reservations");
            Console.WriteLine("0. Return to guest menu");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    CreateReservation(guest);
                    break;

                case "2":
                    MyReservations(guest);
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

        // multiply as decimal first, otherwise the total can become something like 2760.8900000000003
        double price = Convert.ToDouble(selectedRoom.Price * nights);

        Console.WriteLine("\n=== Reservation Details ===");
        Console.WriteLine($"Room: {selectedRoom.Name}");
        Console.WriteLine($"Type: {selectedRoom.Type}");
        Console.WriteLine($"Check-in: {startDate:dd-MM-yyyy}");
        Console.WriteLine($"Check-out: {endDate:dd-MM-yyyy}");
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

    private static void MyReservations(UserModel guest)
    {
        Console.Clear();

        Console.WriteLine("=== My Reservations ===\n");

        List<HotelReservationModel> reservations =
            _reservationLogic.GetReservationsByUserID(guest.Id);

        if (reservations.Count == 0)
        {
            Console.WriteLine("You don't have any reservations yet.");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < reservations.Count; i++)
        {
            HotelReservationModel reservation = reservations[i];
            RoomModel room = _roomLogic.SearchRoomByID((int)reservation.Room_Id);
            string roomName = room != null ? room.Name : "Room no longer exists";
            string status = reservation.EndDate.Date < DateTime.Today ? "(past)" : "";

            Console.WriteLine($"[{i + 1}] {roomName} {status}");
            Console.WriteLine($"    {reservation.StartDate:dd-MM-yyyy} until {reservation.EndDate:dd-MM-yyyy}");
            Console.WriteLine($"    Total price: {reservation.Price:C}");
            Console.WriteLine();
        }

        Console.Write("Enter a number to cancel that reservation (or press Enter to go back): ");
        string input = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(input))
            return;

        if (!int.TryParse(input, out int choice) ||
            choice < 1 || choice > reservations.Count)
        {
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            return;
        }

        HotelReservationModel selected = reservations[choice - 1];

        if (selected.StartDate.Date <= DateTime.Today)
        {
            Console.WriteLine("You can only cancel reservations that have not started yet.");
            Console.ReadKey();
            return;
        }

        Console.Write("Are you sure you want to cancel this reservation? (y/n): ");

        if (Console.ReadLine()?.ToLower() != "y")
            return;

        bool success = _reservationLogic.DeleteHotelReservationByID(selected.Id);

        Console.WriteLine(success
            ? "\nReservation cancelled."
            : "\nCould not cancel the reservation.");

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

        Console.Write("Check-in date (dd-mm-yyyy): ");

        if (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            Console.WriteLine("Invalid check-in date.");
            Console.ReadKey();
            return false;
        }

        Console.Write("Check-out date (dd-mm-yyyy): ");

        if (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate) ||
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
