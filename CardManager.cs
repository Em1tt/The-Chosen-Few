namespace The_Chosen_Few
{
    public class CardManager
    {
        private List<Card> availableCards = [];
        private List<Card> playerDeck = [];

        public CardManager()
        {
            InitializeCards();
        }

        private void InitializeCards()
        {
            // Add initial cards to available cards
            availableCards.Add(new TheFirstSermon());
            // Add more starter cards here
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
            card.Upgrade();
        }
    }

    public abstract class Card
    {
        // Base properties
        protected int influenceGain;
        protected int faithCost;
        protected int faithGain;
        protected string name;
        protected string description;
        protected int upgradeLevel = 0;
        protected int maxUpgradeLevel; // No default value, each card will set this

        // Card traits and effects
        protected int cardsToPickWhenPlayed = 0;
        protected float influenceMultiplier = 1.0f;
        protected bool causesGlitch = false;
        protected bool targetsFaithful = false;

        // Public getters
        public int FaithCost => faithCost;
        public int FaithGain => faithGain;
        public int InfluenceGain => influenceGain;
        public float InfluenceMultiplier => influenceMultiplier;
        public string Name => name;
        public string Description => description;
        public int UpgradeLevel => upgradeLevel;

        // Play the card - returns gained influence
        public virtual int Play(GameState gameState)
        {
            // Basic functionality when card is played
            int gainedInfluence = CalculateInfluenceGain(gameState);

            // Handle card effects
            if (cardsToPickWhenPlayed > 0)
                gameState.DrawCards(cardsToPickWhenPlayed);

            if (causesGlitch)
                gameState.TriggerGlitchEffect();

            return gainedInfluence;
        }

        protected virtual int CalculateInfluenceGain(GameState gameState)
        {
            // Base calculation with multiplier
            return (int)(influenceGain * influenceMultiplier * gameState.GetCurrentMultiplier());
        }

        public virtual void Upgrade()
        {
            if (upgradeLevel < maxUpgradeLevel)
            {
                upgradeLevel++;
                ApplyUpgradeEffects();
            }
        }

        protected virtual void ApplyUpgradeEffects()
        {
            // Base upgrade effects - override in specific cards
            influenceGain += (int)(influenceGain * 0.25f); // 25% increase
            faithCost = Math.Max(1, faithCost - 1); // Decrease cost by 1, minimum 1
        }

        // Helper method to display card info
        public virtual string GetCardInfo()
        {
            return $"{name} (Level {upgradeLevel})\n" +
                   $"Cost: {faithCost} Faith | Influence: {influenceGain}\n" +
                   $"{description}";
        }
    }

    // Example card implementation
    public class TheFirstSermon : Card
    {
        public TheFirstSermon()
        {
            name = "The First Sermon";
            description = "Your first message to the faithful. Simple but powerful.";
            faithCost = 20;
            influenceGain = 30;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 10;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    influenceGain = 35;
                    faithCost = 2;
                    cardsToPickWhenPlayed = 1;
                    break;

                case 2:
                    description = "Your charismatic words inspire the faithful.";
                    influenceGain = 12;
                    faithCost = 1;
                    cardsToPickWhenPlayed = 1;
                    break;

                case 3:
                    description = "Your divine sermon reaches deep into the souls of your followers.";
                    influenceGain = 15;
                    faithCost = 1;
                    cardsToPickWhenPlayed = 2; // Now draws two cards!
                    break;
            }
        }

        public override int Play(GameState gameState)
        {
            // Special effect for The First Sermon
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Your words echo through the congregation...");
            Console.ResetColor();

            return base.Play(gameState);
        }
    }

    // Helper class to maintain game state
    public class GameState
    {
        private float currentMultiplier = 1.0f;
        private List<Card> hand = new List<Card>();
        private CardManager cardManager;

        public GameState(CardManager cardManager)
        {
            this.cardManager = cardManager;
        }

        public void DrawCards(int count)
        {
            // Implementation for drawing cards
            Console.WriteLine($"Drawing {count} cards...");
        }

        public void TriggerGlitchEffect()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Reality glitches around you...");
            Console.ResetColor();
        }

        public float GetCurrentMultiplier()
        {
            return currentMultiplier;
        }

        public void SetMultiplier(float multiplier)
        {
            currentMultiplier = multiplier;
        }
    }
}