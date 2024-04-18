using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public int CardIndexInHand;
    public Text rankText;
    public Text suitText;

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

        CardManager.Instance.MarkDirty();

    }
    
    
    
}
