namespace The_Chosen_Few
{
    public class GameManager
    {
        public static void Main()
        {
            _ = new GameManager();
        }
        public static int difficulty = 0;
        private static int startingCards = 2;

        public static int cardsPlayed = 0;
        public static int cardsUpgraded = 0;

        //Upgrade points
        private static int ascension = 0;
        //Target points
        public static int influence = 0;
        private static int day = 0;
        private static int maxDays = 10;
        private static int neededDayInfluence = 0;
        private static int neededAscension = 20;
        private static int neededInfluence = 0;

        private static CardManager? cardManager; // Card management

        public static CardManager CardManager => cardManager;
        private static int startFaith = 40;
        private static int expeditionFaith = startFaith;
        private static int expeditionInfluence = 0;
        public static int ExpeditionFaith { get => expeditionFaith; set => expeditionFaith = value; }

        public static int ExpeditionInfluence { get => expeditionInfluence; set => expeditionInfluence = value; }

        private static float currentTurnMultiplier = 1.0f;
        public static float CurrentTurnMultiplier
        {
            get => currentTurnMultiplier;
            set => currentTurnMultiplier = value;
        }

        // Add access to faith gained times counter
        private static int faithGainedTimes = 0;
        public static int FaithGainedTimes
        {
            get => faithGainedTimes;
            set => faithGainedTimes = value;
        }

        // Add access to influence gained times counter
        private static int influenceGainedTimes = 0;
        public static int InfluenceGainedTimes
        {
            get => influenceGainedTimes;
            set => influenceGainedTimes = value;
        }

        // Add a method to access cards played this turn
        private static List<CardInfo> currentPlayedCards = new List<CardInfo>();

        public static List<CardInfo> CurrentPlayedCards { get => currentPlayedCards; set => currentPlayedCards = value; }

        // Add a method to get current currentHand
        private static List<Card> currentHand = new List<Card>();
        public static List<Card> CurrentHand { set => currentHand = value; get => currentHand; }

        private static int startFaithLevel = 1;
        private static int startingCardsLevel = 1;

        public GameManager()
        {
            Util.StartGame();
        }

        public static void InitializeDifficulty()
        {

            switch (difficulty)
            {
                case 1:
                    neededInfluence = 1200;
                    cardManager = new CardManager(1);
                    break;
                case 2:
                    neededInfluence = 2000;
                    cardManager = new CardManager(2);
                    break;
                case 3:
                    neededInfluence = 4000;
                    cardManager = new CardManager(3);
                    break;
            }
            neededDayInfluence = Util.CalculateNeededDayInfluence(day);
        }

        public static void Loop()
        {
            while (day < maxDays)
            {
                neededDayInfluence = Util.CalculateNeededDayInfluence(day);
                DrawHomeUI();
                Console.WriteLine($"1. Go on an expedition [Gather influence: {neededDayInfluence}]");
                Console.WriteLine($"2. Visit the shrine [Needed Ascension: {neededAscension}]");
                Console.WriteLine("3. Visit the chapel");
                string action = Console.ReadLine() ?? string.Empty;
                switch (action)
                {
                    case "1":
                        Expedition();
                        break;
                    case "2":
                        Shrine();
                        break;
                    case "3":
                        Chapel();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
                        break;
                }
            }
            //Win or lose condtion for the end
            if (day == maxDays)
            {
                if (influence >= neededInfluence)
                {
                    Util.WinGame();
                }
                else
                {
                    startFaithLevel = 1;
                    startingCardsLevel = 1;
                    Util.LoseGameTime();
                }
            }
        }

        public static void DrawHomeUI()
        {
            Console.Clear();
            Console.WriteLine($"\u001b[1m\u001b[32mDay {day+1}/{maxDays} | Ascension: {ascension} | Influence: {influence} / {neededInfluence}\u001b[0m");
        }

        private static void Expedition()
        {

            const int maxCardsPerTurn = 7;

            GameState gameState = new GameState(cardManager);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\u001b[1mEXPEDITION\u001b[0m");
            Console.ResetColor();
            expeditionFaith = startFaith;
            expeditionInfluence = 0;
            faithGainedTimes = 0;  // Use the static variable
            influenceGainedTimes = 0;  // Use the static variable
            int turnsRemaining = 2;
            currentHand = new List<Card>();  // Use the static variable
            currentPlayedCards = new List<CardInfo>();  // Use the static variable
            currentTurnMultiplier = 1.0f;

            if (cardManager == null)
            {
                Console.WriteLine("Error: Card manager not initialized. Please set difficulty first.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            // Draw initial cards
            for (int i = 0; i < startingCards; i++)
            {
                Card randomCard = cardManager.GetRandomCard(currentHand, currentPlayedCards);
                currentHand.Add(randomCard);
            }

            // Main expedition loop
            while (turnsRemaining > 0)
            {
                // Display turn info with multiplier
                Console.Clear();

                // Show played cards and currentHand
                if (currentPlayedCards.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\u001b[1mCards Played This Turn: ({currentPlayedCards.Count}/{maxCardsPerTurn})\u001b[0m");
                    DrawCardTable(currentPlayedCards.Cast<object>().ToList(), false, false);
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\u001b[1mYour Hand:\u001b[0m");
                DrawCardTable(currentHand.Cast<object>().ToList());
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\u001b[1mTurn {3 - turnsRemaining} of 2 | Faith: {expeditionFaith} | Influence: {expeditionInfluence}/{neededDayInfluence}\u001b[0m");
                Console.ResetColor();
                if (currentTurnMultiplier > 1.0f)
                {
                    Console.WriteLine($"\u001b[1m\u001b[33mActive Multiplier: x{currentTurnMultiplier.ToString("0.0")}\u001b[0m");
                }

                if (currentPlayedCards.Count >= maxCardsPerTurn)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You've played the maximum number of cards for this turn!");
                    Console.ResetColor();
                    Console.WriteLine("Press Enter to end turn...");
                    Console.ReadLine();

                    // End the turn automatically
                    currentTurnMultiplier = 1.0f;
                    faithGainedTimes = 0;
                    influenceGainedTimes = 0;
                    currentPlayedCards.Clear();
                    turnsRemaining--;
                    expeditionFaith = startFaith;
                    currentHand.Clear();

                    // Draw new cards for next turn
                    for (int i = 0; i < startingCards; i++)
                    {
                        Card randomCard = cardManager.GetRandomCard(currentHand, currentPlayedCards);
                        currentHand.Add(randomCard);
                    }
                    continue;
                }


                Console.WriteLine("Which card would you like to play? (Enter card number, or 0 to end turn)");

                string selection = Console.ReadLine() ?? string.Empty;
                if (selection == "0")
                {
                    currentTurnMultiplier = 1.0f;
                    faithGainedTimes = 0;
                    influenceGainedTimes = 0;
                    currentPlayedCards.Clear();
                    turnsRemaining--;
                    expeditionFaith = startFaith;
                    currentHand.Clear();

                    // Draw new cards for next turn
                    for (int i = 0; i < startingCards; i++)
                    {
                        Card randomCard = cardManager.GetRandomCard(currentHand, currentPlayedCards);
                        currentHand.Add(randomCard);
                    }
                    continue;
                }

                // Try to parse the selection
                if (int.TryParse(selection, out int cardIndex) && cardIndex > 0 && cardIndex <= currentHand.Count)
                {
                    Card selectedCard = currentHand[cardIndex - 1];

                    // Check if player has enough faith
                    if (expeditionFaith >= selectedCard.FaithCost)
                    {
                        // Play the card
                        selectedCard.Play(gameState, cardIndex);
                    }
                    else
                    {
                        // Not enough faith
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Not enough faith to play this card!");
                        Console.ResetColor();
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    }
                }
                else
                {
                    // Invalid selection
                    Console.WriteLine("Invalid selection. Try again.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }

            // Check if expedition was successful
            if (expeditionInfluence < neededDayInfluence)
            {
                // Expedition failed
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Expedition failed! You only gained {expeditionInfluence}/{neededDayInfluence} influence.");
                Console.ResetColor();
                Console.WriteLine("Press Enter to continue...");
                startFaithLevel = 1;
                startingCardsLevel = 1;
                Util.DisplayGlitchDeathMessage();
                Restart();
                Console.ReadLine();
                return;
            }

            // Expedition complete
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Expedition complete! You gained {expeditionInfluence} influence. Choose a card to unlock:");
            Console.ResetColor();

            // Get three random cards and store them
            List<Card> cardsToChooseFrom = cardManager.GetThreeRandomCards();

            for (int i = 0; i < cardsToChooseFrom.Count; i++)
            {
                Card card = cardsToChooseFrom[i];
                Console.ForegroundColor = card.Rarity switch
                {
                    1 => ConsoleColor.Gray,// Common
                    2 => ConsoleColor.Blue,// Rare
                    3 => ConsoleColor.Magenta,// Epic
                    4 => ConsoleColor.Yellow,// Legendary
                    _ => ConsoleColor.White,// Default color
                };
                Console.WriteLine($"{i + 1}. {card.Name} - Rarity: {card.Rarity}");
                Console.WriteLine($"   {card.Description}");
                Console.ResetColor();
            }

            Console.WriteLine("Enter the number of the card you want to unlock:");
            string? unlockSelection = Console.ReadLine();
            int unlockIndex;
            while (true)
            {
                // Try to parse as a card selection
                if (int.TryParse(unlockSelection, out unlockIndex) && unlockIndex > 0 && unlockIndex <= cardsToChooseFrom.Count)
                {
                    // Valid card selection - continue with the game
                    break;
                }

                // If we got here, the input was invalid or empty
                Console.WriteLine("Invalid selection. Please choose a card number:");
                unlockSelection = Console.ReadLine();
            }
            if (unlockIndex > 0 && unlockIndex <= cardsToChooseFrom.Count)
            {
                // Add the selected card to the player's deck
                Card unlockedCard = cardsToChooseFrom[unlockIndex - 1];
                cardManager.AddCardToDeck(unlockedCard);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You unlocked {unlockedCard.Name}!");
                Console.ResetColor();
                Console.WriteLine("Press Enter to continue...");

                // Increment day, add influence and ascension
                day++;
                influence += expeditionInfluence;
                ascension += expeditionInfluence;

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid selection. No card unlocked.");
                // Still increment day and add rewards
                day++;
                influence += expeditionInfluence;
                ascension += expeditionInfluence;

                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }

        private static void Shrine()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\u001b[1mSHRINE\u001b[0m");
            Console.ResetColor();

            // Check if player has enough ascension
            if (ascension < neededAscension)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You need {neededAscension} ascension to enter the shrine. You only have {ascension}.");
                Console.ResetColor();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            // Subtract the ascension cost
            ascension -= neededAscension;
            int currentCost = neededAscension;

            // Get three random cards from the player's deck
            List<Card> upgradeOptions = cardManager.GetRandomCardsFromDeck(3);

            if (upgradeOptions.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You don't have any cards to upgrade.");
                Console.ResetColor();
                // Refund full cost if no cards available
                ascension += currentCost;
                Console.WriteLine($"Your {currentCost} ascension has been refunded.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            // Main selection loop - continue until valid selection is made
            bool selectionMade = false;
            while (!selectionMade)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\u001b[1mSHRINE\u001b[0m");
                Console.ResetColor();

                Console.WriteLine($"You spend {currentCost} ascension to enter the shrine.");
                Console.WriteLine("Select a card to upgrade:");

                // Display available cards
                for (int i = 0; i < upgradeOptions.Count; i++)
                {
                    Card card = upgradeOptions[i];
                    Console.ForegroundColor = card.Rarity switch
                    {
                        1 => ConsoleColor.Gray,      // Common
                        2 => ConsoleColor.Blue,      // Rare
                        3 => ConsoleColor.Magenta,   // Epic
                        4 => ConsoleColor.Yellow,    // Legendary
                        _ => ConsoleColor.White,     // Default color
                    };

                    // Get the upgrade preview information
                    CardUpgradeInfo upgradeInfo = cardManager.GetCardUpgradeInfo(card);

                    Console.WriteLine($"{i + 1}. {card.Name} (Level {card.UpgradeLevel}+1/{card.MaxUpgradeLevel})");
                    if (card.FaithCost != upgradeInfo.NewFaithCost)
                    {
                        Console.WriteLine($"Faith cost: {card.FaithCost} » \u001b[1m{upgradeInfo.NewFaithCost}\u001b[0m");
                    }
                    if (card.InfluenceGain != upgradeInfo.NewInfluenceGain)
                    {
                        Console.WriteLine($"Influence: {card.InfluenceGain} » \u001b[1m{upgradeInfo.NewInfluenceGain}\u001b[0m");
                    }
                    if (card.FaithGain != upgradeInfo.NewFaithGain)
                    {
                        Console.WriteLine($"Faith Gain: {card.FaithGain} » \u001b[1m{upgradeInfo.NewFaithGain}\u001b[0m");
                    }
                    if (card.InfluenceMultiplier != upgradeInfo.NewInfluenceMultiplier)
                    {
                        Console.WriteLine($"Multiplier: x{card.InfluenceMultiplier.ToString("0.0")} » \u001b[1mx{upgradeInfo.NewInfluenceMultiplier.ToString("0.0")}\u001b[0m");
                    }
                    Console.WriteLine();
                    Console.ResetColor();
                }

                Console.WriteLine($"{upgradeOptions.Count + 1}. Leave (Refund {currentCost / 2} ascension)");

                string? selection = Console.ReadLine();

                if (int.TryParse(selection, out int selectedIndex) && selectedIndex > 0)
                {
                    if (selectedIndex <= upgradeOptions.Count)
                    {
                        // Upgrade the selected card
                        Card selectedCard = upgradeOptions[selectedIndex - 1];
                        cardManager.UpgradeCard(selectedCard); 

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You upgraded {selectedCard.Name} to level {selectedCard.UpgradeLevel}!");
                        Console.ReadLine();
                        Console.ResetColor();

                        // Increase the cost for next shrine visit
                        IncreaseShrineCost();

                        selectionMade = true;
                    }
                    else if (selectedIndex == upgradeOptions.Count + 1)
                    {
                        // Player chose to leave - refund half the cost
                        int refund = currentCost / 2;
                        ascension += refund;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"You leave the shrine. {refund} ascension has been refunded.");
                        Console.ResetColor();

                        selectionMade = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid selection. Please choose from the available options.");
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                }
            }
        }

        private static void IncreaseShrineCost()
        {
            // Non-linear increase in shrine cost
            double baseCost = difficulty switch
            {
                1 => 10.0,  // Easy
                2 => 15.0,  // Medium
                3 => 20.0,  // Hard
                _ => 10.0   // Default
            };

            int upgradeCount = GetTotalUpgradeCount();
            double exponent = 1.2;  // Less steep than expedition curve

            neededAscension = (int)(baseCost + Math.Pow(upgradeCount + 1, exponent) * 5);
        }

        private static int GetTotalUpgradeCount()
        {
            // Get the sum of all card levels in the player's deck
            return cardManager?.GetPlayerDeck()?.Sum(card => card.UpgradeLevel) ?? 0;
        }

        private static void Chapel()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\u001b[1mCHAPEL\u001b[0m");
            Console.ResetColor();
            Console.WriteLine("Here you can strengthen your spiritual foundation.");
            Console.WriteLine($"Current Ascension: {ascension}");
            Console.WriteLine();

            // Check if upgrades have reached their maximum levels
            bool faithUpgradeMaxed = startFaithLevel >= 8;
            bool cardsUpgradeMaxed = startingCardsLevel >= 4;

            // Calculate costs only if not maxed out
            int faithUpgradeCost = faithUpgradeMaxed ? 0 : CalculateChapelUpgradeCost(startFaithLevel, 1.4);
            int cardsUpgradeCost = cardsUpgradeMaxed ? 0 : CalculateChapelUpgradeCost(startingCardsLevel, 2.2);

            // Display options
            Console.Write("1. ");
            if (faithUpgradeMaxed)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Strengthen Faith Foundation ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[\u001b[1mMAX LEVEL\u001b[0m]");
                Console.ResetColor();
                Console.WriteLine($"   Current Starting Faith: \u001b[1m{startFaith}\u001b[0m (Level {startFaithLevel}/8)");
            }
            else
            {
                Console.WriteLine("Strengthen Faith Foundation");
                Console.WriteLine($"   Current Starting Faith: \u001b[1m{startFaith}\u001b[0m (Level {startFaithLevel}/8)");
                Console.WriteLine($"   Upgrade to \u001b[1m{startFaith + 10}\u001b[0m starting Faith");
                Console.WriteLine($"   Cost: \u001b[1m{faithUpgradeCost}\u001b[0m Ascension");
            }
            Console.WriteLine();

            Console.Write("2. ");
            if (cardsUpgradeMaxed)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Expand Divine Knowledge ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[\u001b[1mMAX LEVEL\u001b[0m]");
                Console.ResetColor();
                Console.WriteLine($"   Current Starting Cards: \u001b[1m{startingCards}\u001b[0m (Level {startingCardsLevel}/4)");
            }
            else
            {
                Console.WriteLine("Expand Divine Knowledge");
                Console.WriteLine($"   Current Starting Cards: \u001b[1m{startingCards}\u001b[0m (Level {startingCardsLevel}/4)");
                Console.WriteLine($"   Upgrade to \u001b[1m{startingCards + 1}\u001b[0m starting Cards");
                Console.WriteLine($"   Cost: \u001b[1m{cardsUpgradeCost}\u001b[0m Ascension");
            }
            Console.WriteLine();

            Console.WriteLine("0. Leave Chapel");

            string? selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    // Upgrade starting faith
                    if (faithUpgradeMaxed)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Faith foundation is already at maximum level (8/8).");
                        Console.ResetColor();
                    }
                    else if (ascension >= faithUpgradeCost)
                    {
                        ascension -= faithUpgradeCost;
                        startFaith += 10;
                        startFaithLevel++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Faith foundation strengthened! Starting Faith is now {startFaith}.");

                        // Check if we just reached max level
                        if (startFaithLevel >= 8)
                        {
                            Console.WriteLine("You have reached the maximum faith level!");
                        }

                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Not enough Ascension. You need {faithUpgradeCost}.");
                        Console.ResetColor();
                    }
                    break;

                case "2":
                    // Upgrade starting cards
                    if (cardsUpgradeMaxed)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Divine knowledge is already at maximum level (4/4).");
                        Console.ResetColor();
                    }
                    else if (ascension >= cardsUpgradeCost)
                    {
                        ascension -= cardsUpgradeCost;
                        startingCards += 1;
                        startingCardsLevel++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Divine knowledge expanded! Starting Cards is now {startingCards}.");

                        // Check if we just reached max level
                        if (startingCardsLevel >= 4)
                        {
                            Console.WriteLine("You have reached the maximum cards level!");
                        }

                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Not enough Ascension. You need {cardsUpgradeCost}.");
                        Console.ResetColor();
                    }
                    break;

                case "0":
                    // Leave chapel
                    Console.WriteLine("You leave the chapel.");
                    break;

                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private static int CalculateChapelUpgradeCost(int currentUpgradeLevel, double exponent = 1.6)
        {
            // Base cost + non-linear increase based on current level
            int baseCost = 30;

            // If it's the first upgrade, use the base cost
            if (currentUpgradeLevel <= 0)
                return baseCost;

            // Otherwise, calculate based on the upgrade level
            return baseCost + (int)(Math.Pow(currentUpgradeLevel, exponent) * 10);
        }

        // Add this utility method to draw tables
        private static void DrawCardTable(List<object> cards, bool isHand = true, bool showIndex = true)
        {
            // Define table column widths
            const int idxWidth = 3;
            const int nameWidth = 28;
            const int costWidth = 12;
            const int influenceWidth = 12;
            const int faithGainWidth = 12;
            const int multiplierWidth = 12;
            const int cardsToPickWidth = 12;

            // Draw table header
            string header = showIndex ? $"{"#".PadRight(idxWidth)} | " : "";
            header += $"{"Name".PadRight(nameWidth)} | {"Faith cost".PadRight(costWidth)} | {"Influence".PadRight(influenceWidth)} | {"Faith Gain".PadRight(faithGainWidth)} | {"Multiplier".PadRight(multiplierWidth)} | {"Pick cards".PadRight(cardsToPickWidth)}";

            Console.WriteLine(new string('-', header.Length));
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            // Draw each card row
            for (int i = 0; i < cards.Count; i++)
            {
                ConsoleColor color = ConsoleColor.White;
                string name = "Unknown Card";
                int rarity = 0;
                int faithCost = 0;
                int influenceGain = 0;
                int faithGain = 0;
                float influenceMultiplier = 1.0f;
                string description = "";
                int cardsToPick = 0;

                // Get properties based on object type
                if (cards[i] is Card card)
                {
                    CardInfo cardInfo = card.GetCardInfoObject();
                    name = cardInfo.Name;
                    rarity = cardInfo.Rarity;
                    faithCost = cardInfo.FaithCost;
                    influenceGain = cardInfo.InfluenceGain;
                    faithGain = cardInfo.FaithGain;
                    influenceMultiplier = cardInfo.InfluenceMultiplier;
                    description = cardInfo.Description;
                    cardsToPick = cardInfo.CardsToPickWhenPlayed;
                }
                else if (cards[i] is CardInfo info)
                {
                    name = info.Name;
                    rarity = info.Rarity;
                    faithCost = info.FaithCost;
                    influenceGain = info.InfluenceGain;
                    faithGain = info.FaithGain;
                    influenceMultiplier = info.InfluenceMultiplier;
                    description = info.Description;
                    cardsToPick = info.CardsToPickWhenPlayed;
                }

                // Set color based on rarity
                color = rarity switch
                {
                    1 => ConsoleColor.Gray,      // Common
                    2 => ConsoleColor.Blue,      // Rare
                    3 => ConsoleColor.Magenta,   // Epic
                    4 => ConsoleColor.Yellow,    // Legendary
                    _ => ConsoleColor.White,     // Default color
                };

                Console.ForegroundColor = color;

                string row = showIndex ? $"{(i + 1).ToString().PadRight(idxWidth)} | " : "";
                row += $"{name.PadRight(nameWidth)} | ";
                row += $"{faithCost.ToString().PadRight(costWidth)} | ";
                row += $"{influenceGain.ToString().PadRight(influenceWidth)} | ";
                row += $"{faithGain.ToString().PadRight(faithGainWidth)} | ";
                row += $"x{influenceMultiplier.ToString("0.0").PadRight(multiplierWidth - 1)}";
                row += $" | {cardsToPick.ToString().PadRight(cardsToPickWidth)}";

                Console.WriteLine(row);
            }

            Console.WriteLine(new string('-', header.Length));
            Console.ResetColor();
        }

        private static void Restart()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\u001b[1mYour journey has ended.\u001b[0m");
            Console.WriteLine("Would you like to start a new cult? (Y/N)");

            string? response = Console.ReadLine()?.ToUpper();

            if (response == "N")
            {
                // Exit the game
                Environment.Exit(0);
            }
            else if (response == "Y")
            {
                // Reset game state variables
                ascension = 0;
                influence = 0;
                day = 0;
                startingCards = 2;
                startFaith = 40;
                neededAscension = 20;

                Console.Clear();
                Util.StartGame();
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter Y or N.");
                Console.ReadLine();
                Restart();
            }
        }
    }
}