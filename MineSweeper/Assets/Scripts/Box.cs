using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Box : MonoBehaviour
{
    
    public Sprite flag;
    public Sprite boxColor;
    public bool mine;
    public int neighbour;
    public int scale;
    public Sprite[] number;
    public float delayTime = 1f;
    public float time = 0f;
    public bool die;
    //three states, 0 for init state, 1 for flag, 2 for opened
    public int state;
    

    // Update is called once per frame
    void Update()
    {
        if (die)
        {
            time += Time.deltaTime;
        }

        if (time >= delayTime)
        {
            SceneManager.LoadScene("Scenes/End");
        }
    }
    
    (int,int) findLocation(GameObject o)
    {
                    
        Regex pattern = new Regex(@"box(\d+)_(\d+)");
        Match match = pattern.Match(o.name);
        int x = int.Parse(match.Groups[1].Value);  
        int y = int.Parse(match.Groups[2].Value);
        return (x, y);
    }
    
    void ChangeSpriteFlag(GameObject o)
    {
        o.GetComponent<SpriteRenderer>().sprite = flag;
        o.GetComponent<SpriteRenderer>().color = Color.white;
        o.transform.localScale = new Vector3((1024f / (scale+1)) / flag.rect.size.x,
            (1024f / (scale+1)) / flag.rect.size.y, 1f);
        o.GetComponent<Box>().state = 1;
    }
                
    void ChangeSpriteMine(GameObject o)
    {
        o.GetComponent<SpriteRenderer>().sprite = flag;
        o.GetComponent<SpriteRenderer>().color = Color.red;
        o.transform.localScale = new Vector3((1024f / (scale+1)) / flag.rect.size.x,
            (1024f / (scale+1)) / flag.rect.size.y, 1f);
        o.GetComponent<Box>().state = 2;
    }
                
    void ChangeSprite(GameObject o)
    {
        Sprite image = number[o.GetComponent<Box>().neighbour];
        o.GetComponent<SpriteRenderer>().sprite = image;
        o.GetComponent<SpriteRenderer>().color = Color.white;
        o.transform.localScale = new Vector3((1024f / (scale+1)) / image.rect.size.x,
            (1024f / (scale+1)) / image.rect.size.y, 1f);
        o.GetComponent<Box>().state = 2;
    }
    
    void ChangeSpriteReset(GameObject o)
    {
        Sprite image = boxColor;
        o.GetComponent<SpriteRenderer>().sprite = image;
        o.GetComponent<SpriteRenderer>().color = Color.white;
        o.transform.localScale = new Vector3((1024f / (scale+1)) / image.rect.size.x,
            (1024f / (scale+1)) / image.rect.size.y, 1f);
        o.GetComponent<Box>().state = 0;
    }

    public void ClickOn()
    {
        GameObject self = GameObject.Find(name);
        if (state == 0)
        {
            if (Input.GetMouseButtonUp(1))
            {
                ChangeSpriteFlag(self);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (mine)
                {
                    ChangeSpriteMine(self);
                    die = true;
                }
                else
                {
                    ChangeSprite(self);
                    // first find the x and y
                    (int x, int y) = findLocation(GameObject.Find(name));
                    // bfs to expand neighbour
                    List<(int, int)> queue = new List<(int, int)>();
                    if (neighbour == 0)
                    {
                        queue.Add((x,y));
                    }
                    while (queue.Count > 0)
                    {
                        ( x,  y)= queue[0];
                        queue.RemoveAt(0);
                        // get the valid neighbours
                        GameObject curBox;
                        for (int i = -1; i < 2; i++)
                        {
                            if (x + i < 0 || x + i >= scale)
                            {
                                continue;
                            }
                            for (int j = -1; j < 2; j++)
                            {
                                if (y + j >= 0 && y + j < scale)
                                {
                                    if (i == 0 && j == 0)
                                    {
                                        continue;
                                    }
                                    curBox = GameObject.Find("box" + (x+i) + "_" + (y+j));
                                    int stateBeforeModify = curBox.GetComponent<Box>().state;
                                    ChangeSprite(curBox);
                                    if (curBox.GetComponent<Box>().neighbour==0 && stateBeforeModify==0)
                                    {
                                        queue.Add((x+i,y+j));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return;
        }

        if (state == 1)
        {
            if (Input.GetMouseButtonUp(1))
            {
                ChangeSpriteReset(self);
            }

        }

    }
}
