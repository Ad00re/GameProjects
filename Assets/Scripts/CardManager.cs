using System;
using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        Cards = new List<Card>();
        Deck = new List<int>();
        Drawed = new List<int>();
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
        

    }

    // Update is called once per frame
    void Update()
    {
        if (modified)
        {
            UpdateDrawDisplay();
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

    private List<GameObject> CardsView = new ();
    void UpdateDrawDisplay()
    {
        for(int i = 0;i<Drawed.Count;i++)
        {
            if (i >= CardsView.Count)
            {
                GameObject card = Instantiate(cardPrefab);
                CardsView.Add(card);
                card.transform.position = new Vector3(-1+i,-1,0);
            }
        }
        for (int i = 0; i < CardsView.Count; i++)
        {
            GameObject card = CardsView[i];
            card.transform.position= new Vector3(-1+i,Selected.Contains(i)?0:-2,0);
            var cardDisplay = card.GetComponent<CardDisplay>();
            cardDisplay.rankText.text = Cards[Drawed[i]].Rank.ToString();
            cardDisplay.suitText.text = Cards[Drawed[i]].Suit.ToString();
            card.SetActive(true);
            cardDisplay.CardIndexInHand = i;
        }

        modified = false;

    }

    public void MarkDirty()
    {
        modified = true;
    }
}


