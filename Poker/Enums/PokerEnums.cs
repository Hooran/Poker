using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Enums
{
    public class PokerEnums
    {
        public enum Rank
        {
            HighCard = 1,
            Pair = 2,
            TwoPairs = 3,
            ThreeOfKind = 4,
            Straight = 5,
            Flush = 6,
            FullHouse = 7,
            FourOfKind = 8,
            StraightFlush = 9,
            RoyalFlush = 10
        }

        public enum Player
        {
            PlayerOne = 1,
            PlayerTwo = 2
        }
    }
}
