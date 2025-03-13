namespace The_Chosen_Few
{
    public class GameManager
    {
        private static int difficulty = 0;

        //Play points
        private static int faith = 0;

        //Upgrade points
        private static int ascension = 0;

        //Target points
        private static int influence = 0;
        private static int day = 0;

        private static int maxDays = 14;
        private static int neededDayInfluence = 0;
        private static int neededInfluence = 0;

        private static int startFaith = 30;

        public GameManager()
        {
            StartGame();
        }

        private static void StartGame()
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
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. The Cult of the void [EASY]");
            Console.WriteLine("2. The Cult of the Gray [MEDIUM]");
            Console.WriteLine("3. The Cult of the Black Sun [HARD]");

            //Repeat until difficulty chosen
            string? inputDif = Console.ReadLine() ?? string.Empty;
            while (difficulty == 0)
            {
                switch (inputDif)
                {
                    case "1":
                        difficulty = 1;
                        break;
                    case "2":
                        difficulty = 2;
                        break;
                    case "3":
                        difficulty = 3;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
                        inputDif = Console.ReadLine();
                        break;
                }
            }
            InitializeDifficulty();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\u001b[1mEnable tutorial? (Y / N)\u001b[0m");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            string? inputTutorial = Console.ReadLine() ?? string.Empty;
            while (inputTutorial != "Y" && inputTutorial != "N")
            {
                Console.WriteLine("Invalid input. Please enter Y or N.");
                inputTutorial = Console.ReadLine();
            }
            if (inputTutorial == "Y")
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("You must gather influence to ascend. Gather influence by going on expeditions. Beware, if you fail an expedition your fate will be sealed.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter to continue...");
                Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("When you enter an expedition, you will be given starting faith. You can use faith to play cards. Each card has a cost, and you can only play cards if you have enough faith. The dealt cards are random, and you need to play your hand efficiently. Played cards are read in order you play them, after you submit your turn. You will have 3 turns to play cards. Upon completing an expedition, the accumulated influence will be added to your total influence, and you will unlock a new card.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter to continue...");
                Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("You can visit the shrine to upgrade your cards with ascension points. Out of all your cards, random three cards will be selected, out of which you can upgrade only one.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter to continue...");
                Console.ReadLine();
            }
            Util.DisplayGlitchIntroMessage();
            Loop();
        }

        private static void InitializeDifficulty()
        {
            switch (difficulty)
            {
                case 1:
                    neededInfluence = 10000;
                    neededDayInfluence = 30;
                    break;
                case 2:
                    neededInfluence = 20000;
                    neededDayInfluence = 50;
                    break;
                case 3:
                    neededInfluence = 30000;
                    neededDayInfluence = 70;
                    break;
            }
        }

        public static void Loop()
        {
            while (day < maxDays)
            {
                DrawHomeUI();
                Console.WriteLine($"1. Go on an expedition [Needed Influence: {neededDayInfluence}]");
                Console.WriteLine("2. Visit the shrine");
                Console.WriteLine("3. Visit the chapel");
                string action = Console.ReadLine() ?? string.Empty;
                switch (action)
                {
                    case "1":
                        Expedition();
                        break;
                    case "2":
                        //Shrine();
                        break;
                    case "3":
                        //Chapel();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
                        break;
                }
            }
        }
        public static void Main(string[] args)
        {
            GameManager game = new GameManager();
        }

        public static void DrawHomeUI()
        {
            Console.Clear();
            Console.WriteLine($"\u001b[1m\u001b[32mDay {day} | Ascension: 0 | Influence: {influence} / {neededInfluence}\u001b[0m");
        }

        private static void Expedition()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\u001b[1mEXPEDITION\u001b[0m");
    Console.ResetColor();

    // Initialize expedition values
    int expeditionFaith = startFaith; // Starting faith
    int expeditionInfluence = 0; // Accumulated influence
    int turnsRemaining = 3; // Total turns
    CardManager cardManager = new CardManager(); // Card management
    List<Card> hand = new List<Card>(); // Player's hand
    List<Card> cardsPlayedThisTurn = new List<Card>(); // Track cards played this turn

    // Create a game state for this expedition
    GameState gameState = new GameState(cardManager);

    // Draw initial cards (assuming 5 cards)
    for (int i = 0; i < 5; i++)
    {
        // In a real implementation, you would draw cards from the deck
        hand.Add(new TheFirstSermon());
    }

    // Main expedition loop
    while (turnsRemaining > 0)
    {
        Console.Clear();
        Console.WriteLine($"\u001b[1mTurn {4 - turnsRemaining} of 3 | Faith: {expeditionFaith} | Influence: {expeditionInfluence}/{neededDayInfluence}\u001b[0m");
        
        // Display cards played this turn
        if (cardsPlayedThisTurn.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\u001b[1mCards Played This Turn:\u001b[0m");
            foreach (var playedCard in cardsPlayedThisTurn)
            {
                Console.WriteLine($"- {playedCard.Name} | Influence: {playedCard.InfluenceGain} | Faith Cost: {playedCard.FaithCost} | Faith Gain: {playedCard.FaithGain} | Multiplier: x{playedCard.InfluenceMultiplier.ToString("0.0")}");
            }
            Console.WriteLine();
        }
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Your hand:");

        // Display cards in hand with detailed info
        for (int i = 0; i < hand.Count; i++)
        {
            Card card = hand[i];
            Console.WriteLine($"{i + 1}. {card.Name} - Cost: {card.FaithCost} Faith | Influence: {card.InfluenceGain} | Faith Gain: {card.FaithGain} | Multiplier: x{card.InfluenceMultiplier.ToString("0.0")}");
            Console.WriteLine($"   {card.Description}");
        }

        Console.WriteLine("\nWhich card would you like to play? (Enter card number, or 0 to end turn)");

        // Process card selection
        string selection = Console.ReadLine() ?? string.Empty;
        if (selection == "0")
        {
            // End turn - clear the played cards list
            cardsPlayedThisTurn.Clear();
            turnsRemaining--;
            expeditionFaith = startFaith;
            continue;
        }

        // Try to parse the selection
        if (int.TryParse(selection, out int cardIndex) && cardIndex > 0 && cardIndex <= hand.Count)
        {
            Card selectedCard = hand[cardIndex - 1];

            // Check if player has enough faith
            if (expeditionFaith >= selectedCard.FaithCost)
            {
                // Play the card
                expeditionFaith -= selectedCard.FaithCost;
                expeditionFaith += selectedCard.FaithGain; // Apply any faith gain
                int gainedInfluence = selectedCard.Play(gameState);
                expeditionInfluence += gainedInfluence;

                // Add the card to played cards this turn
                cardsPlayedThisTurn.Add(selectedCard);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You played {selectedCard.Name} and gained {gainedInfluence} influence!");
                if (selectedCard.FaithGain > 0)
                    Console.WriteLine($"You also gained {selectedCard.FaithGain} faith!");
                Console.ResetColor();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();

                // Remove the played card from hand
                hand.RemoveAt(cardIndex - 1);

                // Draw a new card if the deck isn't empty
                hand.Add(new TheFirstSermon());
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not enough faith to play this card!");
                Console.ResetColor();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }
        else
        {
            Console.WriteLine("Invalid selection. Try again.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        // Check if all cards have been played
        if (hand.Count == 0)
        {
            Console.WriteLine("You have played all your cards. Turn ending.");
            cardsPlayedThisTurn.Clear(); // Clear played cards for next turn
            turnsRemaining--;
        }
    }

    // Expedition complete code remains the same...
}
    }
}