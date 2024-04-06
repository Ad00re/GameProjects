using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    public static int  gridSize;
    public static int mineCount;
    public static int[,] grid;
    public static int[,] states;
    
    void Start()
    {
        gridSize = 9;
        mineCount = 10;
        grid = new int[gridSize, gridSize];
        states = new int[gridSize, gridSize];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
