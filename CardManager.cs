namespace The_Chosen_Few
{
    public class CardManager
    {
        private List<Card> availableCards = [];
        private List<Card> playerDeck = [];

        public CardManager(int difficulty)
        {
            InitializeCards();
            InitializeDeckBasedOnDifficulty(difficulty);
        }

        public Card UnlockCard(int index)
        {
            // Get unobtained cards
            List<Card> unobtainedCards = availableCards.Except(playerDeck).ToList();
            List<Card> selectedCards = GetThreeRandomCards();

            if (index > 0 && index <= selectedCards.Count)
            {
                Card unlockedCard = selectedCards[index - 1];
                // Add the card to the player's deck
                AddCardToDeck(unlockedCard);
                return unlockedCard;
            }
            throw new ArgumentOutOfRangeException(nameof(index), "Invalid card index.");
        }

        public Card GetRandomCard(List<Card> hand, List<CardInfo> played)
        {
            Random rnd = new Random();
            Card randomCard;
            do
            {
                int index = rnd.Next(playerDeck.Count);
                randomCard = playerDeck[index];
            } while (hand.Contains(randomCard) || played.Any(card => card.Name == randomCard.Name));
            return randomCard;
        }

        public List<Card> GetThreeRandomCards()
        {
            // Get all cards that haven't been obtained yet 
            List<Card> unobtainedCards = availableCards.Except(playerDeck).ToList();
            List<Card> selectedCards = new List<Card>();

            Random rnd = new Random();

            // If we have 3 or fewer unobtained cards, add all of them
            if (unobtainedCards.Count <= 3)
            {
                return new List<Card>(unobtainedCards);
            }

            // Otherwise, select 3 cards using weighted random selection
            for (int i = 0; i < 3; i++)
            {
                if (unobtainedCards.Count == 0) break;

                // Calculate inverse weights - the lower the rarity number, the higher the chance
                // 5 - rarity ensures rarity 1 has weight 4, and rarity 4 has weight 1
                int totalWeight = unobtainedCards.Sum(card => 5 - card.Rarity);
                int randomWeight = rnd.Next(totalWeight);
                int currentWeight = 0;

                foreach (var card in unobtainedCards)
                {
                    // Use inverse weighting: higher rarity (4) has lower chance than lower rarity (1)
                    currentWeight += 5 - card.Rarity;
                    if (currentWeight >= randomWeight)
                    {
                        selectedCards.Add(card);
                        unobtainedCards.Remove(card);
                        break;
                    }
                }
            }

            return selectedCards;
        }

        public void InitializeDeckBasedOnDifficulty(int difficulty)
        {
            switch (difficulty)
            {
                case 1:
                    InitializeEasyCards();
                    break;
                case 2:
                    InitializeMediumCards();
                    break;
                case 3:
                    InitializeHardCards();
                    break;
                default:
                    throw new ArgumentException("Invalid difficulty level");
            }
        }

        private void InitializeEasyCards()
        {
            playerDeck.Add(new TheFirstSermon());
            playerDeck.Add(new SeanceOfTheLost());
            playerDeck.Add(new TheBloodlettingRitual());
            playerDeck.Add(new Tetragrammaton());
        }

        private void InitializeMediumCards()
        {
            // Add medium level cards
            playerDeck.Add(new TheFirstSermon());
            playerDeck.Add(new TheBloodlettingRitual());
            playerDeck.Add(new Tetragrammaton());
        }

        private void InitializeHardCards()
        {
            // Add hard level cards
            playerDeck.Add(new TheFirstSermon());
            playerDeck.Add(new TheBloodlettingRitual());
        }
        private void InitializeCards()
        {
            availableCards.Add(new TheFirstSermon());
            availableCards.Add(new GatherTheFaithful());
            availableCards.Add(new TheRiteOfDivination());
            availableCards.Add(new SeanceOfTheLost());
            availableCards.Add(new TheBloodlettingRitual());
            availableCards.Add(new Kohenet());
            availableCards.Add(new TheBlackMass());
            availableCards.Add(new TheDevilsPact());
            availableCards.Add(new ChantOfTheFallenSeraph());
            availableCards.Add(new DemonicFavor());
            availableCards.Add(new PreliminaryInvocation());
            availableCards.Add(new Shemhamphorash());
            availableCards.Add(new Primeumaton());
            availableCards.Add(new Tetragrammaton());
            availableCards.Add(new Anaphaxeton());
        }

        public List<Card> GetPlayerDeck()
        {
            return playerDeck;
        }

        public void AddCardToDeck(Card card)
        {
            playerDeck.Add(card);
        }

        public void UpgradeCard(Card card)
        {
            GameManager.cardsUpgraded++;
            // We don't need to manually set properties
            // Just increase the level and let the card class handle the upgrades
            card.UpgradeLevel++;

            // Call the card's ApplyUpgradeEffects method using reflection
            try
            {
                Type cardType = card.GetType();
                var upgradeMethod = cardType.GetMethod("ApplyUpgradeEffects",
                                                    System.Reflection.BindingFlags.Instance |
                                                    System.Reflection.BindingFlags.NonPublic);

                if (upgradeMethod != null)
                {
                    upgradeMethod.Invoke(card, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying upgrade: {ex.Message}");
            }
        }

        public List<Card> GetRandomCardsFromDeck(int count)
        {
            List<Card> result = new List<Card>();
            List<Card> availableForUpgrade = playerDeck.Where(c => c.UpgradeLevel < c.MaxUpgradeLevel).ToList();

            // If there are no cards to upgrade, return empty list
            if (availableForUpgrade.Count == 0)
                return result;

            // If we have fewer cards than requested, return them all
            if (availableForUpgrade.Count <= count)
                return new List<Card>(availableForUpgrade);

            // Select 'count' random cards
            Random rnd = new Random();
            while (result.Count < count && availableForUpgrade.Count > 0)
            {
                int index = rnd.Next(availableForUpgrade.Count);
                result.Add(availableForUpgrade[index]);
                availableForUpgrade.RemoveAt(index);
            }

            return result;
        }

        public CardUpgradeInfo GetCardUpgradeInfo(Card card)
        {
            // Create a new info object
            CardUpgradeInfo info = new CardUpgradeInfo();

            // We'll use reflection to get the next upgrade stats
            // This works by creating a temporary copy of the card and applying the upgrade

            // 1. Create a temporary copy of the card
            Card tempCard;
            try
            {
                // Create a new instance of the specific card type
                tempCard = (Card)Activator.CreateInstance(card.GetType());

                // Copy the current stats
                tempCard.FaithCost = card.FaithCost;
                tempCard.FaithGain = card.FaithGain;
                tempCard.InfluenceGain = card.InfluenceGain;
                tempCard.InfluenceMultiplier = card.InfluenceMultiplier;

                // Set the upgrade level to current + 1 (to see next level)
                tempCard.UpgradeLevel = card.UpgradeLevel + 1;

                // Call the card's internal upgrade method (simulates an upgrade)
                Type cardType = tempCard.GetType();
                var upgradeMethod = cardType.GetMethod("ApplyUpgradeEffects",
                                                     System.Reflection.BindingFlags.Instance |
                                                     System.Reflection.BindingFlags.NonPublic);

                if (upgradeMethod != null)
                {
                    upgradeMethod.Invoke(tempCard, null);
                }

                // Get the upgraded stats
                info.NewFaithCost = tempCard.FaithCost;
                info.NewFaithGain = tempCard.FaithGain;
                info.NewInfluenceGain = tempCard.InfluenceGain;
                info.NewInfluenceMultiplier = tempCard.InfluenceMultiplier;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting upgrade info: {ex.Message}");

                // Fallback to basic calculation if reflection fails
                info.NewFaithCost = Math.Max(1, card.FaithCost - 3);
                info.NewFaithGain = card.FaithGain + 3;
                info.NewInfluenceGain = card.InfluenceGain;
                info.NewInfluenceMultiplier = card.InfluenceMultiplier + 0.1;
            }

            return info;
        }

        public List<Card> DrawCards(int count, List<Card> playedCards)
        {
            // Get available cards (cards in the deck that haven't been played this turn)
            List<Card> availableCards = playerDeck.Where(card => !playedCards.Contains(card)).ToList();

            // Prepare the result list
            List<Card> drawnCards = new List<Card>();

            // If no cards are available, return an empty list
            if (availableCards.Count == 0)
                return drawnCards;

            // Determine how many cards we can actually draw
            int cardsToDrawCount = Math.Min(count, availableCards.Count);

            // Use random selection to draw cards
            Random rnd = new Random();
            for (int i = 0; i < cardsToDrawCount; i++)
            {
                int index = rnd.Next(availableCards.Count);
                drawnCards.Add(availableCards[index]);
                availableCards.RemoveAt(index);
            }

            return drawnCards;
        }
    }

    // Helper class to maintain game state
    public class GameState
    {
        public float GetCurrentMultiplier()
        {
            return currentMultiplier;
        }
        public float currentMultiplier { get; set; } = 1.0f;
        private List<Card> hand = new List<Card>();
        private CardManager cardManager;

        public GameState(CardManager cardManager)
        {
            this.cardManager = cardManager;
        }

        public void DrawCards(int count, List<Card> playedCards)
        {
            // Use the CardManager's method to draw cards
            List<Card> drawnCards = cardManager.DrawCards(count, playedCards);

            // Add the drawn cards to the hand
            foreach (Card card in drawnCards)
            {
                hand.Add(card);
            }

            // Inform the player about the drawn cards
            Console.WriteLine($"Drew {drawnCards.Count} card(s).");
        }

        public List<Card> GetHand()
        {
            return hand;
        }

        public void ClearHand()
        {
            hand.Clear();
        }

        public void SetMultiplier(float multiplier)
        {
            currentMultiplier = multiplier;
        }
    }

    // Add this class to store upgrade preview information
    public class CardUpgradeInfo
    {
        public int NewFaithCost { get; set; }
        public int NewInfluenceGain { get; set; }
        public int NewFaithGain { get; set; }
        public double NewInfluenceMultiplier { get; set; }
    }
}