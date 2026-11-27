static class Menu
{
    static public void Start()
    {
        List<string> options = new List<string> { "Log in", "Create account", "Quit" };

        // the menu keeps coming back until the user chooses to quit
        while (true)
        {
            int choice = ArrowMenu.Show("HOTEL ROTTERDAM", options);

            switch (choice)
            {
                case 0:
                    Console.Clear();
                    UserLogin.Start();
                    break;
                case 1:
                    Console.Clear();
                    UserRegister.Start();
                    break;
                case 2:
                    return;
            }
        }
    }
}
