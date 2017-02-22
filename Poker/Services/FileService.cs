using Poker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Poker.Enums;

namespace Poker.Services
{
    public class FileService
    {
        protected List<string> lines;
        protected List<Hand> hands;
        protected CardSet cardSet;
        protected List<string> cards;

        /// <summary>
        /// read hands from text file
        /// </summary>
        /// <param name="_string">the path of text file</param>
        /// <returns>hands list</returns>
        public List<Hand> GetHands(string _string)
        {
            try
            {
                hands = new List<Hand>();
                lines = File.ReadLines(_string).ToList();

                // iterate through file lines (hands)
                lines.ForEach(x => hands.Add(new Hand() {
                    CardSetOne = ConvertToCardSet(x, PokerEnums.Player.PlayerOne),
                    CardSetTwo = ConvertToCardSet(x, PokerEnums.Player.PlayerTwo)}));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return hands;
        }

        public List<Hand> GetHandsFromConsoleInput(string _line)
        {
            try
            {
                hands = new List<Hand>();
                lines = _line.Split('\n').ToList();

                // iterate through file lines (hands)
                lines.ForEach(x => hands.Add(new Hand()
                {
                    CardSetOne = ConvertToCardSet(x, PokerEnums.Player.PlayerOne),
                    CardSetTwo = ConvertToCardSet(x, PokerEnums.Player.PlayerTwo)
                }));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return hands;
        }

        /// <summary>
        /// convert a hand to cardset
        /// </summary>
        /// <param name="_line">line of string containing hands</param>
        /// <returns>cardset</returns>
        public CardSet ConvertToCardSet(string _line, PokerEnums.Player _player)
        {
            cardSet = new CardSet();
            cards = (_player == PokerEnums.Player.PlayerOne) ? _line.Split(' ').Take(5).ToList() : _line.Split(' ').Skip(5).ToList();

            cards.ForEach(x => cardSet.Cards.Add(new Card()
                        {
                            Value = Convert.ToByte(ReplaceValues(x.Substring(0, 1))),
                            Suit = Convert.ToByte(ReplaceValues(x.Substring(1, 1)))
                        }));

            return cardSet;
        }

        /// <summary>
        /// replaces values and suits with numbers
        /// </summary>
        /// <param name="line"></param>
        /// <returns>line replaced with numbers</returns>
        protected string ReplaceValues(string line)
        {
                // for values
            return line.Replace("J", "11")
                .Replace("T", "10")
                .Replace("J", "11")
                .Replace("Q", "12")
                .Replace("K", "13")
                .Replace("A", "14")
                // for suits
                .Replace("D", "1")
                .Replace("H", "2")
                .Replace("S", "3")
                .Replace("C", "4");
        }

        /// <summary>
        /// get a line from file
        /// </summary>
        /// <returns></returns>
        public string GetLine(string _filePath, int _lineNumber)
        {
            return File.ReadLines(_filePath).ToList().Skip(_lineNumber - 1).FirstOrDefault();
        }
    }
}
