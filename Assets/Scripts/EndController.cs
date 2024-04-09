using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    public Text result;
    // Start is called before the first frame update
    void Start()
    {
        if (MineManager.Instance.win)
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
        MineManager.Instance.win = false;
        MineManager.Instance.lose = false;
        SceneManager.LoadScene("Scenes/Start");
    }
}
