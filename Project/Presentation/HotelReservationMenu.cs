using System.Globalization;

public class HotelReservationMenu
{
    private static HotelReservationLogic _reservationLogic = new();
    private static RoomLogic _roomLogic = new();

    public static void Start(UserModel guest)
    {
        List<string> options = new List<string>
        {
            "Create reservation",
            "My reservations",
            "Back"
        };

        while (true)
        {
            int choice = ArrowMenu.Show("RESERVATIONS", options);

            switch (choice)
            {
                case 0:
                    CreateReservation(guest);
                    break;
                case 1:
                    MyReservations(guest);
                    break;
                default:
                    // Back or Esc
                    return;
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

        Console.Write("\nChoose a room number, c for cheapest, or e for expensive: ");

        string input = Console.ReadLine()?.ToLower();

        if (input == "c")
        {
            availableRooms = availableRooms
                .OrderBy(room => room.Price)
                .ToList();

            Console.WriteLine("\n=== Rooms: Cheapest first ===");
            DisplayRooms(availableRooms);

            Console.Write("\nChoose a room number: ");
            input = Console.ReadLine();
        }
        else if (input == "e")
        {
            availableRooms = availableRooms
                .OrderByDescending(room => room.Price)
                .ToList();

            Console.WriteLine("\n=== Rooms: Most expensive first ===");
            DisplayRooms(availableRooms);

            Console.Write("\nChoose a room number: ");
            input = Console.ReadLine();
        }

        if (!int.TryParse(input, out int choice) ||
            choice < 1 || choice > availableRooms.Count)
        {
            Console.WriteLine("Invalid room selection.");
            Console.ReadKey();
            return;
        }

        RoomModel selectedRoom = availableRooms[choice - 1];

        int nights = (endDate.Date - startDate.Date).Days;

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

        if (success)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Booking Confirmation ===");
            Console.ResetColor();
            Console.WriteLine("Your room is booked!\n");
            Console.WriteLine($"Room: {selectedRoom.Name} ({selectedRoom.Type})");
            Console.WriteLine($"Check-in: {startDate:dd-MM-yyyy}");
            Console.WriteLine($"Check-out: {endDate:dd-MM-yyyy}");
            Console.WriteLine($"Nights: {nights}");
            Console.WriteLine($"Total price: {price:C}");
            Console.WriteLine("\nYou can always find this booking under 'My reservations'.");
        }
        else
        {
            Console.WriteLine("\nCould not create reservation.");
        }

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

        Console.Write("Enter a number to manage that reservation (or press Enter to go back): ");
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
            Console.WriteLine("You can only change or cancel reservations that have not started yet.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n1. Change reservation");
        Console.WriteLine("2. Cancel reservation");
        Console.WriteLine("0. Go back");
        Console.Write("Choose an option: ");

        switch (Console.ReadLine())
        {
            case "1":
                EditReservation(selected);
                break;
            case "2":
                CancelReservation(selected);
                break;
        }
    }

    private static void EditReservation(HotelReservationModel reservation)
    {
        Console.Clear();

        RoomModel currentRoom = _roomLogic.SearchRoomByID((int)reservation.Room_Id);

        Console.WriteLine("=== Change Reservation ===\n");
        Console.WriteLine($"Room: {(currentRoom != null ? currentRoom.Name : "Room no longer exists")}");
        Console.WriteLine($"Check-in: {reservation.StartDate:dd-MM-yyyy}");
        Console.WriteLine($"Check-out: {reservation.EndDate:dd-MM-yyyy}");
        Console.WriteLine($"Total price: {reservation.Price:C}");
        Console.WriteLine("\nPress Enter to keep the current value.\n");

        if (!ReadDate("Check-in date", reservation.StartDate, out DateTime startDate) ||
            !ReadDate("Check-out date", reservation.EndDate, out DateTime endDate))
            return;

        if (endDate.Date <= startDate.Date)
        {
            Console.WriteLine("Check-out has to be after check-in.");
            Console.ReadKey();
            return;
        }

        if (startDate.Date <= DateTime.Today)
        {
            Console.WriteLine("Check-in has to be after today.");
            Console.ReadKey();
            return;
        }

        // rooms that are free on the new dates, this reservation itself does not count
        List<RoomModel> availableRooms = _roomLogic.GetAllRooms()
            .Where(room => _reservationLogic.IsRoomAvailableForUpdate(
                reservation.Id, room.ID, startDate, endDate))
            .ToList();

        if (availableRooms.Count == 0)
        {
            Console.WriteLine("No rooms available for these dates.");
            Console.ReadKey();
            return;
        }

        bool currentRoomFree = availableRooms.Any(room => room.ID == reservation.Room_Id);

        DisplayRooms(availableRooms);

        Console.Write(currentRoomFree
            ? "Choose a room number (or press Enter to keep your current room): "
            : "Your current room is not free on these dates. Choose a room number: ");

        string input = Console.ReadLine() ?? "";
        RoomModel newRoom;

        if (string.IsNullOrWhiteSpace(input) && currentRoomFree)
        {
            newRoom = availableRooms.First(room => room.ID == reservation.Room_Id);
        }
        else if (int.TryParse(input, out int choice) &&
                 choice >= 1 && choice <= availableRooms.Count)
        {
            newRoom = availableRooms[choice - 1];
        }
        else
        {
            Console.WriteLine("Invalid room selection.");
            Console.ReadKey();
            return;
        }

        int nights = (endDate.Date - startDate.Date).Days;
        double price = Convert.ToDouble(newRoom.Price * nights);

        Console.WriteLine("\n=== New Reservation Details ===");
        Console.WriteLine($"Room: {newRoom.Name}");
        Console.WriteLine($"Type: {newRoom.Type}");
        Console.WriteLine($"Check-in: {startDate:dd-MM-yyyy}");
        Console.WriteLine($"Check-out: {endDate:dd-MM-yyyy}");
        Console.WriteLine($"Nights: {nights}");
        Console.WriteLine($"Price per night: {newRoom.Price:C}");
        Console.WriteLine($"Total price: {price:C} (was {reservation.Price:C})");

        Console.Write("\nSave these changes? (y/n): ");

        if (Console.ReadLine()?.ToLower() != "y")
            return;

        bool success = _reservationLogic.UpdateHotelReservation(
            reservation.Id,
            reservation.User_Id,
            newRoom.ID,
            startDate,
            endDate,
            price);

        Console.WriteLine(success
            ? "\nReservation successfully changed!"
            : "\nCould not change the reservation.");

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    // reads a date, an empty input keeps the current date
    private static bool ReadDate(string label, DateTime current, out DateTime date)
    {
        Console.Write($"{label} (dd-mm-yyyy) [{current:dd-MM-yyyy}]: ");
        string input = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(input))
        {
            date = current;
            return true;
        }

        if (DateTime.TryParseExact(input.Trim(), "dd-MM-yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            return true;

        Console.WriteLine($"Invalid {label.ToLower()}.");
        Console.ReadKey();
        return false;
    }

    private static void CancelReservation(HotelReservationModel selected)
    {
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
            Console.WriteLine($"    Description:");
            Console.WriteLine($"    {rooms[i].Description}");
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
