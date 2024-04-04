
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MineManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int gridSize;
    public int mineCount;
    public float windowSize =10f;
    public bool[,] grid;
    
    [SerializeField] public GameObject[] boxs;
    
    
    
    
    
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
        
        grid = new bool[gridSize, gridSize];
        for (int i = 0; i < mineCount; i++)
        {
            int x;
            int y;
            do
            {
                x = Random.Range(0, gridSize-1);
                y = Random.Range(0, gridSize-1);
            } while (grid[x, y]);
            Debug.Log(x+","+y);
            grid[x, y] = true;
            
        }
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
                newBox.GetComponent<Box>().mine = grid[i, j];
                newBox.GetComponent<Box>().scale = gridSize;
                newBox.GetComponent<Box>().neighbour = checkNeighbor(new Vector2Int(i, j));

            }
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
                    if (grid[loc.x + i, loc.y + j])
                    {
                        counter += 1;
                    }
                }
            }
        }
        return counter;
    } 
    
    
    
    
}
