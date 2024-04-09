using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static String Difficulty;

    private void Start()
    {
        Debug.Log("load start page");
    }

    public void LoadGame(string input)
    {
        Difficulty = input;
        SceneManager.LoadScene("Scenes/Game");
    }
}
