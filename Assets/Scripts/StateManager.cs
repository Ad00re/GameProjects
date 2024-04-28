using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum GameState
    {
        blind,game,cash,shop
    }

    public enum BlindState
    {
        small,big,boss
    }
    
    public GameState gameState;
    public BlindState blindState;
    public bool modified;
    public GameObject gameView;
    public GameObject shopView;
    public GameObject blindView;
    public GameObject cashView;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI chipText;
    public TextMeshProUGUI multiText;
    public TextMeshProUGUI handText;
    public TextMeshProUGUI discardText;
    public TextMeshProUGUI cashText;

    public int score;
    public int roundTarget;
    public int chip;
    public int multi;
    public int hand;
    public int discard;
    public int cash;
    public static StateManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        gameState = GameState.blind;
        blindState = BlindState.small;
        cash = 0;
        modified = true;
        roundTarget = 300;
    }

    // Update is called once per frame
    void Update()
    {
        if (modified)
        {
            //update the display of text
            scoreText.text = score.ToString();
            chipText.text = chip.ToString();
            multiText.text = multi.ToString();
            handText.text = hand.ToString();
            discardText.text = discard.ToString();
            cashText.text = cash.ToString();
            //check GameState, activate/deactivate game object
            if (gameState == GameState.blind)
            {
                blindView.SetActive(true);
                gameView.SetActive(false);
                cashView.SetActive(false);
                shopView.SetActive(false);
            }
            else if (gameState == GameState.game)
            {
                blindView.SetActive(false);
                gameView.SetActive(true);
                cashView.SetActive(false);
                shopView.SetActive(false);
            }
            else if (gameState == GameState.cash)
            {
                blindView.SetActive(false);
                gameView.SetActive(false);
                cashView.SetActive(true);
                shopView.SetActive(false);
            }
            else if (gameState == GameState.shop)
            {
                blindView.SetActive(false);
                gameView.SetActive(false);
                cashView.SetActive(false);
                shopView.SetActive(true);
            }
            
        }
        modified = false;
    }
    
    public void MarkDirty()
    {
        modified = true;
    }

    public void SetDefault()
    {
        score=0; 
        chip=0; 
        multi=0; 
        hand=4; 
        discard=4;
    }
    
}
