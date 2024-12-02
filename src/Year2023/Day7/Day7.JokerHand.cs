using System.Diagnostics;
using Utilities.Extensions;

namespace Year2023;

public partial class Day7
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    private class JokerHand : IComparable<JokerHand>, IBiddable
    {
        private const string CardStrengths = "AKQT98765432J";

        public JokerHand(string cards, int bid)
        {
            _cards = cards;
            Bid = bid;

            var groups = cards.ToFrequencies();

            if (cards == "JJJJJ")
            {
                Type = HandType.FiveOfKind;
                return;
            }

            if (groups.TryGetValue('J', out var jokerCount))
            {
                groups.Remove('J');
                var highestGroup = groups
                    .OrderByDescending(_ => _.Value)
                    .First();
                groups[highestGroup.Key] = highestGroup.Value + jokerCount;
            }

            Type = groups.Values
                .OrderDescending()
                .ToArray() switch
                {
                    [ 5 ] => HandType.FiveOfKind,
                    [ 4, 1 ] => HandType.FourOfKind,
                    [ 3, 2 ] => HandType.FullHouse,
                    [ 3, 1, 1 ] => HandType.ThreeOfKind,
                    [ 2, 2, 1 ] => HandType.TwoPair,
                    [ 2, 1, 1, 1 ] => HandType.OnePair,
                    [ 1, 1, 1, 1, 1 ] => HandType.HighCard,
                    _ => throw new Exception("Invalid hand")
                };
        }

        protected readonly string _cards;
        public HandType Type { get; }
        public int Bid { get; }
        private string DebuggerDisplay => $"{_cards} {Type} {Bid}";

        public int CompareTo(JokerHand? other)
        {
            if (other is null)
            {
                return -1;
            }

            var handDifference = (int)Type - (int)other.Type;

            if (handDifference != 0)
            {
                return handDifference;
            }

            var firstDifferentCard = _cards
                .Zip(other._cards)
                .SkipWhile(t => t.First == t.Second)
                .FirstOrDefault();

            if (firstDifferentCard == default)
            {
                return 0;
            }

            var myCardStrength = CardStrengths.IndexOf(firstDifferentCard.First);
            var otherCardStrength = CardStrengths.IndexOf(firstDifferentCard.Second);

            if (myCardStrength == -1 || otherCardStrength == -1)
            {
                throw new Exception("Invalid card");
            }

            return otherCardStrength - myCardStrength;
        }
    }
}
