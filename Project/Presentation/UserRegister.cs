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

        string Password;
        do
        {
            Console.WriteLine("Please enter your password (min. 8 characters, with a number and a special character):");
            Password = UserLogin.ReadPassword();
        }
        while (!Check.CheckValidPassword(Password));

        string FullName;
        do
        {
            Console.WriteLine("Please enter your Name");
            FullName = Console.ReadLine()!;
        }
        while (!Check.CheckValidName(FullName));

        string PhoneNumber;
        do
        {
            Console.WriteLine("Please enter your Phone-Number");
            PhoneNumber = Console.ReadLine()!;
        }
        while (!Check.CheckValidPhoneNumber(PhoneNumber));

        string DateOfBirth;
        do
        {
            Console.WriteLine("Please enter your DateOfBirth (dd-mm-yyyy)");
            DateOfBirth = Console.ReadLine()!;
        }
        while (!Check.CheckValidDateOfBirth(DateOfBirth));

        bool registered = accountsLogic.Register(Email,Password!,FullName,PhoneNumber,DateOfBirth);
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