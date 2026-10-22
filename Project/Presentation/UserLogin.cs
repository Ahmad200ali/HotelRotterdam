static class UserLogin
{
    static private UserLogic accountsLogic = new UserLogic();
    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();
        UserModel acc = accountsLogic.CheckLogin(email, password);
        if (acc != null)
        {
            if (acc.Role == "ADMIN")
            {
                Console.WriteLine($"Welcome {acc.FullName} you are Admin");
                Console.WriteLine("Please Enter A valid Choies");
                Console.WriteLine("press 1 to add a room");
                Console.WriteLine("press 2 add a emploee");
                
                int result = Convert.ToInt32(Console.ReadLine());
                switch (result)
                {
                    case 1:
                        Console.WriteLine($"inn the development Mode");
                        break;
                    case 2:
                        Console.WriteLine($"inn the development Mode");
                        break;
                    case 3:
                        Console.WriteLine($"inn the development Mode");
                        break;
                    case 4:
                        Console.WriteLine($"inn the development Mode");
                        break;
                    default:
                        Console.WriteLine("done!");
                        break;
                }
                Console.ReadKey();

            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Welcome " + acc.FullName);
            Console.WriteLine("Your email is " + acc.EmailAddress);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine("Press Any Key to Logout");
            Console.ResetColor();
            Console.ReadKey();
            Menu.Start();

        }
        else
        {
            Console.WriteLine("No account found with that email and password");
        }
    }
}