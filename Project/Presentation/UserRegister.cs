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

        Console.WriteLine("Please enter your password:");
        string Passwoord = Console.ReadLine();
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
            Console.ReadKey();
            Menu.Start();
            
        }
    }


}