using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}

public class CardManager : MonoBehaviour
{
    public List<Card> Cards;
    
    // Start is called before the first frame update
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


