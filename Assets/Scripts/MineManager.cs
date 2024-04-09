using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int  gridSize;
    public int mineCount;
    public int[,] gameGrid;
    public State[,] gridStates;
    public int opened = 0;
    
    //view
    [SerializeField] private GameObject[] boxs;
    [SerializeField] private Sprite flag;
    [SerializeField] private Sprite[] number;
    private Sprite defaultSprite;
    public Color[] colors;
    
    
    
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
        gridSize = 9;
        mineCount = 10;
        gameGrid = new int[gridSize, gridSize];
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
            } while (gameGrid[x, y] == -1);

            gameGrid[x, y] = -1;
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
                if (gameGrid[i, j] != -1)
                {
                    gameGrid[i, j] = CheckNeighbor(new Vector2Int(i, j));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    if (gameGrid[loc.x + i, loc.y + j] ==-1)
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
        }
    }
    
    public void ChangeSpriteFlag(Vector2Int loc, GameObject o)
    {
        o.GetComponent<SpriteRenderer>().sprite = flag;
        o.GetComponent<SpriteRenderer>().color = Color.white;
        o.transform.localScale = new Vector3((1024f / (gridSize+1)) / flag.rect.size.x,
            (1024f / (gridSize+1)) / flag.rect.size.y, 1f);
    }
    
                
    public void ChangeSprite(Vector2Int loc, GameObject o)
    {
        if (gameGrid[loc.x, loc.y] == -1)
        {
            o.GetComponent<SpriteRenderer>().sprite = flag;
            o.GetComponent<SpriteRenderer>().color = Color.red;
            o.transform.localScale = new Vector3((1024f / (gridSize+1)) / flag.rect.size.x,
                (1024f / (gridSize+1)) / flag.rect.size.y, 1f);

        }
        else
        {
            Sprite image = number[gameGrid[loc.x,loc.y]];
            o.GetComponent<SpriteRenderer>().sprite = image;
            o.GetComponent<SpriteRenderer>().color = Color.white;
            o.transform.localScale = new Vector3((1024f / (gridSize+1)) / image.rect.size.x,
                (1024f / (gridSize+1)) / image.rect.size.y, 1f);
        }
    }
    public void ChangeSpriteReset(Vector2Int loc, GameObject o)
    {
        Sprite image = defaultSprite;
        o.GetComponent<SpriteRenderer>().sprite = image;
        o.GetComponent<SpriteRenderer>().color = colors[(loc.x+loc.y)%2];
        o.transform.localScale = new Vector3((1024f / (gridSize+1)) / image.rect.size.x,
            (1024f / (gridSize+1)) / image.rect.size.y, 1f);
    }
    
}
