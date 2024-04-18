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
    public List<int> prevDrawed;
    public List<int> Drawed;
    public List<int> Selected;
    private bool modified = false;

    [SerializeField] private GameObject cardPrefab;
    
    // Start is called before the first frame update
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
        for (int i = 0; i < numberOfDraw; i++)
        {
            int indexOfCard = Random.Range(0, Deck.Count);
            Drawed.Add(Deck[indexOfCard]);
            Deck.RemoveAt(indexOfCard);
            modified = true;
        }
        
    }

    void UpdateDrawDisplay()
    {
        int counter = -1;
        foreach (int ind in Drawed)
        {
            if (!prevDrawed.Contains(ind))
            {
                GameObject card = Instantiate(cardPrefab);
                card.transform.Translate(counter,0,0);
                card.GetComponent<CardDisplay>().rankText.text = Cards[ind].Rank.ToString();
                card.GetComponent<CardDisplay>().suitText.text = Cards[ind].Suit.ToString();
                card.SetActive(true);
                counter += 1;
            }
        }

        modified = false;

    }
}


