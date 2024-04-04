using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
   public void LoadGame(string input)
   {
      StateManager.Difficulty = input;
      SceneManager.LoadScene("Scenes/Game");
   }
   
   
    
}
