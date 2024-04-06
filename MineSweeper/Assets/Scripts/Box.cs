using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Box : MonoBehaviour
{
    
    public static Sprite flag;
    public static Sprite boxColor;
    
    public int value;
    public static int scale;
    
    public static Sprite[] number;
    
    
    


    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
    }
    
    (int,int) findLocation(GameObject o)
    {
        Regex pattern = new Regex(@"box(\d+)_(\d+)");
        Match match = pattern.Match(o.name);
        int x = int.Parse(match.Groups[1].Value);  
        int y = int.Parse(match.Groups[2].Value);
        return (x, y);
    }
    
    public static void ChangeSpriteFlag(GameObject o)
    {
        o.GetComponent<SpriteRenderer>().sprite = flag;
        o.GetComponent<SpriteRenderer>().color = Color.white;
        o.transform.localScale = new Vector3((1024f / (scale+1)) / flag.rect.size.x,
            (1024f / (scale+1)) / flag.rect.size.y, 1f);
    }
                
    public static void ChangeSpriteMine(GameObject o)
    {
        o.GetComponent<SpriteRenderer>().sprite = flag;
        o.GetComponent<SpriteRenderer>().color = Color.red;
        o.transform.localScale = new Vector3((1024f / (scale+1)) / flag.rect.size.x,
            (1024f / (scale+1)) / flag.rect.size.y, 1f);
        
    }
                
    public static void ChangeSprite(GameObject o)
    {
        Sprite image = number[o.GetComponent<Box>().value];
        o.GetComponent<SpriteRenderer>().sprite = image;
        o.GetComponent<SpriteRenderer>().color = Color.white;
        o.transform.localScale = new Vector3((1024f / (scale+1)) / image.rect.size.x,
            (1024f / (scale+1)) / image.rect.size.y, 1f);
        
    }
    
    public void ChangeSpriteReset(GameObject o)
    {
        Sprite image = boxColor;
        o.GetComponent<SpriteRenderer>().sprite = image;
        o.GetComponent<SpriteRenderer>().color = Color.white;
        o.transform.localScale = new Vector3((1024f / (scale+1)) / image.rect.size.x,
            (1024f / (scale+1)) / image.rect.size.y, 1f);
        
    }

    public void ClickOn()
    {
        GameObject self = GameObject.Find(name);
        (int selfX, int selfY) = findLocation(self);
        if (MineManager.states[selfX,selfY] == 0)
        {
            if (Input.GetMouseButtonUp(1))
            {
                MineManager.ChangeState(new Vector2Int(selfX,selfY),1);
                ChangeSpriteFlag(self);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (value ==-1)
                {
                    ChangeSpriteMine(self);
                    EndGameController.win = false;
                    MineManager.endGame = true;
                }
                else
                {
                    ChangeSprite(self);
                    // first find the x and y
                    (int x, int y) = findLocation(GameObject.Find(name));
                    // bfs to expand neighbour
                    MineManager.Expand(new Vector2Int(selfX,selfY));
                    
                    
                }
            }
            //check for win game
        }

        if (MineManager.states[selfX,selfY] == 1)
        {
            if (Input.GetMouseButtonUp(1))
            {
                MineManager.ChangeState(new Vector2Int(selfX,selfY),0);
                ChangeSpriteFlag(self);
            }

        }

    }
}
