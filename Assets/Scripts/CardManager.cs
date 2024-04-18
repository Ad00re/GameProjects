using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAsync;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Suit
{
    Clubs, Diamonds, Hearts, Spades
}

public enum Rank
{
    Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
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
    
    public bool modified = false;

    [SerializeField] private GameObject cardPrefab;

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
        
        Debug.Log("this is the first msg");
        await UnityAsync.Await.Seconds(1);
        Debug.Log("this is the second msg");
    }

    // Update is called once per frame
    public List<GameObject> CardsViewPlay = new ();
    public List<GameObject> CardsViewHand = new ();
    void Update()
    {
        if (modified)
        {
            DisplayCardView(Drawed, new Vector3(-1, -2, 0), Selected,CardsViewHand);
            modified = false;
            DisplayCardView(Play,new Vector3(-1, 3, 0),new List<int>(),CardsViewPlay);
        }
        
    }

    public void DrawCard()
    {
        Debug.Log(Deck.Count);
        Debug.Log(Drawed.Count);
        int numberOfDraw = 7 - Drawed.Count;
        for (int i = 0; i < 2; i++)
        {
            int indexOfCard = Random.Range(0, Deck.Count);
            Drawed.Add(Deck[indexOfCard]);
            Deck.RemoveAt(indexOfCard);
            MarkDirty();
        }
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
                Play.Add(Drawed[i]);
                Drawed.RemoveAt(i);
                Selected.Remove(i);                
                MarkDirty();
                await UnityAsync.Await.Seconds(1f);
            }
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
}


