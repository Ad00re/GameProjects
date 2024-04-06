using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
