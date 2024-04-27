using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum GameState
    {
        bind,game,shop
    }

    public GameState gameState;
    public bool modified;
    public GameObject gameView;
    public GameObject shopView;
    public GameObject bindView;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI chipText;
    public TextMeshProUGUI multiText;
    public TextMeshProUGUI handText;
    public TextMeshProUGUI discardText;
    public TextMeshProUGUI cashText;

    public int score;
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
        gameState = GameState.bind;
        cash = 0;
        modified = true;
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
            if (gameState == GameState.bind)
            {
                bindView.SetActive(true);
                gameView.SetActive(false);
                shopView.SetActive(false);
            }
            else if (gameState == GameState.game)
            {
                bindView.SetActive(false);
                gameView.SetActive(true);
                shopView.SetActive(false);
            }
            else if (gameState == GameState.shop)
            {
                bindView.SetActive(false);
                gameView.SetActive(false);
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
