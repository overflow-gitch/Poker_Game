using System;
using System.Collections.Generic;
using System.Text;

namespace Poker_Game
{
    /// <summary>
    ///     <brief>
    ///         This class creates and manages card objects.
    ///     </brief>
    ///     <fields>
    ///         string _suit/Suit: informs the suit of the card; Either Clubs, Diamonds, Hearts and Spades.
    ///         
    ///         int _number/Number: holds a value, that when paired up with the appropriate enum will show a value from "Two" to "Ace". This field also has validation as it won't accept values outside of "Two" to "Ace".
    ///     </fields>
    ///     <Constructor>Since it is not possible any other way, there is only one constructor. It accepts arguments for it's suit and number.</Constructor>
    ///     <methods>
    ///         ToString(): This override shows the object's suit and number values.
    ///     </methods>
    /// </summary>
    class Card
    {
        private string _suit;
        private int _number;

        public string Suit
        {
            get
            {
                return _suit;
            }
            set
            {
                _suit = value;
            }
        }
        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                if (value >= (int)Deck.Numbers.Two && value <= (int)Deck.Numbers.Ace)
                    _number = value;
                else
                    throw new ArgumentOutOfRangeException("Card Number out of range", "The number for this card is out of range");

            }
        }
        public Card(string suit, int num)
        {
            Suit = suit;
            Number = num;
        }
        public override string ToString()
        {
            return string.Format("{0} of {1}", (Deck.Numbers)Number, Suit);
        }
    }
}
