using System;
using static System.Console;

namespace Poker_Game
{
    /// <summary>
    ///     <brief>
    ///         This program uses two classes to assemble a deck of cards.
    ///     </brief>
    ///     <consts>
    ///         <note>
    ///             const int EXIT: a value that represents the value required to exit the discard loop.
    ///             
    ///             consr inr CARD_LIMIT
    ///         </note>
    ///     </consts>
    ///     <methods>
    ///         Main(): Manages two functions: Game() and Test()
    ///         Game(): plays the poker game. Manipulates rounds, bankroll and the hand
    ///         Test(): Shows all win conditions of the game, as well as how the bankroll is manipulated
    ///         Printerror/success: prints out colored text, red for errors and green for success
    ///         PrintMoney()/PrintWinnings(): Displays money in bank/money earned
    ///         GetPositiveInteger/Double(): gets a positive integer/double as an input.
    ///     </methods>
    ///     <Testers>
    ///         1.Ethan Briffet
    ///         2.Phil Aube
    ///     </Testers>
    /// </summary>
    class Program
    {
        const int EXIT = 0;
        const int CARD_LIMIT = 4;
        const int CHOICE_LIMIT = 2;
        const int TEST_LIMIT = 9;
        static void Main(string[] args)
        {
            int choice;

            WriteLine("1: Play");
            WriteLine("2: Test");
            WriteLine("Press 0 to quit");

            Write("\nPlease enter your desired option:");

            choice = GetPositiveInteger();
            while (choice > CHOICE_LIMIT || choice < 0)
            {
                PrintError("Please pick one of the options based on corresponding integer");
                choice = GetPositiveInteger();
            }

            switch (choice)
            {
                case 1: Game();
                    break;
                case 2: Test();
                    break;
                default:
                    break;
            }

            //Game();
        
        
            WriteLine("Thank you for playing!");
            ReadKey();
        }

        private static void Game()
        {
            Deck d = new Deck();
            int input;
            Card results;
            int count;
            double money;
            double startBank;
            string conf;
            bool quit = false;

            while (!quit)
            {

                count = 0;
                
                d.Shuffle();

                WriteLine("Please enter the amount you wish to bet: {0} ... {1}", 1, d.Bankroll);


                money = GetPositiveDouble();

                while (money < 1 || money > d.Bankroll)
                {
                    PrintError(string.Format("Please enter a number of at least 1 and less than or equal to {0}", d.Bankroll));
                    money = GetPositiveDouble();
                }

                d.BetMoney(money);

                startBank = d.Bankroll;

                PrintMoney(d);

                d.Deal();

                WriteLine(d.ToString());

                WriteLine("Please enter a number corresponding to the card in your hand, press 0 to stop");

                input = GetPositiveInteger();

                while (input > EXIT && count < CARD_LIMIT)
                {
                    if (count < CARD_LIMIT && input - 1 <= (CARD_LIMIT))
                    {
                        results = d.Hand[input - 1];
                        d.Discard(input);
                        if (results == d.Hand[input - 1])//should I create an operator for this?
                        {
                            PrintError("Error: You have already discarded this card!");
                        }
                        else
                        {
                            count++;
                            PrintSuccess("Card succssfully replaced!");
                        }

                    }
                    else
                    {
                        PrintError("Error: Please choose a number pertaining to the cards in your hand!");
                    }

                    if (count < CARD_LIMIT)
                    {
                        input = GetPositiveInteger();
                    }
                    else if (count >= CARD_LIMIT)
                    {
                        WriteLine("Finished");

                    }
                    else
                        PrintError("Error: Please choose a number pertaining to the cards in your hand!");
                }

                d.Redraw();

                WriteLine(d.ToString());

                d.Payout();
                if (d.Bankroll > startBank)
                    PrintWinnings(d, startBank);
                PrintMoney(d);

                d.PutBack();

                if (d.Bankroll > 0)
                {
                    WriteLine("Would you like to play again? Press enter to continue. Press 'N' to quit.");
                    conf = ReadLine();
                    if (conf.ToUpper() == "N")
                        quit = true;
                    else
                        quit = false;
                }
                else
                    quit = true;
            }
        }

        private static void Test()
        {
            int choice = 1;
            Deck d = new Deck();
            d.Deal();
            double money;
            double startBank;

            while (choice != 0)
            {
                WriteLine("1: Test Royal Flush");
                WriteLine("2: Test Straight Flush");
                WriteLine("3: Test Four-of-a-kind");
                WriteLine("4: Test Full House");
                WriteLine("5: Test Flush");
                WriteLine("6: Test Straight");
                WriteLine("7: Test Three-of-a-kind");
                WriteLine("8: Test 2 Pair");
                WriteLine("9: Test Pair");
                WriteLine("Press 0 to quit");

                Write("\nPlease enter your desired option:");
                choice = GetPositiveInteger();
                while (choice > TEST_LIMIT || choice < 0)
                {
                    PrintError("Please pick one of the options based on corresponding integer");
                    choice = GetPositiveInteger();
                }

                if (choice != 0)
                {

                    

                    WriteLine("Please enter the amount you wish to bet: {0} ... {1}", 1, d.Bankroll);
                    money = GetPositiveDouble();

                    while (money < 1 || money > d.Bankroll)
                    {
                        PrintError(string.Format("Please enter a number of at least 1 and less than or equal to {0}", d.Bankroll));
                        money = GetPositiveDouble();
                    }

                    d.BetMoney(money);
                    startBank = d.Bankroll;
                    switch (choice)
                    {
                        case 1:
                            testRF(d);
                            break;
                        case 2:
                            testSF(d);
                            break;
                        case 3:
                            test4k(d);
                            break;
                        case 4:
                            testFH(d);
                            break;
                        case 5:
                            testF(d);
                            break;
                        case 6:
                            testS(d);
                            break;
                        case 7:
                            test3k(d);
                            d.Payout();
                            break;
                        case 8:
                            test2P(d);
                            break;
                        case 9:
                            testP(d);
                            break;
                        default:
                            break;
                    }
                    WriteLine("\n" + d.ToString());
                    d.Payout();
                    PrintSuccess(string.Format("Money won: ${0}", d.Bankroll - startBank));
                    PrintSuccess(string.Format("Money in bank: ${0}\n", d.Bankroll));
                }
            }
            
        }

        private static void testRF(Deck d)
        {
            d.Hand[0].Number = 8;
            d.Hand[1].Number = 9;
            d.Hand[2].Number = 10;
            d.Hand[3].Number = 11;
            d.Hand[4].Number = 12;

            d.Hand[0].Suit = "Hearts";
            d.Hand[1].Suit = "Hearts";
            d.Hand[2].Suit = "Hearts";
            d.Hand[3].Suit = "Hearts";
            d.Hand[4].Suit = "Hearts";
        }
        private static void testSF(Deck d)
        {
            d.Hand[0].Number = 0;
            d.Hand[1].Number = 1;
            d.Hand[2].Number = 2;
            d.Hand[3].Number = 3;
            d.Hand[4].Number = 4;

            d.Hand[0].Suit = "Clubs";
            d.Hand[1].Suit = "Clubs";
            d.Hand[2].Suit = "Clubs";
            d.Hand[3].Suit = "Clubs";
            d.Hand[4].Suit = "Clubs";
        }
        private static void test4k(Deck d)
        {
            d.Hand[0].Number = 0;
            d.Hand[1].Number = 0;
            d.Hand[2].Number = 0;
            d.Hand[3].Number = 0;
        }
        private static void testFH(Deck d)
        {
            d.Hand[0].Number = 0;
            d.Hand[1].Number = 0;
            d.Hand[2].Number = 1;
            d.Hand[3].Number = 1;
            d.Hand[4].Number = 1;
        }
        private static void testF(Deck d)
        {
            d.Hand[0].Suit = "Clubs";
            d.Hand[1].Suit = "Clubs";
            d.Hand[2].Suit = "Clubs";
            d.Hand[3].Suit = "Clubs";
            d.Hand[4].Suit = "Clubs";
            d.Hand[0].Number = 4;
        }
        private static void testS(Deck d)
        {
            d.Hand[0].Number = 0;
            d.Hand[1].Number = 1;
            d.Hand[2].Number = 2;
            d.Hand[3].Number = 3;
            d.Hand[4].Number = 4;
            d.Hand[0].Suit = "Spades";
        }
        private static void test3k(Deck d)
        {
            d.Hand[0].Number = 10;
            d.Hand[1].Number = 3;
            d.Hand[2].Number = 1;
            d.Hand[3].Number = 1;
            d.Hand[4].Number = 1;
            d.Hand[0].Suit = "Spades";
        }
        private static void test2P(Deck d)
        {
            d.Hand[0].Number = 0;
            d.Hand[1].Number = 0;
            d.Hand[2].Number = 2;
            d.Hand[3].Number = 1;
            d.Hand[4].Number = 1;

            d.Hand[0].Suit = "Spades";
        }
        private static void testP(Deck d)
        {
            d.Hand[0].Number = 10;
            d.Hand[1].Number = 10;
            d.Hand[2].Number = 1;
            d.Hand[3].Number = 3;
            d.Hand[4].Number = 2;

            d.Hand[0].Suit = "Spades";
        }
        private static int GetPositiveInteger()
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input) || input < 0)
            {
                PrintError("Error! Please enter a positive number: ");
            }

            return input;
        }
        private static double GetPositiveDouble()
        {
            double input;
            while (!double.TryParse(Console.ReadLine(), out input) || input < 0)
            {
                PrintError("Error! Please enter a positive number: ");
            }

            return input;
        }
        public static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        public static void PrintSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        private static void PrintMoney(Deck d)
        {
            WriteLine(string.Format("Money in bank: ${0}", d.Bankroll));
        }
        private static void PrintWinnings(Deck d, double startBank)
        {
            WriteLine("Money won: ${0}", d.Bankroll - startBank);
        }
    }
}
