using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Models
{
    /// <summary>
    /// contains a hand (cardset of player one and two)
    /// </summary>
    public class Hand
    {
        public CardSet CardSetOne { get; set; } = new CardSet();
        public CardSet CardSetTwo { get; set; } = new CardSet();
    }
}
