static class Menu
{  
    static public void Start()
    {
        bool running = true;

        // the menu keeps coming back until the user chooses to quit
        while (running)
        {
            Console.Clear();
            Console.WriteLine("Enter 1 to login");
            Console.WriteLine("Enter 2 to make account");
            Console.WriteLine("Enter 0 to quit");

            string input = Console.ReadLine();
            if (input == "1")
            {
                Console.Clear();
                UserLogin.Start();
            }
            else if (input == "2")
            {
                Console.Clear();
                UserRegister.Start();
            }
            else if (input == "0")
            {
                running = false;
            }
            else
            {
                Console.WriteLine("Invalid input. Press any key to try again.");
                Console.ReadKey();
            }
        }
    }
}