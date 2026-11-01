public static class ArrowMenu
{
    // returns the index of the chosen option, or -1 when Escape is pressed
    public static int Show(string title, List<string> options)
    {
        int selected = 0;

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(title);
            Console.ResetColor();
            Console.WriteLine("Use the arrow keys and press Enter to choose (Esc = back)\n");

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selected--;
                    if (selected < 0)
                        selected = options.Count - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selected++;
                    if (selected >= options.Count)
                        selected = 0;
                    break;
                case ConsoleKey.Enter:
                    return selected;
                case ConsoleKey.Escape:
                    return -1;
            }
        }
    }
}
