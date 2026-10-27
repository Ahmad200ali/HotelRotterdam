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
                Console.WriteLine("press 1 To Go ROOM MANAGEMENT DASHBOARD");
                Console.WriteLine("press 2 add a emploee");
                
                int result = Convert.ToInt32(Console.ReadLine());
                switch (result)
                {
                    case 1:
                        Room.Start();
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
                return;
            }
            GuestMenu.Start(acc);

        }
        else
        {
            Console.WriteLine("No account found with that email and password");
        }
    }
}
