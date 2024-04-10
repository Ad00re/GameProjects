using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static String GameDifficulty;
    

    private void Start()
    {
        Debug.Log("load start page");
    }

    public void LoadGame(String input)
    {
        GameDifficulty = input;
        SceneManager.LoadScene("Scenes/Game");
    }
}
