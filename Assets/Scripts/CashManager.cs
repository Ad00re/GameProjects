using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityAsync;


public class CashManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject incomePrefab;
    public GameObject canvas;
    public TextMeshProUGUI blindName;
    public TextMeshProUGUI blindIncome;
    
    public int tempCash;
    public bool modified = true;
    
    void Start()
    {
        tempCash = 0;
    }

    private void Update()
    {
        if (StateManager.Instance.gameState==StateManager.GameState.cash && modified)
        {
            switch (StateManager.Instance.blindState)
            {
                case StateManager.BlindState.small:
                    blindName.text = "Small Blind";
                    blindIncome.text = "$$$";
                    tempCash += 3;
                    break;
                case StateManager.BlindState.big:
                    blindName.text = "Big Blind";
                    blindIncome.text = "$$$$$";
                    tempCash += 5;
                    break;
                case StateManager.BlindState.boss:
                    blindName.text = "Boss Blind";
                    blindIncome.text = "$$$$$";
                    tempCash += 5;
                    break;
            }

            int counter = 0;
            //case remaining hand
            if (StateManager.Instance.hand > 0)
            {
                GameObject income = Instantiate(incomePrefab, canvas.transform);
                income.transform.position += 150*Vector3.down;
                income.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Remaining Hands";
                income.transform.Find("Cash").GetComponent<TextMeshProUGUI>().text = new string('$', StateManager.Instance.hand);
                counter += 1;
                tempCash += StateManager.Instance.hand;
            }
            //case interest
            if (StateManager.Instance.cash > 5)
            {
                GameObject income = Instantiate(incomePrefab, canvas.transform);
                income.transform.position += (150+100*counter)*Vector3.down;
                income.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Interest";
                income.transform.Find("Cash").GetComponent<TextMeshProUGUI>().text = new string('$', StateManager.Instance.cash/5);
                counter += 1;
                tempCash += StateManager.Instance.cash / 5;
            }
            modified = false;
        }
    }

    public async void CashOut()
    {
        StateManager.Instance.cash += tempCash;
        tempCash = 0;
        StateManager.Instance.MarkDirty();
        await UnityAsync.Await.Seconds(1f);
        StateManager.Instance.gameState = StateManager.GameState.shop;
        StateManager.Instance.SetDefault();
        StateManager.Instance.MarkDirty();
        modified = true;
    }
    
}
