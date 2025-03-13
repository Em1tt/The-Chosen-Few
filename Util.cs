namespace The_Chosen_Few
{
    public class Util
    {
        public static void DisplayGlitchIntroMessage()
        {
            Console.Clear();
            Random random = new();

            // First message
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n\n\n");
            CenterText("You have 2 Weeks.");
            System.Threading.Thread.Sleep(2000);

            // Glitch effect
            for (int i = 0; i < 15; i++)
            {
                Console.Clear();
                Console.WriteLine("\n\n\n\n");

                // Random color glitches
                Console.ForegroundColor = (ConsoleColor)random.Next(9, 15);

                // Create glitch text with random characters
                string glitchText = "Y";
                if (i % 3 == 0) glitchText = "Y0u h@v3 2 W33k$.";
                if (i % 4 == 0) glitchText = "Yo# h#ve % We#ks.";
                if (i % 5 == 0) glitchText = "M@k3 th3m b3l13v3.";

                CenterText(glitchText);

                // Random horizontal static lines
                if (i % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    int line = random.Next(2, 6);
                    Console.SetCursorPosition(0, line);
                    Console.Write(new string('█', Console.WindowWidth));
                }

                System.Threading.Thread.Sleep(random.Next(50, 200));
            }

            // Final message
            Console.Clear();
            Console.WriteLine("\n\n\n\n");
            Console.ForegroundColor = ConsoleColor.Red;
            CenterText("Make them believe.");
            System.Threading.Thread.Sleep(2000);
            Console.ResetColor();
        }

        public static void CenterText(string text)
        {
            int centerX = (Console.WindowWidth / 2) - (text.Length / 2);
            Console.SetCursorPosition(centerX, Console.CursorTop);
            Console.WriteLine(text);
        }

        public static void LeftAndRightText(string leftText, string rightText)
        {
            int rightX = Console.WindowWidth - rightText.Length;
            Console.WriteLine(leftText.PadRight(rightX));
            Console.WriteLine(rightText);
        }

        public static void RightText(string text){
            int rightX = Console.WindowWidth - text.Length;
            Console.SetCursorPosition(rightX, Console.CursorTop);
            Console.WriteLine(text);
        }
    }
}