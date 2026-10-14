
public static class Check
{
    public static bool CheckValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Email cannot be null!");
            Console.ResetColor();
            return false;
        }

        if (!email.Contains("@"))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Email should contain @. Please enter a valid email.");
            Console.ResetColor();
            return false;
        }

        if (!email.EndsWith(".com", StringComparison.OrdinalIgnoreCase) &&
            !email.EndsWith(".nl", StringComparison.OrdinalIgnoreCase))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Email should end with (.com) or (.nl). Please enter a valid email.");
            Console.ResetColor();
            return false;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Email is valid!");
        Console.ResetColor();

        return true;
    }
}
