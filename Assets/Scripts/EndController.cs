using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    public Text result;
    // Start is called before the first frame update
    void Start()
    {
        if (MineManager.Instance.gameState == MineManager.GameState.Win)
        {
            result.text = "You Win!";
        }
        else if (MineManager.Instance.gameState == MineManager.GameState.Lose)
        {
            result.text = "You Loose";
        }
    }

    public void Replay()
    {
        Destroy(MineManager.Instance);
        SceneManager.LoadScene("Scenes/Start");
        
    }
}
