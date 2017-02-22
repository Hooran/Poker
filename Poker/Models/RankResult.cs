using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Models
{
    /// <summary>
    /// a rank and its winning card value (highest value)
    /// </summary>
    public class RankResult
    {
        public int Rank { get; set; }
        public byte Value { get; set; }
    }
}
