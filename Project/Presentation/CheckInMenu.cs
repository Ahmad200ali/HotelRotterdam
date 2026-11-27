static class CheckInMenu
{
    private static readonly HotelReservationLogic reservationLogic = new HotelReservationLogic();
    private static readonly RoomLogic roomLogic = new RoomLogic();
    private static readonly UserLogic userLogic = new UserLogic();

    public static void Start()
    {
        // after a check-in or check-out the list is shown again
        while (true)
        {
            List<HotelReservationModel> reservations = reservationLogic.GetAllReservations();

            if (reservations.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("There are no reservations yet.");
                WaitForKey();
                return;
            }

            List<string> options = new List<string>();
            foreach (HotelReservationModel reservation in reservations)
            {
                options.Add($"{reservation.Id}. {GetGuestName(reservation)} | {GetRoomName(reservation)} | " +
                            $"{reservation.StartDate:dd-MM-yyyy} until {reservation.EndDate:dd-MM-yyyy} | {reservation.Status}");
            }
            options.Add("Back");

            int choice = ArrowMenu.Show("CHECK-IN / CHECK-OUT (choose a reservation)", options);

            if (choice == -1 || choice == reservations.Count)
                return;

            ReservationOptions(reservations[choice]);
        }
    }

    private static void ReservationOptions(HotelReservationModel reservation)
    {
        string title = $"Reservation: {reservation.Id}\n" +
                       $"Guest: {GetGuestName(reservation)}\n" +
                       $"Room: {GetRoomName(reservation)} ({(reservationLogic.RoomIsOccupied(reservation.Room_Id) ? "occupied" : "free")})\n" +
                       $"Check-in date: {reservation.StartDate:dd-MM-yyyy}\n" +
                       $"Check-out date: {reservation.EndDate:dd-MM-yyyy}\n" +
                       $"Status: {reservation.Status}\n" +
                       $"Checked in at: {FormatTime(reservation.CheckInTime)}\n" +
                       $"Checked out at: {FormatTime(reservation.CheckOutTime)}";

        int choice = ArrowMenu.Show(title, new List<string> { "Check in guest", "Check out guest", "Back" });

        if (choice == 0)
        {
            bool result = reservationLogic.CheckIn(reservation.Id);

            ShowMessage(
                result ? "Guest checked in!" : "Check-in failed. The reservation must be 'Reserved', today must be during the stay and the room must be free.",
                result);
            WaitForKey();
        }
        else if (choice == 1)
        {
            bool result = reservationLogic.CheckOut(reservation.Id);

            ShowMessage(
                result ? "Guest checked out!" : "Check-out failed. The guest has to be checked in first.",
                result);
            WaitForKey();
        }
    }

    private static string FormatTime(DateTime? time)
    {
        return time.HasValue ? time.Value.ToString("dd-MM-yyyy HH:mm") : "-";
    }

    private static string GetGuestName(HotelReservationModel reservation)
    {
        UserModel guest = userLogic.GetUserByID(reservation.User_Id);
        return guest != null ? guest.FullName : "Unknown guest";
    }

    private static string GetRoomName(HotelReservationModel reservation)
    {
        RoomModel room = roomLogic.SearchRoomByID((int)reservation.Room_Id);
        return room != null ? room.Name : "Room no longer exists";
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
