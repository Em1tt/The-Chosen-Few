using System.Linq;
namespace The_Chosen_Few
{
    public class CardInfo
    {
        public CardInfo(string name, int upgradeLevel, int faithCost, int influenceGain, string description, int faithGain, float influenceMultiplier = 1.0f, int influenceGainPerGainedInfluence = 0, int faithGainPerGainedFaith = 0, int cardsToPickWhenPlayed = 0, int rarity = 1)
        {
            this.Name = name;
            this.UpgradeLevel = upgradeLevel;
            this.FaithCost = faithCost;
            this.InfluenceGain = influenceGain;
            this.Description = description;
            this.FaithGain = faithGain;
            this.InfluenceMultiplier = influenceMultiplier;
            this.InfluenceGainPerGainedInfluence = influenceGainPerGainedInfluence;
            this.FaithGainPerGainedFaith = faithGainPerGainedFaith;
            // Default rarity
            this.Rarity = rarity;
            this.CardsToPickWhenPlayed = cardsToPickWhenPlayed;
        }
        public string Name { get; set; }
        public int UpgradeLevel { get; set; }
        public int FaithCost { get; set; }
        public int FaithGain { get; set; }
        public int InfluenceGain { get; set; }
        public float InfluenceMultiplier { get; set; }
        public int InfluenceGainPerGainedInfluence { get; set; }
        public int FaithGainPerGainedFaith { get; set; }
        public string Description { get; set; }
        public int Rarity { get; set; }
        public int CardsToPickWhenPlayed { get; set; }
    }
    public abstract class Card
    {
        // Base properties
        protected int influenceGain;
        protected int influenceGainPerGainedInfluence;
        protected int faithCost;
        protected int faithGain;
        protected int faithGainPerGainedFaith;
        protected string? name;

        protected int rarity;
        protected string? description;
        protected int upgradeLevel = 0;
        protected int maxUpgradeLevel; // No default value, each card will set this

        // Card traits and effects
        protected int cardsToPickWhenPlayed = 0;
        protected float influenceMultiplier = 1.0f;

        // Public getters and setters
        public int InfluenceGainPerGainedInfluence => influenceGainPerGainedInfluence;
        public int FaithGainPerGainedFaith => faithGainPerGainedFaith;
        public int FaithCost { get => faithCost; set => faithCost = value; }
        public int FaithGain { get => faithGain; set => faithGain = value; }
        public int InfluenceGain { get => influenceGain; set => influenceGain = value; }
        public int Rarity => rarity;
        public float InfluenceMultiplier { get => influenceMultiplier; set => influenceMultiplier = value; }
        public string Name => name;
        public string Description => description;
        public int UpgradeLevel { get => upgradeLevel; set => upgradeLevel = value; }
        public int MaxUpgradeLevel => maxUpgradeLevel;
        public int CardsToPickWhenPlayed => cardsToPickWhenPlayed;

        // Play the card - returns gained influence
        public virtual void Play(GameState gameState, int cardIndex)
        {
            GameManager.cardsPlayed++;
            CardInfo cardInfo = GetCardInfoObject();
            GameManager.ExpeditionFaith -= cardInfo.FaithCost;
            GameManager.ExpeditionFaith += cardInfo.FaithGain;

            // Calculate influence with current multiplier
            int baseInfluence = cardInfo.InfluenceGain;
            int multipliedInfluence = (int)(baseInfluence * GameManager.CurrentTurnMultiplier);

            if (cardInfo.FaithGain > 0)
            {
                GameManager.FaithGainedTimes += 1;
            }
            if (cardInfo.InfluenceGain > 0)
            {
                GameManager.InfluenceGainedTimes++;
            }

            // Add to total influence
            GameManager.ExpeditionInfluence += multipliedInfluence;

            // Update the multiplier for subsequent cards
            if (cardInfo.InfluenceMultiplier > 1.0f)
            {
                if (GameManager.CurrentTurnMultiplier > 1)
                {
                    GameManager.CurrentTurnMultiplier += cardInfo.InfluenceMultiplier;
                }
                else
                {
                    GameManager.CurrentTurnMultiplier = cardInfo.InfluenceMultiplier;
                }
            }
            // Add the card to played cards this turn
            GameManager.CurrentPlayedCards.Add(cardInfo);

            // Handle card drawing if applicable
            if (cardInfo.CardsToPickWhenPlayed > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{Name} lets you draw {cardInfo.CardsToPickWhenPlayed} additional card(s)!");
                Console.ResetColor();

                // Draw the specified number of cards
                int cardsToDraw = cardInfo.CardsToPickWhenPlayed;
                int cardsDrawn = 0;

                for (int i = 0; i < cardsToDraw; i++)
                {
                    // Only draw if we have cards left in the deck
                    if (GameManager.CurrentPlayedCards.Count + GameManager.CurrentHand.Count <= GameManager.CardManager.GetPlayerDeck().Count)
                    {
                        // Get a random card that's not already in currentHand or played this turn
                        Card newCard = GameManager.CardManager.GetRandomCard(GameManager.CurrentHand, GameManager.CurrentPlayedCards);
                        if (newCard != null)
                        {
                            GameManager.CurrentHand.Add(newCard);
                            cardsDrawn++;
                        }
                    }
                }

                // Provide feedback about cards drawn
                if (cardsDrawn > 0)
                {
                    Console.WriteLine($"Drew {cardsDrawn} new card(s)!");
                }
                else
                {
                    Console.WriteLine("No cards left to draw!");
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            if (multipliedInfluence > baseInfluence)
            {
                Console.WriteLine($"You played {Name} and gained {baseInfluence} x{GameManager.CurrentTurnMultiplier} = {multipliedInfluence} influence!");
            }
            else
            {
                Console.WriteLine($"You played {Name} and gained {multipliedInfluence} influence!");
            }

            if (FaithGain > 0)
                Console.WriteLine($"You also gained {FaithGain} faith!");

            if (InfluenceMultiplier > 1.0f)
                Console.WriteLine($"Future cards this turn will have a x{GameManager.CurrentTurnMultiplier} multiplier!");

            Console.ResetColor();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            // Remove the played card from currentHand
            GameManager.CurrentHand.RemoveAt(cardIndex - 1);
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

        public virtual CardInfo GetCardInfoObject()
        {
            int influenceCalc = influenceGain;
            influenceCalc += influenceGainPerGainedInfluence * GameManager.InfluenceGainedTimes;
            int faithCalc = faithGain;
            faithCalc += faithGainPerGainedFaith * GameManager.FaithGainedTimes;
            CardInfo info = new CardInfo(
                name,
                upgradeLevel,
                faithCost,
                influenceCalc,
                description,
                faithCalc,
                influenceMultiplier,
                influenceGainPerGainedInfluence,
                faithGainPerGainedFaith,
                cardsToPickWhenPlayed,
                rarity
            );

            info.Rarity = this.rarity;
            return info;
        }

        // Override Equals to compare cards based on their type rather than reference
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            // We consider cards equal if they are the same type
            // You might want to check name or other properties instead
            return GetType() == obj.GetType();
        }

        // Always override GetHashCode when overriding Equals
        public override int GetHashCode()
        {
            // Use type name as hash code source
            return GetType().Name.GetHashCode();
        }
    }
    public class TheFirstSermon : Card
    {

        public TheFirstSermon()
        {
            name = "The First Sermon";
            description = "Your first message to the faithful. Simple but powerful. [FAITH GAIN + INFLUENCE]";
            faithCost = 20;
            faithGain = 5;
            rarity = 1;
            influenceGain = 20;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1f;
            maxUpgradeLevel = 5;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 18;
                    faithGain = 6;
                    influenceGain = 30;
                    break;
                case 2:
                    faithCost = 16;
                    faithGain = 7;
                    influenceGain = 38;
                    break;
                case 3:
                    faithCost = 14;
                    faithGain = 8;
                    influenceGain = 46;
                    break;
                case 4:
                    faithCost = 12;
                    faithGain = 9;
                    influenceGain = 54;
                    break;
                case 5:
                    faithCost = 10;
                    faithGain = 10;
                    influenceGain = 62;
                    break;
            }
        }

        public override void Play(GameState gameState, int cardIndex)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your words echo through the congregation...");
            Console.ResetColor();

            base.Play(gameState, cardIndex);
        }
    }
    public class GatherTheFaithful : Card
    {
        public GatherTheFaithful()
        {
            name = "Gather the faithful";
            description = "Recruit new followers to your cause. [PICK CARDS]";
            faithCost = 20;
            faithGain = 0;
            rarity = 1;
            influenceGain = 0;
            cardsToPickWhenPlayed = 1;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 2;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 14;
                    break;

                case 2:
                    faithCost = 8;
                    cardsToPickWhenPlayed = 2;
                    break;
            }
        }

        public override void Play(GameState gameState, int cardIndex)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You gather the faithful to your cause...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class TheRiteOfDivination : Card
    {
        public TheRiteOfDivination()
        {
            name = "The Rite of Divination";
            description = "Peer into the unknown for guidance. [FAITH GAIN]";
            faithCost = 30;
            faithGain = 60;
            rarity = 2;
            influenceGain = 0;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 5;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 24;
                    faithGain = 66;
                    break;
                case 2:
                    faithCost = 18;
                    faithGain = 72;
                    break;
                case 3:
                    faithCost = 12;
                    faithGain = 78;
                    break;
                case 4:
                    faithCost = 6;
                    faithGain = 81;
                    break;
                case 5:
                    faithCost = 1;
                    faithGain = 87;
                    break;
            }
        }
        public override void Play(GameState gameState, int cardIndex)
        {

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("The smoke swirls, the signs reveal themselves...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }

    public class SeanceOfTheLost : Card
    {
        public SeanceOfTheLost()
        {
            name = "Seance of the Lost";
            description = "Commune with spirits for hidden knowledge. [FAITH GAIN]";
            faithCost = 20;
            faithGain = 30;
            rarity = 1;
            influenceGain = 0;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 4;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 15;
                    faithGain = 32;
                    break;

                case 2:
                    faithCost = 10;
                    faithGain = 34;
                    break;

                case 3:
                    faithCost = 5;
                    faithGain = 36;
                    break;
                case 4:
                    faithCost = 1;
                    faithGain = 41;
                    break;
            }
        }

        public override void Play(GameState gameState, int cardIndex)
        {

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("The candle flickers as a whisper cuts through the veil...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class TheBloodlettingRitual : Card
    {
        public TheBloodlettingRitual()
        {
            name = "The Bloodletting Ritual";
            description = "Sacrifice blood for power. [INFLUENCE]";
            faithCost = 30;
            faithGain = 0;
            rarity = 1;
            influenceGain = 30;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 5;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 26;
                    influenceGain = 35;
                    break;
                case 2:
                    faithCost = 22;
                    influenceGain = 40;
                    break;
                case 3:
                    faithCost = 20;
                    influenceGain = 45;
                    break;
                case 4:
                    faithCost = 18;
                    influenceGain = 50;
                    break;
                case 5:
                    faithCost = 16;
                    influenceGain = 55;
                    break;
            }
        }

        public override void Play(GameState gameState, int cardIndex)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The crimson offering drips, and power surges within you...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class Kohenet : Card
    {
        public Kohenet()
        {
            name = "Kohenet";
            description = "Inspired by an ancient Jewish priestesses, this rite grants divine insight. [INFLUENCE + MULTIPLIER]";
            faithCost = 50;
            faithGain = 0;
            rarity = 4;
            influenceGain = 30;
            cardsToPickWhenPlayed = 1;
            influenceMultiplier = 1.2f;
            maxUpgradeLevel = 4;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 40;
                    cardsToPickWhenPlayed = 2;
                    influenceMultiplier = 1.4f;
                    break;
                case 2:
                    faithCost = 30;
                    cardsToPickWhenPlayed = 2;
                    influenceMultiplier = 1.6f;
                    break;
                case 3:
                    faithCost = 20;
                    cardsToPickWhenPlayed = 3;
                    influenceMultiplier = 1.9f;
                    break;
                case 4:
                    faithCost = 5;
                    cardsToPickWhenPlayed = 3;
                    influenceMultiplier = 2.2f;
                    break;
            }
        }

        public override void Play(GameState gameState, int cardIndex)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("A sacred voice speaks through you, unveiling divine mysteries...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class TheBlackMass : Card
    {
        public TheBlackMass()
        {
            name = "The Black Mass";
            description = "Profane the sacred, shattering the chains of faith. [FAITH GAIN]";
            faithCost = 25;
            faithGain = 30;
            rarity = 1;
            influenceGain = 0;
            cardsToPickWhenPlayed = 1;
            influenceMultiplier = 1f;
            maxUpgradeLevel = 3;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 20;
                    faithGain = 30;
                    break;

                case 2:
                    faithCost = 15;
                    faithGain = 35;
                    break;

                case 3:
                    faithCost = 10;
                    faithGain = 40;
                    break;
            }
        }

        public override void Play(GameState gameState, int cardIndex)
        {

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("The altar is overturned, the sacred made profane...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class TheDevilsPact : Card
    {
        public TheDevilsPact()
        {
            name = "The Devil's Pact";
            description = "A blood-bound contract for knowledge beyond mortal grasp. Gains more faith for everytime you gain faith during a single turn. [FAITH GAIN]";
            faithCost = 35;
            faithGainPerGainedFaith = 5;
            faithGain = 20;
            rarity = 2;
            influenceGain = 0;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1f;
            maxUpgradeLevel = 5;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 30;
                    faithGainPerGainedFaith = 8;
                    faithGain = 23;
                    break;

                case 2:
                    faithCost = 25;
                    faithGainPerGainedFaith = 9;
                    faithGain = 26;
                    break;

                case 3:
                    faithCost = 20;
                    faithGainPerGainedFaith = 10;
                    faithGain = 29;
                    break;
                case 4:
                    faithCost = 15;
                    faithGainPerGainedFaith = 11;
                    faithGain = 32;
                    break;
                case 5:
                    faithCost = 10;
                    faithGainPerGainedFaith = 12;
                    faithGain = 35;
                    break;
            }
        }

        public override void Play(GameState gameState, int cardIndex)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The ink is blood, the contract eternal...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class ChantOfTheFallenSeraph : Card
    {
        public ChantOfTheFallenSeraph()
        {
            name = "Chant of the Fallen Seraph";
            description = "Invoke Lucifer's wisdom through whispered incantations. [FAITH GAIN + INFLUENCE]";
            faithCost = 60;
            faithGainPerGainedFaith = 5;
            faithGain = 40;
            influenceGain = 30;
            influenceGainPerGainedInfluence = 5;
            rarity = 3;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1f;
            maxUpgradeLevel = 4;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 50;
                    faithGainPerGainedFaith = 8;
                    faithGain = 45;
                    influenceGain = 40;
                    influenceGainPerGainedInfluence = 7;
                    break;
                case 2:
                    faithCost = 40;
                    faithGainPerGainedFaith = 11;
                    faithGain = 50;
                    influenceGain = 50;
                    influenceGainPerGainedInfluence = 9;
                    break;
                case 3:
                    faithCost = 30;
                    faithGainPerGainedFaith = 14;
                    faithGain = 55;
                    influenceGain = 60;
                    influenceGainPerGainedInfluence = 11;
                    break;
                case 4:
                    faithCost = 20;
                    faithGainPerGainedFaith = 17;
                    faithGain = 60;
                    influenceGain = 70;
                    influenceGainPerGainedInfluence = 13;
                    break;
            }
        }

        public override void Play(GameState gameState, int cardIndex)
        {

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("A forbidden melody rises, carried on the wings of a fallen light...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }

    public class DemonicFavor : Card
    {
        public DemonicFavor()
        {
            name = "Demonic Favor";
            description = "Gain Influence equal to the half of the faith you have currently. [INFLUENCE]";
            faithCost = 25;
            faithGain = 0;
            rarity = 2;
            influenceGain = 0; // Base influence is 0, will be calculated dynamically
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 5;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 20;
                    break;

                case 2:
                    faithCost = 15;
                    break;

                case 3:
                    faithCost = 10;
                    break;

                case 4:
                    faithCost = 5;
                    break;

                case 5:
                    faithCost = 1;
                    break;
            }
        }

        // Override the calculation method to use current faith
        public override CardInfo GetCardInfoObject()
        {
            // Calculate the influence gain based on current faith when the card is played
            int calculatedInfluence = GameManager.ExpeditionFaith / 2;

            // Create a CardInfo snapshot with the fixed influence value
            return new CardInfo(
                name,
                upgradeLevel,
                faithCost,
                calculatedInfluence,  // This value is now fixed at the time of snapshot
                description,
                faithGain,
                influenceMultiplier,
                influenceGainPerGainedInfluence,
                faithGainPerGainedFaith,
                cardsToPickWhenPlayed,
                rarity
            );
        }
        // Get current faith before calculating influence
        public override void Play(GameState gameState, int cardIndex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("The demon smiles. 'A fair trade,' it whispers...");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class PreliminaryInvocation : Card
    {
        public PreliminaryInvocation()
        {
            name = "Preliminary Invocation";
            description = "Before true power can be harnessed, the first words must be spoken. Call upon the unseen forces to prepare the way. [MULTIPLIER + INFLUENCE]";
            faithCost = 50;
            faithGain = 10;
            rarity = 3;
            influenceGain = 30;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1.3f;
            maxUpgradeLevel = 3;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 45;
                    faithGain = 15;
                    influenceGain = 36;
                    influenceMultiplier = 1.4f;
                    break;

                case 2:
                    faithCost = 40;
                    faithGain = 20;
                    influenceGain = 42;
                    influenceMultiplier = 1.5f;
                    break;

                case 3:
                    faithCost = 35;
                    faithGain = 25;
                    influenceGain = 48;
                    influenceMultiplier = 1.6f;
                    break;
            }
        }
        // Get current faith before calculating influence
        public override void Play(GameState gameState, int cardIndex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\u001b[3mThe first words are spoken, the veil trembles... the ritual begins.\u001b[0m");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class Shemhamphorash : Card
    {
        public Shemhamphorash()
        {
            name = "Shemhamphorash";
            description = "The sacred and the profane intertwine as the hidden names of power are spoken, calling forth the forces beyond. [PICK CARDS + INFLUENCE]";
            faithCost = 35;
            faithGain = 5;
            rarity = 2;
            influenceGain = 15;
            cardsToPickWhenPlayed = 2;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 5;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 30;
                    faithGain = 8;
                    influenceGain = 20;
                    break;

                case 2:
                    faithCost = 25;
                    faithGain = 11;
                    influenceGain = 25;
                    break;

                case 3:
                    faithCost = 20;
                    faithGain = 14;
                    influenceGain = 30;
                    break;
                case 4:
                    faithCost = 15;
                    faithGain = 17;
                    influenceGain = 35;
                    break;
                case 5:
                    faithCost = 10;
                    faithGain = 20;
                    influenceGain = 40;
                    break;
            }
        }
        // Get current faith before calculating influence
        public override void Play(GameState gameState, int cardIndex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\u001b[3mThe true names are uttered—reality bends, the abyss answers.\u001b[0m");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class Primeumaton : Card
    {
        public Primeumaton()
        {
            name = "Primeumaton";
            description = "The first and final authority, an invocation of the highest forces beyond mortal comprehension. Gives a 3x multiplier if played as the fifth card. [INFLUENCE + MULTIPLIER]";
            faithCost = 60;
            faithGain = 0;
            rarity = 3;
            influenceGain = 20;
            cardsToPickWhenPlayed = 0;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 5;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 50;
                    influenceGain = 25;
                    break;

                case 2:
                    faithCost = 40;
                    influenceGain = 30;
                    break;

                case 3:
                    faithCost = 30;
                    influenceGain = 35;
                    break;
                case 4:
                    faithCost = 20;
                    influenceGain = 40;
                    break;
                case 5:
                    faithCost = 10;
                    influenceGain = 45;
                    break;
            }
        }

        public override CardInfo GetCardInfoObject()
        {
            // Calculate the influence gain based on current faith when the card is played
            int multiplier = GameManager.CurrentPlayedCards.Count == 4 ? 3 : 1;

            // Create a CardInfo snapshot with the fixed influence value
            return new CardInfo(
                name,
                upgradeLevel,
                faithCost,
                influenceGain,
                description,
                faithGain,
                multiplier,
                influenceGainPerGainedInfluence,
                faithGainPerGainedFaith,
                cardsToPickWhenPlayed,
                rarity
            );
        }

        // Get current faith before calculating influence
        public override void Play(GameState gameState, int cardIndex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\u001b[3mThe ancient name is spoken, and the cosmos shudders in response.\u001b[0m");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class Tetragrammaton : Card
    {
        public Tetragrammaton()
        {
            name = "Tetragrammaton";
            description = "The unspoken name of divinity, a word of power that shapes reality itself. [PICK CARDS]";
            faithCost = 20;
            faithGain = 0;
            rarity = 1;
            influenceGain = 0;
            cardsToPickWhenPlayed = 2;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 3;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 16;
                    break;

                case 2:
                    faithCost = 12;
                    break;

                case 3:
                    faithCost = 8;
                    break;
            }
        }

        // Get current faith before calculating influence
        public override void Play(GameState gameState, int cardIndex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\u001b[3mThe sacred name resounds, bending fate to your will.\u001b[0m");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
    public class Anaphaxeton : Card
    {
        public Anaphaxeton()
        {
            name = "Anaphaxeton";
            description = "A name of authority, invoking the forces that govern existence and bend reality to your will. Gives you 1 more card if this is your last card. [PICK CARDS]";
            faithCost = 28;
            faithGain = 4;
            rarity = 2;
            influenceGain = 0;
            cardsToPickWhenPlayed = 2;
            influenceMultiplier = 1.0f;
            maxUpgradeLevel = 2;
        }

        protected override void ApplyUpgradeEffects()
        {
            switch (upgradeLevel)
            {
                case 1:
                    faithCost = 25;
                    faithGain = 7;
                    break;

                case 2:
                    faithCost = 20;
                    faithGain = 12;
                    cardsToPickWhenPlayed = 3;
                    break;
            }
        }

        public override CardInfo GetCardInfoObject()
        {
            int cardsToPickCalc = cardsToPickWhenPlayed;
            if (GameManager.CurrentHand.Count == 1)
            {
                cardsToPickCalc++;
            }
            // Create a CardInfo snapshot with the fixed influence value
            return new CardInfo(
                name,
                upgradeLevel,
                faithCost,
                influenceGain,
                description,
                faithGain,
                influenceMultiplier,
                influenceGainPerGainedInfluence,
                faithGainPerGainedFaith,
                cardsToPickCalc,
                rarity
            );
        }

        // Get current faith before calculating influence
        public override void Play(GameState gameState, int cardIndex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\u001b[3mThe hidden command is spoken, and the fabric of fate trembles.\u001b[0m");
            Console.ResetColor();
            base.Play(gameState, cardIndex);
        }
    }
}