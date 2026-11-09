public static class UserRegister
{
    static private UserLogic accountsLogic = new UserLogic();
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Register page");
        string Email;

        do
        {
            Console.WriteLine("Please enter your email address:");
            Email = Console.ReadLine();

        }
        while (!Check.CheckValidEmail(Email));

        string Passwoord;
        do
        {
            Console.WriteLine("Please enter your password (min. 8 characters, with a number and a special character):");
            Passwoord = UserLogin.ReadPassword();
        }
        while (!Check.CheckValidPassword(Passwoord));

        Console.WriteLine("Please enter your Name");
        string FullName = Console.ReadLine()!;
        Console.WriteLine("Please enter your Phone-Number");
        string PhoneNumber = Console.ReadLine()!;
        Console.WriteLine("Please enter your DateOfBirth (yyyy-mmmm-dddd)");
        string DateOfBirth = Console.ReadLine()!;
        bool registered = accountsLogic.Register(Email,Passwoord!,FullName,PhoneNumber,DateOfBirth);
        if (registered)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registration successful!");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("An account with this email already exists.");
            Console.ResetColor();
        }

        Console.WriteLine("Press any key to go back.");
        Console.ReadKey();
    }


}