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

            Console.Clear();
            Console.ForegroundColor =ConsoleColor.DarkGreen;
            Console.WriteLine("Welcome" + acc.FullName);
            Console.WriteLine("Your email is " + acc.EmailAddress);
            Console.ResetColor();
            Console.ForegroundColor =ConsoleColor.DarkRed;
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