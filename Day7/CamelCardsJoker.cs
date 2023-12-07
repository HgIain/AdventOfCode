using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7
{
    public class CamelCardsJoker(string filename)
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
            '_' => 0,
            >= '2' and <= '9' => card - '2' + 1,
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => throw new Exception("Unexpected card")
        };

        private static HandType GetHandType(string cards)
        {
            if(cards.Length != 5)
            {
                throw new Exception("Invalid hand");
            }

            var analysis = new Dictionary<char, int>();

            int jokerCount = 0;

            foreach (var card in cards)
            {
                if(card == '_')
                {
                    jokerCount++;
                }

                if (analysis.TryGetValue(card, out int count))
                {
                    analysis[card] = ++count;
                }
                else
                {
                    analysis[card] = 1;
                }
            }

            if(jokerCount == 5)
            {
                // pathological case
                return HandType.FiveOfAKind;
            }

            if (jokerCount > 0)
            {
                analysis.Remove('_');

                // loop over the dictionary to find the most common card
                var keyOfMaxValue = analysis.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                // add the jokers to the most common card
                // this will give us the best possible hand
                analysis[keyOfMaxValue] += jokerCount;
            }

            switch(analysis.Count)
            {
                case 1:
                    return HandType.FiveOfAKind;
                case 2:
                    if (analysis.Values.Any(v => v == 4))
                    {
                        return HandType.FourOfAKind;
                    }
                    else
                    {
                        return HandType.FullHouse;
                    }
                case 3:
                    if(analysis.Values.Any(v => v == 3))
                    {
                        return HandType.ThreeOfAKind;
                    }
                    else
                    {
                        return HandType.TwoPairs;
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

        public int Process(bool jokersWild = true)
        {
            var text = File.ReadAllLines(filename);

            List<Hand> hands = [];

            foreach (var line in text)
            {
                var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);

                if (split.Length != 2)
                {
                    throw new Exception("Invalid line");
                }

                var cards = split[0];

                if(jokersWild)
                {
                    // replace all J with _ to make jokers wild
                    // this allows us to use the same code for both parts
                    cards = cards.Replace('J', '_');
                }

                var hand = new Hand(cards, int.Parse(split[1]), GetHandType(cards));
                hands.Add(hand);
            }

            hands.Sort((a, b) => a.CompareTo(b));

            int total = hands.Select((hand, index) => hand.score * (index + 1)).Sum();

            Console.WriteLine($"Total: {total}");

            return total;
        }

    }
}
