using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7
{
    public class CamelCards(string filename)
    {
        private enum HandType
        {
            HighCard,
            OnePair,
            TwoPairs,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind,
        }

        static private int GetValueForCard(char card) => card switch
        {
            >= '2' and <= '9' => card - '2',
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => throw new Exception("Unexpected card")
        };

        private static HandType GetHandType(string cards)
        {
            var cardArray = cards.OrderBy(c => c).ToList();

            var analysis = new Dictionary<char, int>();

            foreach(var card in cardArray)
            {
                if (analysis.ContainsKey(card))
                {
                    analysis[card]++;
                }
                else
                {
                    analysis[card] = 1;
                }
            }

            switch(analysis.Count)
            {
                case 1:
                    return HandType.FiveOfAKind;
                case 2:
                    if(analysis.Values.Any(v => v == 4))
                    {
                        return HandType.FourOfAKind;
                    }
                    else if(analysis.Values.Any(v => v == 3))
                    {
                        return HandType.FullHouse;
                    }
                    else
                    {
                        throw new Exception("Invalid hand");
                    }
                case 3:
                    if(analysis.Values.Any(v => v == 3))
                    {
                        return HandType.ThreeOfAKind;
                    }
                    else if(analysis.Values.Any(v => v == 2))
                    {
                        return HandType.TwoPairs;
                    }
                    else
                    {
                        throw new Exception("Invalid hand");
                    }
                case 4:
                    return HandType.OnePair;
                case 5:
                    return HandType.HighCard;
                default:
                    throw new Exception("Invalid hand");
            }
        }

        static int CompareCards(string cards1, string cards2)
        {
            if(cards1.Length != cards2.Length)
            {
                throw new Exception("Invalid hand");
            }

            for(int i = 0; i < cards1.Length; i++)
            {
                var value1 = GetValueForCard(cards1[i]);
                var value2 = GetValueForCard(cards2[i]);

                if(value1 != value2)
                {
                    return value1.CompareTo(value2);
                }
            }

            return 0;
        }


        private record Hand(string cards, int score, HandType handType) : IComparable<Hand>
        {
            public int CompareTo(Hand? other)
            {
                if (other == null)
                {
                    return 1;
                }
                if (handType != other.handType)
                {
                    return handType.CompareTo(other.handType);
                }

                return CompareCards(cards, other.cards);
            }
        }

        private int CompareHands(Hand hand1, Hand hand2)
        {
            return 0;
        }

        private readonly List<Hand> hands = [];

        public int Process()
        {
            var text = File.ReadAllLines(filename);

            int total = 0;

            foreach (var line in text)
            {
                var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);

                if (split.Length != 2)
                {
                    throw new Exception("Invalid line");
                }

                var hand = new Hand(split[0], int.Parse(split[1]), GetHandType(split[0]));
                hands.Add(hand);
            }

            hands.Sort((a, b) => a.CompareTo(b));

            for(int i = 0; i < hands.Count; i++)
            {
                total += hands[i].score * (i + 1);
            }

            Console.WriteLine($"Total: {total}");

            return total;
        }

    }
}
