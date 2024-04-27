using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void NextRound()
    {
        StateManager.Instance.gameState = StateManager.GameState.blind;
        if (StateManager.Instance.blindState == StateManager.BlindState.small)
        {
            StateManager.Instance.blindState = StateManager.BlindState.big;
        }
        else if (StateManager.Instance.blindState == StateManager.BlindState.big)
        {
            StateManager.Instance.blindState = StateManager.BlindState.boss;
        }
        else if (StateManager.Instance.blindState == StateManager.BlindState.boss)
        {
            StateManager.Instance.blindState = StateManager.BlindState.small;
        }

        StateManager.Instance.roundTarget *= 2;
        CardManager.Instance.SetDefault();
        StateManager.Instance.MarkDirty();
        CardManager.Instance.MarkDirty();
        
    }
}
