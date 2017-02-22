using System;
using System.Collections.Generic;
using Poker.Services;
using Poker.Models;

namespace Poker
{
    class Program
    {
        static void Main(string[] args)
        {
            // convert hands from the file or input
            List<Hand> hands;

            FileService fileService = new FileService();
            PokerService pokerService = new PokerService();

            // from file input
            //hands = fileService.GetHands(Constants.FilePath);
            //Console.WriteLine(pokerService.Compare(hands, true) + "\n");
            //Console.ReadLine();

            // from console input
            while (true)
            {
                Console.WriteLine("Enter poker hands:");
                string inputHands = Console.ReadLine();

                hands = fileService.GetHandsFromConsoleInput(inputHands);

                Console.WriteLine(pokerService.Compare(hands, false) + "\n");
            }
        }
    }
}
