public static class AdminMenu
{
    public static void Start(UserModel employee)
    {
        List<string> options = new List<string>
        {
            "Manage rooms",
            "Manage dishes",
            "Manage activities",
            "Check-in / check-out guests",
            "Log out"
        };

        while (true)
        {
            int choice = ArrowMenu.Show($"WELCOME {employee.FullName} (ADMIN)".ToUpper(), options);

            switch (choice)
            {
                case 0:
                    Room.Start();
                    break;
                case 1:
                    DishManagement.Start();
                    break;
                case 2:
                    ActivityManagement.Start();
                    break;
                case 3:
                    CheckInMenu.Start();
                    break;
                default:
                    // Log out or Esc
                    return;
            }
        }
    }
}
