using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadScene("Scenes/Start");
    }
}
