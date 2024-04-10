using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public class MineManager : MonoBehaviour
{
    // model
    public enum State 
    {
        Init,
        Flagged,
        Opened
    }
    
    public enum GameState 
    {
        Playing,
        Win,
        Lose
    }
    public int  gridSize;
    public int mineCount;
    public int[,] scoreGrid;
    public State[,] gridStates;
    public int opened = 0;
    public String difficulty;

    private float time = 0;
    private float delayTime = 1f;
    
    public GameState gameState = GameState.Playing;
    
    
    //view
    [SerializeField] private GameObject[] boxs;
    [SerializeField] private Sprite flag;
    [SerializeField] private Sprite[] number;
    private Sprite defaultSprite;
    public Color[] colors;

       
    public List<(Vector2Int, State)> changeList = new List<(Vector2Int, State)>();
    
    
    
    public static MineManager Instance { get; private set; }

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
        if (MainMenuController.Difficulty == "Easy")
        {
            gridSize = 9;
            mineCount = 10;
        }
        else if (MainMenuController.Difficulty == "Medium")
        {
            gridSize = 16;
            mineCount = 40;
        }
        else
        {
            gridSize = 25;
            mineCount = 99;
        }
        
        scoreGrid = new int[gridSize, gridSize];
        gridStates = new State[gridSize, gridSize];
        colors = new Color[boxs.Length];

        for (int i = 0; i < boxs.Length; i++)
        {
            colors[i] = boxs[i].GetComponent<SpriteRenderer>().color;
        }

        defaultSprite = boxs[0].GetComponent<SpriteRenderer>().sprite;

        
        // define the mine grid
        for (int i = 0; i < mineCount; i++)
        {
            int x;
            int y;
            do
            {
                x = Random.Range(0, gridSize - 1);
                y = Random.Range(0, gridSize - 1);
            } while (scoreGrid[x, y] == -1);

            scoreGrid[x, y] = -1;
        }
        
        // define the state and GameObject of grids
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                gridStates[i, j] = State.Init;
                GameObject newBox = Instantiate(boxs[(i+j)%2]);
                newBox.transform.position = new Vector3((i-gridSize/2f+0.5f) *10f /(gridSize+1), (j-gridSize/2f+0.5f) *10f /(gridSize+1), 0);
                newBox.transform.localScale = new Vector3(10f /(gridSize+1), 10f /(gridSize+1), 0);
                newBox.name = "box" + i + "_" + j;
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (scoreGrid[i, j] != -1)
                {
                    scoreGrid[i, j] = CheckNeighbor(new Vector2Int(i, j));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != GameState.Playing)
        {
            time += Time.deltaTime;
            if (time > delayTime)
            {
                SceneManager.LoadScene("Scenes/End");
            }
        }
    }
    
    int CheckNeighbor(Vector2Int loc)
    {
        int counter = 0;
        for (int i = -1; i < 2; i++)
        {
            if (loc.x + i < 0 || loc.x + i >= gridSize)
            {
                continue;
            }
            for (int j = -1; j < 2; j++)
            {
                if (loc.y + j >= 0 && loc.y + j < gridSize)
                {
                    if (scoreGrid[loc.x + i, loc.y + j] ==-1)
                    {
                        counter += 1;
                    }
                }
            }
        }
        return counter;
    }

    public void ChangeState(Vector2Int loc, State state)
    {
        gridStates[loc.x, loc.y] = state;
        if (state == State.Opened)
        {
            opened += 1;
            Debug.Log(loc+": "+opened);
            if (scoreGrid[loc.x, loc.y] == -1)
            {
                Debug.Log("end game lose");
                gameState = GameState.Lose;
            }
            if (opened == gridSize * gridSize - mineCount)
            {
                Debug.Log("end game win");
                gameState = GameState.Win;
            }
        }
    }

    public void UpdatedGridValue()
    {
        
        while (changeList.Count > 0)
        {
            Vector2Int loc= changeList[0].Item1;
            State s = changeList[0].Item2;
            GameObject o = GameObject.Find("box" + (loc.x) + "_" + (loc.y));
            Sprite image;
            Color color = Color.white;
            
            changeList.RemoveAt(0);
            if (s == State.Opened)
            {
                if (scoreGrid[loc.x, loc.y] == -1)
                {
                    image = flag;
                    color = Color.red;
                }
                else
                {
                    image = number[scoreGrid[loc.x,loc.y]];
                }
            }
            else if (s == State.Flagged)
            {
                image = flag;
            }
            else
            {
                image = defaultSprite;
                color = colors[(loc.x+loc.y)%2];
            }
            o.GetComponent<SpriteRenderer>().sprite = image;
            o.GetComponent<SpriteRenderer>().color = color;
            o.transform.localScale = new Vector3((1024f / (gridSize+1)) / image.rect.size.x,
                (1024f / (gridSize+1)) / image.rect.size.y, 1f);
        }
    }
}
