using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {

    }

    // Update is called once per frame
    public void StartGame(String gameLevel)
    {
        StateManager.Instance.gameState = StateManager.GameState.game;
        StateManager.Instance.SetDefault();
        StateManager.Instance.MarkDirty();
    }
    
    public void SkipGame(String gameLevel)
    {
        
    }
}
