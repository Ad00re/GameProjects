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
    public bool sortByRank = true;

    [SerializeField] private GameObject cardPrefab;
    public int score;
    public int multi;
    public Hand hand;
    public int roundScore;
    public int roundTarget;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiText;
    public TextMeshProUGUI handText;
    public TextMeshProUGUI roundScoreText;
    public TextMeshProUGUI roundTargetText;
    

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

    void Start()
    {
        Cards = new List<Card>();
        
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                Cards.Add(new Card(suit,rank));
            }
        }

        SetDefault();
        
    }

    public void SetDefault()
    {
        Deck = new List<int>();
        Drawed = new List<int>();
        Play = new List<int>();
        for (int i = 0; i < Cards.Count; i++)
        {
            Deck.Add(i);
        }
        score = 0;
        multi = 0;
        roundScore = 0;
        hand = Hand.Nothing;
        roundTarget = StateManager.Instance.roundTarget;
        MarkDirty();
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
            //change the view of play
            List<int> sortedIndexOfCard;
            DisplayCardView(Play,new Vector3(-2, 3, 0),new List<int>(),CardsViewPlay);
            if (!sortByRank)
            {
                sortedIndexOfCard = Enumerable.Range(0, Play.Count).ToList();
            }
            else
            {
                sortedIndexOfCard = MapIndex(Play);
            }
            for (int i = 0; i < Play.Count; i++)
            {
                int indexOfCard = sortedIndexOfCard[i];
                CardsViewPlay[i].GetComponent<CardDisplay>().scoreText.text = scoringCardIndexInPlay==indexOfCard?CalculationCardScore(Cards[Play[scoringCardIndexInPlay]]).Item1.ToString():"";
            }
            scoreText.text = score.ToString();
            multiText.text = multi.ToString();
            handText.text = (hand == Hand.Nothing)?"":hand.ToString();
            roundScoreText.text = roundScore.ToString();
            roundTargetText.text = roundTarget.ToString();
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
        StateManager.Instance.discard -= 1;
        StateManager.Instance.MarkDirty();
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
        StateManager.Instance.hand -= 1;
        StateManager.Instance.MarkDirty();
        //move selected card to play
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
        List<int> sortedIndexOfCard;
        if (!sortByRank)
        {
            sortedIndexOfCard = Enumerable.Range(0, Play.Count).ToList();
        }
        else
        {
            sortedIndexOfCard = MapIndex(Play);
        }
        //calculate score that count for play
        for (int i = 0; i < Play.Count; i++)
        {
            int indexOfCard = sortedIndexOfCard[i];
            List<Card> playCards = Play.Select(j => Cards[j]).ToList();
            IndexOfCardCountInPlay = CalculateSelectedHand(playCards).Item2;
            if (IndexOfCardCountInPlay.Contains(indexOfCard))
            {
                scoringCardIndexInPlay = indexOfCard;
                (int, int) cardScore = CalculationCardScore(Cards[Play[indexOfCard]]);
                score += cardScore.Item1;
                multi += cardScore.Item2;
            }
            MarkDirty();
            await UnityAsync.Await.Seconds(0.5f);
        }

        scoringCardIndexInPlay = -1;
        //change round score
        roundScore += score * multi;
        MarkDirty();
        //remove card in play 
        await UnityAsync.Await.Seconds(2f);
        for(int i = Play.Count-1; i>-1;i--)
        {
            Play.RemoveAt(i);
            Selected.Remove(i);                
            MarkDirty();
            await UnityAsync.Await.Seconds(0.3f);
        }
        score =0;
        multi =0;
        hand = Hand.Nothing;
        if (roundScore >= roundTarget)
        {
            StateManager.Instance.gameState = StateManager.GameState.cash;
            StateManager.Instance.MarkDirty();
        }
    }

    public void ButtonSortByRank()
    {
        sortByRank = true; 
        MarkDirty();
    }
    public void ButtonSortBySuit()
    {
        sortByRank = false;
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
        //add enough card instance
        for(int i = 0;i<CardIndex.Count;i++)
        {
            if (i >= CardsView.Count)
            {
                GameObject card = Instantiate(cardPrefab,StateManager.Instance.gameView.transform);
                CardsView.Add(card);
            }
        }
        for (int i = 0; i < CardsView.Count; i++)
        {
            
            GameObject card = CardsView[i];
            if (i >= CardIndex.Count)
            {
                card.SetActive(false);
            }
        }
        List<int> sortedIndexOfCard;
        if (!sortByRank)
        {
            sortedIndexOfCard = Enumerable.Range(0, CardIndex.Count).ToList();
        }
        else
        {
            sortedIndexOfCard = MapIndex(CardIndex);
        }
        
        for (int i = 0; i < CardIndex.Count; i++)
        {
            int indexInDrawed = sortedIndexOfCard[i];
            GameObject card = CardsView[i];
            card.transform.position= new Vector3(i,SelectedIndex.Contains(indexInDrawed)?2:0,0) + bottomLeft;
            var cardDisplay = card.GetComponent<CardDisplay>();
            cardDisplay.rankText.text = Cards[CardIndex[indexInDrawed]].Rank.ToString();
            cardDisplay.suitText.text = Cards[CardIndex[indexInDrawed]].Suit.ToString();
            card.SetActive(true);
            cardDisplay.CardIndexInHand = indexInDrawed;
        }
    }


    private class SortByRank : IComparer<int>
    {
        int IComparer<int>.Compare(int a, int b) //implement Compare
        {              
            if (a%13 > b%13)
                return -1; //normally greater than = 1
            if (a%13 < b%13)
                return 1; // normally smaller than = -1
            if (a > b)
                return -1; //normally greater than = 1
            if (a < b)
                return 1;
            return 0; // equal
        }
    }
    public  List<int> MapIndex(List<int> indexes)
    {
       //want to display the 0,1,2,3,4 card, so we want to return the maped 0,1,2,3,4 index
       //so that is, in the sort by suit, it is index 0, in the sort by rank, which index should it be
       //first sort hand by rank

       // Use LINQ OrderBy with your custom comparer
       var sortedWithIndex = indexes.Select((value, index) => new { value, index })
           .OrderBy(n => n.value, new SortByRank())
           .ToList();

       // Step 2: Create a dictionary for reverse lookup
       Dictionary<int, int> indexMap = sortedWithIndex.Select((x, i) => new { originalIndex = x.index, newIndex = i })
           .ToDictionary(x => x.newIndex, x => x.originalIndex);

       // Step 3: Generate the result list directly using LINQ
       List<int> result = Enumerable.Range(0, indexes.Count)
           .Select(i => indexMap[i])
           .ToList();
       return result;





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


