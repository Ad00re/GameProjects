
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
    public static int  gridSize;
    public static int mineCount;
    public static float windowSize =10f;
    public static int[,] grid;

    public static int[,] states;
    public static bool win;
    public static bool endGame;
    public static float time;
    public static float delayTime = 1f;

    public static int opened;
    [SerializeField] public  GameObject[] boxs;
    
    
    
    
    
    void Start()
    {
        if (StateManager.Difficulty == "Easy")
        {
            gridSize = 9;
            mineCount = 10;
        }
        else if (StateManager.Difficulty == "Medium")
        {
            gridSize = 16;
            mineCount = 40;
        }
        else if (StateManager.Difficulty == "Hard")
        {
            gridSize = 25;
            mineCount = 99;
        }
        
        grid = new int[gridSize, gridSize];
        states = new int[gridSize, gridSize];
        for (int i = 0; i < mineCount; i++)
        {
            int x;
            int y;
            do
            {
                x = Random.Range(0, gridSize-1);
                y = Random.Range(0, gridSize-1);
            } while (grid[x, y]==-1);
            Debug.Log(x+","+y);
            grid[x, y] = -1;
            
        }
        
        Box.scale = gridSize;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject newBox = Instantiate(boxs[(i+j)%2]);
                // 总共十个格子，n*n的时候每个格子大小是10/（n+1）
                // 那么 i就在（i-gridSize/2f+0.5f） * 10 /（n+1）
                newBox.transform.position = new Vector3((i-gridSize/2f+0.5f) *windowSize /(gridSize+1), (j-gridSize/2f+0.5f) *windowSize /(gridSize+1), 0);
                newBox.transform.localScale = new Vector3(windowSize /(gridSize+1), windowSize /(gridSize+1), 0);
                newBox.name = "box" + i.ToString() + "_" + j.ToString();
                if (grid[i, j] != -1)
                {
                    newBox.GetComponent<Box>().value = checkNeighbor(new Vector2Int(i, j));

                }
                else
                {
                    newBox.GetComponent<Box>().value = grid[i, j];

                }
                
            }
        }
    }

    void Update()
    {
        if (endGame)
        {
            time += Time.deltaTime;
        }
        
        if (time >= delayTime)
        {
            SceneManager.LoadScene("Scenes/End");
        }
        
    }

    public int checkNeighbor(Vector2Int loc)
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
                    if (grid[loc.x + i, loc.y + j] ==-1)
                    {
                        counter += 1;
                    }
                }
            }
        }
        return counter;
    }

    public static void ChangeState(Vector2Int loc, int state)
    {
        states[loc.x, loc.y] = state;
        if (state == 2)
        {
            opened += 1;
        }
        
    }

    public static void Expand(Vector2Int loc)
    {
        List<(int, int)> queue;
        ChangeState(new Vector2Int(loc.x, loc.y),2);
        Box.ChangeSprite(GameObject.Find("box" + (loc.x) + "_" + (loc.y)));
        if (grid[loc.x, loc.y] == 0)
        {
             queue = new List<(int, int)> { (loc.x,loc.y) };
        }
        else
        {
            return;
        }
        int x;
        int y;
        while (queue.Count > 0)
        {
            (x,  y)= queue[0];
            queue.RemoveAt(0);
            
            for (int i = -1; i < 2; i++)
            {
                if (x + i < 0 || x + i >= gridSize)
                {
                    continue;
                }
                for (int j = -1; j < 2; j++)
                {
                    if (y + j >= 0 && y + j < gridSize)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
                        GameObject curBox = GameObject.Find("box" + (x+i) + "_" + (y+j));
                        int stateBeforeModify = states[x,y];
                        if (stateBeforeModify == 0)
                        {
                            ChangeState(new Vector2Int(x,y),2);
                            Box.ChangeSprite(curBox);
                            if (grid[x,y]==0)
                            {
                                queue.Add((x+i,y+j));
                            }
                        }
                                    
                    }
                }
            }
        }
        if (opened == gridSize * gridSize - mineCount)
        {
            opened = 0;
            EndGameController.win = true;
            win = true;
            endGame = true;
        }
    }
    
    
    
    
}
