using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    public static Difficulty GameDifficulty;
    

    private void Start()
    {
        Debug.Log("load start page");
    }

    public void LoadGame(String input)
    {
        if (input == "Easy")
        {
            GameDifficulty = Difficulty.Easy;
        }
        else if (input == "Medium")
        {
            GameDifficulty = Difficulty.Medium;
        }
        else if (input == "Hard")
        {
            GameDifficulty = Difficulty.Hard;
        }
        SceneManager.LoadScene("Scenes/Game");
    }
}
