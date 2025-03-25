namespace The_Chosen_Few
{
    public class Util
    {
        public static void StartGame()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  _______ _             _____ _                            ______            ");
            Console.WriteLine(" |__   __| |           / ____| |                          |  ____|           ");
            Console.WriteLine("    | |  | |__   ___  | |    | |__   ___  ___  ___ _ __   | |__ _____      __");
            Console.WriteLine("    | |  | '_ \\ / _ \\ | |    | '_ \\ / _ \\/ __|/ _ \\ '_ \\  |  __/ _ \\ \\ /\\ / /");
            Console.WriteLine("    | |  | | | |  __/ | |____| | | | (_) \\__ \\  __/ | | | | | |  __/\\ V  V / ");
            Console.WriteLine("    |_|  |_| |_|\\___|  \\_____|_| |_|\\___/|___/\\___|_| |_| |_|  \\___| \\_/\\_/  ");
            Console.WriteLine("                                                                             ");
            Console.WriteLine("                                                                             ");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The world is blind, lost in chaos, searching for meaning. But you? You have seen the truth. You are the voice of enlightenment, the bridge between mortals and the divine. With whispers and wonders, you gather the faithful, shaping their devotion into power.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("But the path to ascension is treacherous. Rivals spread their own twisted truths, the authorities watch with wary eyes, and even your most loyal followers may waver. Will you guide them to salvation, or will doubt and betrayal unravel your sacred vision?");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The time has come. The faithful are waiting. Will you lead them to the light… or into the abyss?");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\u001b[1mChoose your faction:\u001b[0m");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. The Cult of the void [EASY]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("2. The Cult of the Gray [MEDIUM]");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("3. The Cult of the Black Sun [HARD]");

            //Repeat until difficulty chosen
            string? inputDif = Console.ReadLine() ?? string.Empty;
            while (GameManager.difficulty == 0)
            {
                switch (inputDif)
                {
                    case "1":
                        GameManager.difficulty = 1;
                        break;
                    case "2":
                        GameManager.difficulty = 2;
                        break;
                    case "3":
                        GameManager.difficulty = 3;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
                        inputDif = Console.ReadLine();
                        break;
                }
            }
            GameManager.InitializeDifficulty();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\u001b[1mEnable tutorial? (Y / N)\u001b[0m");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            string? inputTutorial = Console.ReadLine() ?? string.Empty;
            while (inputTutorial?.ToUpper() != "Y" && inputTutorial?.ToUpper() != "N")
            {
                Console.WriteLine("Invalid input. Please enter Y or N.");
                inputTutorial = Console.ReadLine() ?? string.Empty;
            }
            if (inputTutorial?.ToUpper() == "Y")
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Your goal is to gather Influence and ascend. Influence is earned by going on Expeditions, but be warned — failure means doom.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter to continue...");
                Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Expeditions & Gameplay Flow");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. Starting Faith: Each Expedition begins with an initial amount of Faith.");
                Console.WriteLine("2. Playing Cards:");
                Console.WriteLine("   - Cards cost Faith to play. If you don't have enough, you can't use them.");
                Console.WriteLine("   - Your hand is randomly drawn, and you must use your cards efficiently.");
                Console.WriteLine("   - You have 2 turns per Expedition to play a maximum of 7 cards per turn.");
                Console.WriteLine("   - Cards give you bonuses in the order you play them. If a card gives you a multiplier, the multiplier is applied on the following cards only.");
                Console.WriteLine("3. Winning an expedition:");
                Console.WriteLine("   - Your total Influence from played cards is added to your overall Influence.");
                Console.WriteLine("   - Your Influence from this expedition is added to your Ascension.");
                Console.WriteLine("   - You need to reach a certain overall Influence in 10 days. If you fail, you lose the game.");
                Console.WriteLine("   - Completing an Expedition allows you to unlock a new card, expanding your deck.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter to continue...");
                Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Card Parameters");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Each card has unique attributes that affect its impact:");
                Console.WriteLine("   - Faith Cost - The amount of Faith required to play the card.");
                Console.WriteLine("   - Faith Gain - Some cards generate additional Faith, allowing you to play more.");
                Console.WriteLine("   - Influence Gain - Influence is the key to victory; some cards provide it directly.");
                Console.WriteLine("   - Influence Multiplier - Certain cards enhance Influence gained from other cards.");
                Console.WriteLine("   - Card Draw - Some cards allow you to draw additional cards for strategic advantage.");
                Console.WriteLine("   - Special Effects - Unique abilities like scaling rewards, modifying other cards, or triggering extra turns. A lot of times, these effects are hidden and must be discovered through gameplay.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter to continue...");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("The Shrine");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Between Expeditions, you can visit the Shrine to strengthen your deck using Ascension Points:");
                Console.WriteLine("   - Three random cards from your collection will be presented.");
                Console.WriteLine("   - You can upgrade one of the three, making it more powerful for future Expeditions.");
                Console.WriteLine("You can also visit the Chapel, where you can upgrade other aspects:");
                Console.WriteLine("   - Strengthen Faith Foundation - Gain additional faith when starting an Expedition.");
                Console.WriteLine("   - Expand Divine Knowledge - Increase the amount of cards you start an Expedition with.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter to continue...");
                Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Good luck, and may the divine light guide you.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter to continue...");
                Console.ReadLine();
            }
            Util.DisplayGlitchIntroMessage();
            GameManager.Loop();
        }
        public static void DisplayGlitchIntroMessage()
        {
            Console.Clear();
            Random random = new();

            // First message
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n\n\n");
            CenterText("You have 10 days.");
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

        public static void WinGame()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The whispers have become a chorus. The faithful no longer doubt, no longer question. They kneel in reverence, their voices raised in exaltation. The world has seen the truth—not through reason, but through revelation.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The old order crumbles. The blind masses open their eyes. Your word is law, your vision now reality. Cities burn, empires fall, and from the ashes, a new dawn rises—one shaped by your will alone.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You have ascended beyond mortal bounds. No longer a mere prophet, but a god in your own right.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("This is your dominion. This is your eternity.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"You have won the game. Congratulations!");
            // Display game statistics
            Console.WriteLine("\n\u001b[1m=-=-=-=-=-=-= YOUR LEGACY =-=-=-=-=-=-=\u001b[0m");

            // Cult name based on difficulty
            string cultName = GameManager.difficulty switch
            {
                1 => "The Cult of the Void",
                2 => "The Cult of the Gray",
                3 => "The Cult of the Black Sun",
                _ => "Unknown Cult"
            };

            Console.WriteLine($"\n\u001b[3mAs leader of \u001b[1m{cultName}\u001b[0m\u001b[3m, your influence spread across the world.\u001b[0m");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\u001b[1mFINAL STATISTICS:\u001b[0m");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"► Total Influence Gathered: \u001b[1m{GameManager.influence}\u001b[0m");
            Console.WriteLine($"► Cards Played: \u001b[1m{GameManager.cardsPlayed}\u001b[0m");
            Console.WriteLine($"► Cards Upgraded: \u001b[1m{GameManager.cardsUpgraded}\u001b[0m");

            // Calculate difficulty-based rating
            string rating;
            int score = GameManager.influence / GameManager.difficulty;

            if (score > 1250) rating = "Divine";
            else if (score > 1000) rating = "Immortal";
            else if (score > 750) rating = "Transcendent";
            else if (score > 500) rating = "Enlightened";
            else rating = "Ascended";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nFinal Rating: \u001b[1m{rating}\u001b[0m");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nThank you for playing The Chosen Few.");
            Console.WriteLine("\nPress Enter to exit the game...");
            Console.ReadLine();
        }

        public static void LoseGameTime()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Time was never your ally. The faithful wavered, their devotion turning to doubt. Whispers of uncertainty spread like rot, and your vision, once so clear, began to fracture.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The world did not wait. Rivals grew stronger, authorities tightened their grip, and your followers—lost and leaderless—drifted back into the abyss of the ordinary.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The great work remains unfinished. The truth, half-spoken, fades into silence.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Your name will be forgotten. Your cult, nothing more than a footnote in history.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter to continue...");
            Console.ResetColor();
            Console.ReadLine();
            DisplayGlitchDeathMessage();
        }

        public static void DisplayGlitchDeathMessage()
        {
            Console.Clear();
            Random random = new();

            // First message
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n\n\n");
            CenterText("You have gambled your soul away.");
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
                if (i % 3 == 0) glitchText = "Y0u h@v3 g@mbl3d y0ur s0ul @w@y.";
                if (i % 4 == 0) glitchText = "Yo# h#ve # so#l no m#re.";
                if (i % 5 == 0) glitchText = "Th3 v01d cl@1ms @ll.";

                CenterText(glitchText);

                // Random horizontal static lines
                if (i % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    int line = random.Next(2, 6);
                    Console.SetCursorPosition(0, line);
                    Console.Write(new string('█', Console.WindowWidth));
                }

                Thread.Sleep(random.Next(50, 200));
            }

            // Final message with skull ASCII art
            Console.Clear();
            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.Red;
            // ASCII Skull art
            CenterText("______");
            CenterText(".-\"      \"-.");
            CenterText("/            \\");
            CenterText("|              |");
            CenterText("|,  .-.  .-.  ,|");
            CenterText("| )(__/  \\__)( |");
            CenterText("|/     /\\     \\|");
            CenterText("(_     ^^     _)");
            CenterText("\\__|IIIIII|__/");
            CenterText("| \\IIIIII/ |");
            CenterText("\\          /");
            CenterText("`--------`");

            Thread.Sleep(3000);
            Console.ResetColor();

            // Display game statistics
            Console.WriteLine("\n\u001b[1m=-=-=-=-=-=-= YOUR LEGACY =-=-=-=-=-=-=\u001b[0m");

            // Cult name based on difficulty
            string cultName = GameManager.difficulty switch
            {
                1 => "The Cult of the Void",
                2 => "The Cult of the Gray",
                3 => "The Cult of the Black Sun",
                _ => "Unknown Cult"
            };

            Console.WriteLine($"\n\u001b[3mAs leader of \u001b[1m{cultName}\u001b[0m\u001b[3m, your influence spread across the world.\u001b[0m");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\u001b[1mFINAL STATISTICS:\u001b[0m");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"► Total Influence Gathered: \u001b[1m{GameManager.influence}\u001b[0m");
            Console.WriteLine($"► Cards Played: \u001b[1m{GameManager.cardsPlayed}\u001b[0m");
            Console.WriteLine($"► Cards Upgraded: \u001b[1m{GameManager.cardsUpgraded}\u001b[0m");

            // Calculate difficulty-based rating
            string rating;
            int score = GameManager.influence / GameManager.difficulty;

            if (score > 1250) rating = "Divine";
            else if (score > 1000) rating = "Immortal";
            else if (score > 750) rating = "Transcendent";
            else if (score > 500) rating = "Enlightened";
            else rating = "Ascended";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nFinal Rating: \u001b[1m{rating}\u001b[0m");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
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

        public static void RightText(string text)
        {
            int rightX = Console.WindowWidth - text.Length;
            Console.SetCursorPosition(rightX, Console.CursorTop);
            Console.WriteLine(text);
        }

        public static int CalculateNeededDayInfluence(int currentDay)
        {
            int baseDifficulty = GameManager.difficulty switch
            {
                1 => 20,
                2 => 30,
                3 => 40,
                _ => 20
            };

            double multiplier = GameManager.difficulty switch
            {
                1 => 2,
                2 => 2.1,
                3 => 2.35,
                _ => 2.0
            };

            double power = 2;
            double dayFactor = Math.Pow(currentDay + 1, power);

            return baseDifficulty + (int)(dayFactor * multiplier);
        }
    }
}