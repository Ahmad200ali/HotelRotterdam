static class UserLogin
{
    static private UserLogic accountsLogic = new UserLogic();
    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = ReadPassword();
        UserModel acc = accountsLogic.CheckLogin(email, password);
        if (acc != null)
        {
            if (acc.Role == "ADMIN")
            {
                AdminMenu.Start(acc);
                return;
            }
            GuestMenu.Start(acc);

        }
        else
        {
            Console.WriteLine("No account found with that email and password");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }
    }

    // reads the password but shows * instead of the letters
    public static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key = Console.ReadKey(true);

        while (key.Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            key = Console.ReadKey(true);
        }

        Console.WriteLine();
        return password;
    }
}
