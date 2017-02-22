using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Models
{
    /// <summary>
    /// contains collection of cards (hand)
    /// </summary>
    public class CardSet
    {
        public List<Card> Cards { get; set; } = new List<Card>();
    }
}
