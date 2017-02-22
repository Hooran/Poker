using Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Enums;

namespace Poker.Services
{
    public class PokerService
    {
        #region Variables
        protected FileService fileService = new FileService();
        protected List<RankResult> rankresults;
        protected List<CardCount> cardValueCounts;
        protected List<CardCount> cardSuitCounts;
        protected RankResult rankResultPlayerOne;
        protected RankResult rankResultPlayerTwo;
        protected int winPlayerOne;
        protected int winPlayerTwo;
        protected int draw;
        protected bool isConsecutive;
        protected bool isFlush;
        protected int counter;
        #endregion

        public string Compare(List<Hand> _hands, bool _fromFile)
        {
            winPlayerOne = winPlayerTwo = 0;
            counter = 0;

            foreach (Hand _hand in _hands)
            {
                rankResultPlayerOne = GetRank(_hand.CardSetOne);
                rankResultPlayerTwo = GetRank(_hand.CardSetTwo);

                if (rankResultPlayerOne.Rank > rankResultPlayerTwo.Rank)
                {
                    // if player one has higher rank
                    winPlayerOne++;
                    if (_fromFile) WriteLine(counter, PokerEnums.Player.PlayerOne.ToString(), ((PokerEnums.Rank)rankResultPlayerOne.Rank).ToString());
                }
                else if (rankResultPlayerOne.Rank < rankResultPlayerTwo.Rank)
                {
                    // if player two has higher rank
                    winPlayerTwo++;
                    if (_fromFile) WriteLine(counter, PokerEnums.Player.PlayerTwo.ToString(), ((PokerEnums.Rank)rankResultPlayerTwo.Rank).ToString());
                }
                else if (rankResultPlayerOne.Rank == rankResultPlayerTwo.Rank)
                {
                    // high card
                    if (rankResultPlayerOne.Value > rankResultPlayerTwo.Value)
                    {
                        winPlayerOne++;
                        if (_fromFile) WriteLine(counter, PokerEnums.Player.PlayerOne.ToString(), ((PokerEnums.Rank)rankResultPlayerOne.Rank).ToString());
                    }
                    else if (rankResultPlayerOne.Value < rankResultPlayerTwo.Value)
                    {
                        winPlayerTwo++;
                        if (_fromFile) WriteLine(counter, PokerEnums.Player.PlayerTwo.ToString(), ((PokerEnums.Rank)rankResultPlayerTwo.Rank).ToString());
                    }
                    else {
                        draw++;
                        if (_fromFile) WriteLine(counter, "Draw", ((PokerEnums.Rank)rankResultPlayerOne.Rank).ToString());
                    }
                }

                counter++;
            }

            Console.WriteLine("-----------------------------------\n");

            return "Player 1 : " + winPlayerOne + "\n" +
                "Player 2 : " + winPlayerTwo + "\n" + 
                "Draw : " + draw;
        }

        /// <summary>
        /// compare cards and return highest rank
        /// </summary>
        /// <param name="_cardSet">cardset including player's cardset and current hand</param>
        /// <returns>highest rank</returns>
        public RankResult GetRank(CardSet _cardSet)
        {
            rankresults = new List<RankResult>();
            cardValueCounts = new List<CardCount>();
            cardSuitCounts = new List<CardCount>();

            #region Counts
            // number of values
            cardValueCounts = _cardSet.Cards
                .GroupBy(x => x.Value)
                .Select(y => new CardCount()
                {
                    Item = Convert.ToByte(y.Key),
                    Count = y.Count()
                }).ToList();

            // number of suits
            cardSuitCounts = _cardSet.Cards
                .GroupBy(x => x.Suit)
                .Select(y => new CardCount()
                {
                    Item = Convert.ToByte(y.Key),
                    Count = y.Count()
                }).ToList();
            #endregion

            isConsecutive = !_cardSet.Cards.Select(x => x.Value).Select((i, j) => i - j).Distinct().Skip(1).Any();
            isFlush = cardSuitCounts.Any(x => x.Count == 5);

            #region High Card
            if (!cardValueCounts.Any(x => x.Count > 1))
            {
                rankresults.Add(new RankResult()
                {
                    Rank = Enums.PokerEnums.Rank.HighCard.GetHashCode(),
                    Value = cardValueCounts
                        .Where(x => x.Count == 1)
                        .Max(y => y.Item)
                });
            }
            #endregion

            #region Pairs
            rankresults.AddRange(cardValueCounts
                .Where(x => x.Count == 2)
                .Select(y => new RankResult() {
                    Rank = Enums.PokerEnums.Rank.Pair.GetHashCode(),
                    Value = y.Item })
                .ToList());
            #endregion

            #region Two Pairs
            // two pairs
            if (cardValueCounts.Where(x => x.Count == 2).Count() > 1)
            {
                rankresults.Add(new RankResult() {
                    Rank = PokerEnums.Rank.TwoPairs.GetHashCode(),
                    Value = cardValueCounts.Where(x => x.Count == 2).Max(y => y.Item)
                });
            }
            #endregion

            #region Three of Kind
            if (cardValueCounts.Any(x => x.Count == 3))
            {
                rankresults.AddRange(cardValueCounts.Where(x => x.Count == 3).Select(x => new RankResult()
                {
                    Rank = PokerEnums.Rank.ThreeOfKind.GetHashCode(),
                    Value = x.Item
                }));
            }
            #endregion

            #region Straight
            //straight
            if (isConsecutive)
            {
                rankresults.Add(new RankResult()
                {
                    Rank = PokerEnums.Rank.Straight.GetHashCode(),
                    Value = _cardSet.Cards.Max(x => x.Value)
                });
            }
            #endregion

            #region Flush
            if (isFlush)
            {
                rankresults.Add(new RankResult() {
                    Rank = PokerEnums.Rank.Flush.GetHashCode(),
                    Value = _cardSet.Cards.Max(x => x.Value)
                });
            }
            #endregion

            #region Full House
            if (rankresults.Any(x => x.Rank == PokerEnums.Rank.ThreeOfKind.GetHashCode() && rankresults.Any(y => y.Rank == PokerEnums.Rank.Pair.GetHashCode())))
            {
                rankresults.Add(new RankResult()
                {
                    Rank = PokerEnums.Rank.FullHouse.GetHashCode(),
                    Value = _cardSet.Cards.Max(x => x.Value)

                });
            }
            #endregion

            #region Four of Kind
            if (cardValueCounts.Any(x => x.Count == 4))
            {
                rankresults.AddRange(cardValueCounts.Where(x => x.Count == 4).Select(x => new RankResult()
                {
                    Rank = PokerEnums.Rank.FourOfKind.GetHashCode(),
                    Value = x.Item
                }));
            }
            #endregion

            #region Straight Flush
            if (isConsecutive && isFlush)
            {
                rankresults.Add(new RankResult()
                {
                    Rank = PokerEnums.Rank.StraightFlush.GetHashCode(),
                    Value = _cardSet.Cards.Max(x => x.Value)

                });
            }
            #endregion

            #region Royal Flush
            if (isConsecutive && _cardSet.Cards.Max(x => x.Value == 14) && isFlush)
            {
                rankresults.Add(new RankResult()
                {
                    Rank = PokerEnums.Rank.RoyalFlush.GetHashCode(),
                    Value = _cardSet.Cards.Max(x => x.Value)

                });
            }
            #endregion

            return rankresults.OrderByDescending(x => x.Rank).FirstOrDefault();
        }

        public void WriteLine(int _hand, string _winner, string _rank)
        {
            Console.WriteLine(fileService.GetLine(Constants.FilePath, counter) + " : " + _winner + " [" + _rank  + "]");
        }
    }
}
