public static class AdminMenu
{
    public static void Start(UserModel employee)
    {
        bool loggedIn = true;

        while (loggedIn)
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {employee.FullName} (Admin)");
            Console.WriteLine("1. Manage rooms");
            Console.WriteLine("2. Manage menu dishes");
            Console.WriteLine("0. Log out");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Room.Start();
                    break;
                case "2":
                    MenuManagement.Start();
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
}
