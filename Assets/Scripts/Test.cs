using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Card> Straightflush0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Clubs, Rank.Two),
            new (Suit.Clubs, Rank.Three),
            new (Suit.Clubs, Rank.Four),
            new (Suit.Clubs, Rank.Five),
        };
        Debug.Log("Straightflush2"+CardManager.Instance.CalculateSelectedHand(Straightflush0));
        
        List<Card> Straightflush1 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Clubs, Rank.Ten),
            new (Suit.Clubs, Rank.Jack),
            new (Suit.Clubs, Rank.Queen),
            new (Suit.Clubs, Rank.King),
        };
        Debug.Log("Straightflush2"+CardManager.Instance.CalculateSelectedHand(Straightflush1));

        
        List<Card> Straightflush2 = new List<Card>
        {
            new (Suit.Clubs, Rank.Six),
            new (Suit.Clubs, Rank.Two),
            new (Suit.Clubs, Rank.Three),
            new (Suit.Clubs, Rank.Four),
            new (Suit.Clubs, Rank.Five),
        };
        Debug.Log("Straightflush2"+CardManager.Instance.CalculateSelectedHand(Straightflush2));

        
        List<Card> Straight0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Spades, Rank.Ten),
            new (Suit.Diamonds, Rank.Jack),
            new (Suit.Spades, Rank.Queen),
            new (Suit.Hearts, Rank.King),
        };
        Debug.Log("Straight"+CardManager.Instance.CalculateSelectedHand(Straight0));

        List<Card> NoneStraight0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Spades, Rank.Two),
            new (Suit.Diamonds, Rank.Three),
            new (Suit.Spades, Rank.Three),
            new (Suit.Hearts, Rank.Five),
        };
        Debug.Log("NoneStraight0"+CardManager.Instance.CalculateSelectedHand(NoneStraight0));

        
        List<Card> Flush0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Clubs, Rank.Three),
            new (Suit.Clubs, Rank.Five),
            new (Suit.Clubs, Rank.Seven),
            new (Suit.Clubs, Rank.King),
        };
        Debug.Log("Flush0"+CardManager.Instance.CalculateSelectedHand(Flush0));

        
        List<Card> Four0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Hearts, Rank.Ace),
            new (Suit.Diamonds, Rank.Ace),
            new (Suit.Spades, Rank.Ace),
            new (Suit.Clubs, Rank.King),
        };
        Debug.Log("Four0"+CardManager.Instance.CalculateSelectedHand(Four0));

        
        List<Card> FullHouse0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Hearts, Rank.Ace),
            new (Suit.Diamonds, Rank.Ace),
            new (Suit.Spades, Rank.King),
            new (Suit.Clubs, Rank.King),
        };
        Debug.Log("FullHouse0"+CardManager.Instance.CalculateSelectedHand(FullHouse0));

        
        List<Card> TwoPaird0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Hearts, Rank.Ace),
            new (Suit.Diamonds, Rank.Two),
            new (Suit.Spades, Rank.King),
            new (Suit.Clubs, Rank.King),
        };
        Debug.Log("TwoPaird0"+CardManager.Instance.CalculateSelectedHand(TwoPaird0));

        
        List<Card> ThreeOfAKind0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Hearts, Rank.Two),
            new (Suit.Diamonds, Rank.King),
            new (Suit.Spades, Rank.King),
            new (Suit.Clubs, Rank.King),
        };
        Debug.Log("ThreeOfAKind0"+CardManager.Instance.CalculateSelectedHand(ThreeOfAKind0));


        List<Card> Pair0 = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Hearts, Rank.Two),
            new (Suit.Diamonds, Rank.Queen),
            new (Suit.Spades, Rank.King),
            new (Suit.Clubs, Rank.King),
        };
        Debug.Log("Pair0"+CardManager.Instance.CalculateSelectedHand(Pair0));

        
        List<Card> HighCard = new List<Card>
        {
            new (Suit.Clubs, Rank.Ace),
            new (Suit.Hearts, Rank.Two),
            new (Suit.Diamonds, Rank.Queen),
            new (Suit.Spades, Rank.Jack),
            new (Suit.Clubs, Rank.King),
        };
        Debug.Log("HighCard"+CardManager.Instance.CalculateSelectedHand(HighCard));



        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
