using System;
using System.Collections.Generic;
using System.Text;

namespace Poker_Game
{
    /// <summary>
    ///     <brief>
    ///         This class manages most of the game. This Includes earnings, managing cards between classes and manageing the hand.
    ///     </brief>
    ///     <consts>
    ///     The first set of consts are limits for positioning in arrays and construction purposes.
    ///     
    ///     The second set are payout constants for the Payout() method.
    ///     </consts>
    ///     <fields>
    ///         Bankroll: holds the balance of the players money to bet.
    ///         Bet: holds the amount of money currently being bet.
    ///         DeckCards: The array that holds all of the cards in the deck.
    ///         Hand: Holds 5 cards. Taken from the deck.
    ///     </fields>
    ///     <Constructor>Since it is not possible any other way, there is only one constructor. It sets the deck with 52 cards objects. Sets the Bankroll at 1000$ and the bet at 1$. it also sets up the hand array but does not assign the cards to it yet.</Constructor>
    ///     <enums>
    ///     The first enum is a suit enum that helps target suits in loops.
    ///     The second is the numbers enum. Also helps target different values in cards and helps sort the cards in hand.
    ///     </enums>
    ///     <methods>
    ///         
    ///         SetDeck(): called once in the constructor. Sets all cards from Clubs to Spades and from Two to Ace.
    ///         
    ///         Shuffle(): Sorts the cards in a random pattern.
    ///         
    ///         BetMoney(): Sets the Bet amount and subtracts from Bankroll.
    ///         
    ///         SortCards(): Uses insetion method to sort the hand from least to greatest in number. (Two lowest and Ace highest).
    ///         
    ///         Deal(): Gives the hand the first 5 cards from the deck, much like real life! Sorts them with SortCards().
    ///         
    ///         Discard(): Adds a card from the hand to the deck. Removes it from hand.
    ///         
    ///         Redraw(): Replaces the cards removed from hand. Re-sorts them.
    ///         
    ///         Payout(): Runs through each win condition method. If win is detected: pay the appropriate amount and send a message to Program.
    ///         
    ///         RoyalFlush(): If a StraightFlush is detected: see if a ten is located at the first position of the hand. If true, it's a royal flush.
    /// 
    ///         StraightFlush(): If a Straight() and a Flush() are true, a StraightFlush is also true.
    ///         
    ///         FourOfAKind(): Counts instances of two like numbers in cards. If there are 4 cards with the same number, this method is true.
    ///         
    ///         FullHouse(): Checks for a ThreeOfAKind(). If true, checks for a pair. If both are true, so it this method.
    ///         
    ///         Flush(): All cards must have the same suit.
    ///         
    ///         Straight(): All cards most be in an incrementing sequence.
    ///         
    ///         ThreeOfAKind(): Like FourOfAKind(), but with three cards instead.
    ///         
    ///         TwoPair(): Counts two instances of pairs.
    ///         
    ///         OnePair(): Finds a pair that have numbers higher than or equal to a Jack.
    ///         
    ///         PutBack(): Puts all cards from the hand back into the deck.
    ///         
    ///         ToString(): This override shows all cards in hand.
    ///     </methods>
    /// </summary>
    class Deck
    {
        const int DECK_SIZE = 52;
        const int HAND_SIZE = 5;
        const int BANKROLL_START = 1000;
        const int MIN_BET = 1;
        const int REDRAW_START = 5;

        const int TWO_PAIR_PAY = 2;
        const int THREE_OF_A_KIND_PAY = 3;
        const int STRAIGHT_PAY = 4;
        const int FLUSH_PAY = 6;
        const int FULL_HOUSE_PAY = 9;
        const int FOUR_OF_A_KIND_PAY = 25;
        const int STRAIGHT_FLUSH_PAY = 50;
        const int ROYAL_FLUSH_PAY = 250;

        const int CARD1 = 0;
        const int CARD2 = 1;
        const int CARD3 = 2;
        const int CARD4 = 3;
        const int CARD5 = 4;

        private double _bankroll;
        private double _bet = 1;
        private Card[] _deckCards;
        private Card[] _hand;

        public enum Suits
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades
        }
        public enum Numbers
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }
        public double Bankroll
        {
            get
            {
                return _bankroll;
            }
            set
            {
                if (_bankroll < 0)
                    _bankroll = 0;
                else
                    _bankroll = value;
            }
        }
        public double Bet
        {
            get
            {
                return _bet;
            }
            set
            {
                if (_bet >= MIN_BET)
                    _bet = value;
                else
                    throw new ArgumentOutOfRangeException(String.Format("Bet less than ", MIN_BET));
            }
        }
        public Card[] DeckCards
        {
            get
            {
                return _deckCards;
            }
            set
            {
                _deckCards = value;
            }
        }
        public Card[] Hand 
        {
            get
            {
                return _hand;
            }
            set
            {
                _hand = value;
            }
        }
        public Deck()
        {
            Bankroll = BANKROLL_START;
            Bet = 1;
            DeckCards = new Card[DECK_SIZE];
            SetDeck();
            Hand = new Card[HAND_SIZE];
        }
        //called during deck construction
        private void SetDeck()
        {
            int count = 0;
            //loop creates all of the cards in the game
            for (int i = (int)Suits.Clubs; i < (int)Suits.Spades + 1; i++)
            {
                for (int j = (int)Numbers.Two; j < (int)Numbers.Ace + 1; j++)
                {
                    DeckCards[count] = new Card(Convert.ToString((Suits)i), j);
                    count++;
                }
            }
        }
        //at the start of every round, shuffle the cards.
        public void Shuffle()
        {
            Random r = new Random();
            int rPos;
            Card swapper;
            for (int i = 0; i < DeckCards.Length; i++)
            {
                //uses random generator for this.
                rPos = r.Next(DeckCards.Length);
                swapper = DeckCards[i];
                DeckCards[i] = DeckCards[rPos];
                DeckCards[rPos] = swapper;
            }
        }
        public void BetMoney(double money)
        {
            //would have included code as part of a property, but the code affects two fields.
            Bet = money;
            Bankroll -= Bet;
        }

        //uses insertion method, great for small collections like the hand
        private void SortCards()
        {
            Card swap;
            for (int pos = 1; pos < Hand.Length; pos++)
            {
                //will search "behind" in the previous positions to find larger elements to switch with
                for (int sort = pos; sort > 0 && Hand[sort].Number < Hand[sort - 1].Number; sort--)
                {
                    swap = Hand[sort];
                    Hand[sort] = Hand[sort - 1];
                    Hand[sort - 1] = swap;
                }
            }
        }

        //draws the first 5 cards from deck, gives to hand.
        public void Deal()
        {
            //gives the cards to the hand
            for (int i = 0; i < HAND_SIZE; i++)
            {
                Hand[i] = DeckCards[i];
                DeckCards[i] = null;
            }
            SortCards();
        }
        
        public void Discard(int handPos)
        {
            //the reason for the -1 is so that input can conform to the hand card list numbers and to avoid the user hitting zero, which exits the loop.
            int count = 0;
            handPos -= 1;
            if(Hand[handPos] != null)
            {
                while (DeckCards[count] != null)
                {
                    count++;
                }
                DeckCards[count] = Hand[handPos];
                Hand[handPos] = null;
            }
        }
        //redraws all cards missing (nulled) from hand.
        public void Redraw()
        {
            int pos = REDRAW_START;
            for (int i = 0; i < Hand.Length; i++)
            {
                if(Hand[i] == null)
                {
                    Hand[i] = DeckCards[pos];
                    DeckCards[pos] = null;
                    pos++;
                }
            }
            SortCards();
        }
        //checks all win conditions, takes the win with the highest payout. if nothing, claim a loss.
        public void Payout()
        {
            if (RoyalFlush())
            {
                Bankroll += (Bet * ROYAL_FLUSH_PAY);
                Program.PrintSuccess("Win: Royal Flush!!!!!");
            }
            else if (StraightFlush())
            {
                Bankroll += (Bet * STRAIGHT_FLUSH_PAY);
                Program.PrintSuccess("Win: Straight Flush!");
            }
            else if (FourOfAkind())
            {
                Bankroll += (Bet * FOUR_OF_A_KIND_PAY);
                Program.PrintSuccess("Win: Four of a kind!");
            }
            else if (FullHouse())
            {
                Bankroll += (Bet * FULL_HOUSE_PAY);
                Program.PrintSuccess("Win: Full House!");
            }
            else if (Flush())
            {
                Bankroll += (Bet * FLUSH_PAY);
                Program.PrintSuccess("Win: Flush");
            }
            else if (Straight())
            {
                Bankroll += (Bet * STRAIGHT_PAY);
                Program.PrintSuccess("Win: Straight");
            }
            else if (ThreeOfAKind())
            {
                Bankroll += (Bet * THREE_OF_A_KIND_PAY);
                Program.PrintSuccess("Win: Three of a kind");
            }
            else if (TwoPair())
            {
                Bankroll += (Bet * TWO_PAIR_PAY);
                Program.PrintSuccess("Win: Two Pair");
            }
            else if (OnePair())
            {
                Bankroll += Bet;
                Program.PrintSuccess("Win: One Pair");
            }
            else
            {
                Program.PrintError("No Winning combo detected . . .");
            }
        }
        //at the end of the round, put all cards in hand back.
        public void PutBack()
        {
            int count = 0;
            //the idea is to do a for loop for each card and do a while until a null value is found.
            for (int i = 0; i < Hand.Length; i++)
            {
                
                while (DeckCards[count] != null)
                {
                    count++;
                }
                DeckCards[count] = Hand[i];
                Hand[i] = null;
            }
        }


        //Start of win conditions refer to diagram or summary for a better understanding of how these methods function.
        public bool RoyalFlush()
        {
            if (StraightFlush())
            {
                if (Hand[0].Number == (int)Numbers.Ten)
                    return true;
            }

            return false;
        }
        private bool StraightFlush()
        {
            if (Straight() && Flush())
                return true;

                return false;
        }
        private bool FourOfAkind()
        {
            int count = 0;
            for (int i = 0; i < Hand.Length - 1; i++)
            {
                if (Hand[i].Number == Hand[i + 1].Number)
                    count++;
                else
                    count = 0;
                if (count >= 3)
                    return true;
            }
            return false;
        }
        private bool FullHouse()
        {
            if (ThreeOfAKind())
            {
                //the reason I can do this is because I can expect a sorted hand. Therefore, the only way a pair would be possible is at the beginning or at the end.
                if (Hand[CARD1].Number == Hand[CARD2].Number && Hand[CARD2].Number != Hand[CARD3].Number)
                    return true;
                else if (Hand[CARD3].Number != Hand[CARD4].Number && Hand[CARD4].Number == Hand[CARD5].Number)
                    return true;

                return false;
            }
            else
                return false;
        }
        private bool Flush()
        {
            for (int i = 0; i < Hand.Length - 1; i++)
            {
                if (Hand[i].Suit != (Hand[i + 1].Suit))
                    return false;
            }
            return true;
        }
        private bool Straight()
        {
            for (int i = 0; i < Hand.Length - 1; i++)
            {
                if (Hand[i].Number != (Hand[i + 1].Number - 1))
                    return false;
            }
            return true;
        }
        private bool ThreeOfAKind()
        {
            int count = 0;
            for (int i = 0; i < Hand.Length - 1; i++)
            {
                if (Hand[i].Number == Hand[i + 1].Number)
                    count++;
                else
                    count = 0;
                if (count >= 2)
                    return true;
            }
            return false;
        }
        private bool TwoPair()
        {
            int count = 0;

            for (int i = 0; i < Hand.Length - 1; i++)
            {
                if (Hand[i].Number == Hand[i + 1].Number)
                {
                    count++;
                }
            }
            if (count >= 2)
            {
                return true;
            }
            return false;
        }
        
        private bool OnePair()
        {
            for (int i = 0; i < Hand.Length - 1; i++)
            {
                if (Hand[i].Number == Hand[i + 1].Number && Hand[i].Number >= (int)Numbers.Jack && Hand[i].Number >= (int)Numbers.Jack)
                {
                    return true;
                }
            }
            return false;
        }
        //end of win conditions



        public override string ToString()
        {
            //shows the hand.
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Hand.Length; i++)
            {
                builder.Append(String.Format("[{0}] {1}\n", (i + 1), Hand[i].ToString()));
            }
            return builder.ToString();
        }

    }
}
