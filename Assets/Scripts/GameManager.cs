using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject smallButton;
    public GameObject bigButton;
    public GameObject bossButton;

    public bool modified;
    void Start()
    {
        modified = true;

    }

    // Update is called once per frame
    public void StartGame()
    {
        StateManager.Instance.gameState = StateManager.GameState.game;
        StateManager.Instance.SetDefault();
        StateManager.Instance.MarkDirty();
    }
    
    public void SkipGame()
    {
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
        StateManager.Instance.MarkDirty();


    }

    void Update()
    {
        if (modified)
        {
            if (StateManager.Instance.blindState == StateManager.BlindState.small)
            {
                smallButton.SetActive(true);
                bigButton.SetActive(false);
                bossButton.SetActive(false);
            }
            else if (StateManager.Instance.blindState == StateManager.BlindState.big)
            {
                smallButton.SetActive(false);
                bigButton.SetActive(true);
                bossButton.SetActive(false);
            }
            else if (StateManager.Instance.blindState == StateManager.BlindState.boss)
            {
                smallButton.SetActive(false);
                bigButton.SetActive(false);
                bossButton.SetActive(true);
            }
        }
        
    }
}
