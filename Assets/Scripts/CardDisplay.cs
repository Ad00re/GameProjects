using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public int CardIndexInHand;
    public Text rankText;
    public Text suitText;
    public Text scoreText;

    public void OnMouseDown()
    {
        var Selected = CardManager.Instance.Selected;
        if (Selected.Contains(CardIndexInHand))
        {
            Selected.Remove(CardIndexInHand);
        }
        else
        {
            Selected.Add(CardIndexInHand);
        }
        List<int> selectedCardIndices = Selected.Select(i => CardManager.Instance.Drawed[i]).ToList();
        List<int> allCard = selectedCardIndices.Concat(CardManager.Instance.Play).ToList();
        List<Card> selectedCards = allCard.Select(i => CardManager.Instance.Cards[i]).ToList();
        Hand currentHand = CardManager.Instance.CalculateSelectedHand(selectedCards);
        (int, int) currentScore = CardManager.Instance.CalculateSelectedScore(currentHand);
        CardManager.Instance.score = currentScore.Item1;
        CardManager.Instance.multi = currentScore.Item2;
        CardManager.Instance.hand = currentHand;
        CardManager.Instance.MarkDirty();
    }
    
    
    
}
