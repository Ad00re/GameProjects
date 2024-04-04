using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    public static bool win;
    public Text result;

    void Start()
    {
        if (win)
        {
            result.text = "You Win!";
        }
        else
        {
            result.text = "You Loose";
        }
    }
    public void Replay()
    {
        win = false;
        SceneManager.LoadScene("Scenes/Start");
    }
}
