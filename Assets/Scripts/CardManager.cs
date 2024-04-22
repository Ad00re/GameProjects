using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityAsync;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum Suit
{
    Clubs, Diamonds, Hearts, Spades
}

public enum Rank
{
    Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
}

public enum Hand{
    StraightFlush,FourOfAKind,FullHouse,Flush,Straight,ThreeOfAKind,TwoPair,Pair,HighCard,Nothing
}
public class Card
{
    public Suit Suit { get; private set; }
    public Rank Rank { get; private set; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }
}

public class CardManager : MonoBehaviour
{
    public List<Card> Cards;
    public List<int> Deck;
    public List<int> Drawed;
    public List<int> Play;
    public List<int> Selected;
    public List<int> IndexOfCardCountInPlay;
    public int scoringCardIndexInPlay=-1;
    
    public bool modified = false;

    [SerializeField] private GameObject cardPrefab;
    public int score;
    public int multi;
    public Hand hand;
    public int roundScore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiText;
    public TextMeshProUGUI handText;
    public TextMeshProUGUI roundScoreText;

    public static CardManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        Cards = new List<Card>();
        Deck = new List<int>();
        Drawed = new List<int>();
        Play = new List<int>();
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                Cards.Add(new Card(suit,rank));
            }
        }

        for (int i = 0; i < Cards.Count; i++)
        {
            Deck.Add(i);
        }

        score = 0;
        multi = 0;
        roundScore = 0;
        hand = Hand.Nothing;
        


    }

    // Update is called once per frame
    public List<GameObject> CardsViewPlay = new ();
    public List<GameObject> CardsViewHand = new ();
    void Update()
    {
        if (modified)
        {
            DisplayCardView(Drawed, new Vector3(-2, -2, 0), Selected,CardsViewHand);
            modified = false;
            DisplayCardView(Play,new Vector3(-2, 3, 0),new List<int>(),CardsViewPlay);
            for (int i = 0; i < Play.Count; i++)
            {
                CardsViewPlay[i].GetComponent<CardDisplay>().scoreText.text = scoringCardIndexInPlay==i?CalculationCardScore(Cards[Play[scoringCardIndexInPlay]]).Item1.ToString():"";
            }
            scoreText.text = score.ToString();
            multiText.text = multi.ToString();
            handText.text = (hand == Hand.Nothing)?"":hand.ToString();
           roundScoreText.text = roundScore.ToString();
        }
        
    }

    public void DrawCard()
    {
        Debug.Log(Deck.Count);
        Debug.Log(Drawed.Count);
        int numberOfDraw = 8 - Drawed.Count;
        for (int i = 0; i < numberOfDraw ; i++)
        {
            int indexOfCard = Random.Range(0, Deck.Count);
            Drawed.Add(Deck[indexOfCard]);
            Deck.RemoveAt(indexOfCard);
        }
        Drawed.Sort();
        MarkDirty();
    }

    public async void DiscardCard()
    {
        for(int i = Drawed.Count-1; i>-1;i--)
        {
            if (Selected.Contains(i))
            {
                Drawed.RemoveAt(i);
                Selected.Remove(i);
                MarkDirty();
                await UnityAsync.Await.Seconds(0.3f);
            }
        }
    }

    public async void PlayCard()
    {
        for(int i = Drawed.Count-1; i>-1;i--)
        {
            if (Selected.Contains(i))
            {
                Play.Insert(0,Drawed[i]);
                Drawed.RemoveAt(i);
                Selected.Remove(i);                
                MarkDirty();
            }
        }
        for (int i = 0; i < Play.Count; i++)
        {
            List<Card> playCards = Play.Select(j => Cards[j]).ToList();
            IndexOfCardCountInPlay = CalculateSelectedHand(playCards).Item2;
            if (IndexOfCardCountInPlay.Contains(i))
            {
                scoringCardIndexInPlay = i;
                (int, int) cardScore = CalculationCardScore(Cards[Play[i]]);
                score += cardScore.Item1;
                multi += cardScore.Item2;
            }
            MarkDirty();
            await UnityAsync.Await.Seconds(0.5f);
        }
        roundScore += score * multi;
        MarkDirty();
    }

    public (int, int) CalculationCardScore(Card card)
    {
        switch (card.Rank)
        {
            case Rank.Ace:
                return (11, 0);
            case Rank.Jack:
                
            case Rank.Queen:
                
            case Rank.King:
                return (10, 0);
            default:
                return ((int)card.Rank, 0);
        }
    }
    void DisplayCardView(List<int> CardIndex, Vector3 bottomLeft, List<int> SelectedIndex,List<GameObject> CardsView)
    {
        for(int i = 0;i<CardIndex.Count;i++)
        {
            if (i >= CardsView.Count)
            {
                GameObject card = Instantiate(cardPrefab);
                CardsView.Add(card);
            }
        }
        for (int i = 0; i < CardsView.Count; i++)
        {
            GameObject card = CardsView[i];
            if (i >= CardIndex.Count)
            {
                card.SetActive(false);
                continue;
            }
            card.transform.position= new Vector3(i,SelectedIndex.Contains(i)?2:0,0) + bottomLeft;
            var cardDisplay = card.GetComponent<CardDisplay>();
            cardDisplay.rankText.text = Cards[CardIndex[i]].Rank.ToString();
            cardDisplay.suitText.text = Cards[CardIndex[i]].Suit.ToString();
            card.SetActive(true);
            cardDisplay.CardIndexInHand = i;
        }
        
    }
    

    public void MarkDirty()
    {
        modified = true;
    }

    public (int, int) CalculateSelectedScore(Hand currentHand)
    {
        switch (currentHand)
        {
            case Hand.StraightFlush:
                return (100, 8);
            case Hand.FourOfAKind:
                return (60, 7);
            case Hand.FullHouse:
                return (40, 4);
            case Hand.Flush:
                return (35, 4);
            case Hand.Straight:
                return (30, 4);
            case Hand.ThreeOfAKind:
                return (30, 3);
            case Hand.TwoPair:
                return (20, 2);
            case Hand.Pair:
                return (10, 2);
            case Hand.HighCard:
                return (5, 1);
        }

        return (0, 0);
    }

    public (Hand,List<int>) CalculateSelectedHand(List<Card> selectedCards)
    {
        List<Card> sortedCards = selectedCards.OrderBy(card => card.Rank).ToList();
        Dictionary<Rank, int> valueCount =
            selectedCards.GroupBy(card => card.Rank).ToDictionary(grp => grp.Key, grp => grp.Count());
        List<int> Count = null;
        Hand hand = Hand.Nothing;
        if (sortedCards.Count == 5)
        {
            bool straight = isStraight(sortedCards);
            bool flush = isFlush(sortedCards);
            if (flush && straight)
            {
                Count = new List<int> { 0, 1, 2, 3, 4 };
                hand =  Hand.StraightFlush;
            }

            else if (flush)
            {
                Count = new List<int> { 0, 1, 2, 3, 4 };
                hand= Hand.Flush;
            }

            else if (straight)
            {
                Count = new List<int> { 0, 1, 2, 3, 4 };
                hand= Hand.Straight;
            }

            if (hand != Hand.Nothing)
            {
                return (hand, Count);
            }
        }

        if (valueCount.ContainsValue(4))
        {
            List<Rank> rankList = new List<Rank> { valueCount.FirstOrDefault(x => x.Value == 4).Key };
            Count = UpdateCountByRank(selectedCards,rankList);
            hand= Hand.FourOfAKind;
        }

        else if (valueCount.ContainsValue(3))
        {
            if (valueCount.ContainsValue(2))
            {
                Count = new List<int> { 0, 1, 2, 3, 4 };
                hand= Hand.FullHouse;
            }
            else
            {
                List<Rank> rankList = new List<Rank> { valueCount.FirstOrDefault(x => x.Value == 3).Key };
                Count = UpdateCountByRank(selectedCards,rankList);
                hand= Hand.ThreeOfAKind;
            }
           
        }

        else if (valueCount.Count(v => v.Value == 2) == 2)
        {
            List<Rank> rankList = new List<Rank>();
            foreach (KeyValuePair<Rank, int> pair in valueCount)
            {
                if (pair.Value == 2)
                {
                    rankList.Add(pair.Key);
                }
            }
            Count = UpdateCountByRank(selectedCards,rankList);
            hand= Hand.TwoPair;
        }

        else if (valueCount.ContainsValue(2))
        {
            List<Rank> rankList = new List<Rank> { valueCount.FirstOrDefault(x => x.Value == 2).Key };
            Count = UpdateCountByRank(selectedCards,rankList);
            hand= Hand.Pair;
        }
        else if (valueCount.ContainsValue(1))
        {
            if (valueCount.ContainsKey(Rank.Ace))
            {
                Count = UpdateCountByRank(selectedCards,new List<Rank>{Rank.Ace});
            }
            else
            {
                Count = UpdateCountByRank(selectedCards,new List<Rank>{valueCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key});
            }
            hand= Hand.HighCard;
        }

        return (hand, Count);
    }

    public List<int> UpdateCountByRank(List<Card> selectedCards, List<Rank> rankList)
    {
        List<int> Count = new List<int>();
        for (int i = 0; i < selectedCards.Count; i++)
        {
            if (rankList.Contains(selectedCards[i].Rank))
            {
                Count.Add(i);
            }
        }

        return Count;
    }
    
    bool isStraight(List<Card> sortedCards)
    {
        if (sortedCards[0].Rank == Rank.Ace && sortedCards[1].Rank == Rank.Ten)
        {
            for (int i = 1; i < sortedCards.Count - 1; i++)
            {
                if (sortedCards[i + 1].Rank != sortedCards[i].Rank + 1)
                    return false;
            }

            return true;
        }
        for (int i = 0; i < sortedCards.Count - 1; i++)
        {
            if (sortedCards[i + 1].Rank != sortedCards[i].Rank + 1)
                return false;
        }
        return true;
    }
    bool isFlush(List<Card> sortedCards)
    {
        return sortedCards.All(card => card.Suit == sortedCards[0].Suit);
    }

    
}


