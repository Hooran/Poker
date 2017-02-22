using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Models
{
    /// <summary>
    /// contains value and suit of a card
    /// </summary>
    public class Card
    {
        public byte Value { get; set; }
        public byte Suit { get; set; }
    }
}
