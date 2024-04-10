using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEditorInternal.VersionControl;

public class MineDisplay : MonoBehaviour
{ 
    public void ClickOnObject()
    {
        Vector2Int loc  = FindLocation(gameObject);
        if (Input.GetMouseButtonUp(1))
        {
            if (MineManager.Instance.gridStates[loc.x, loc.y] == MineManager.State.Init)
            {
                MineManager.Instance.ChangeState(loc,MineManager.State.Flagged);
                MineManager.Instance.changeList.Add((loc,MineManager.State.Flagged));
            }
            else if (MineManager.Instance.gridStates[loc.x, loc.y] == MineManager.State.Flagged)
            {
                MineManager.Instance.ChangeState(loc,MineManager.State.Init);
                MineManager.Instance.changeList.Add((loc,MineManager.State.Init));
            }
        }
        else
        {
            if (MineManager.Instance.gridStates[loc.x, loc.y] == MineManager.State.Init)
            {
                MineManager.Instance.ChangeState(loc,MineManager.State.Opened);
                MineManager.Instance.changeList.Add((loc,MineManager.State.Opened));
                if (MineManager.Instance.scoreGrid[loc.x, loc.y] == 0)
                {
                    Expand(loc);
                }
            }
        }
        MineManager.Instance.UpdatedGridValue();
    }
    
    Vector2Int FindLocation(GameObject o)
    {
        Regex pattern = new Regex(@"box(\d+)_(\d+)");
        Match match = pattern.Match(o.name);
        int x = int.Parse(match.Groups[1].Value);  
        int y = int.Parse(match.Groups[2].Value);
        return new Vector2Int(x, y);
    }

    void Expand(Vector2Int loc)
    {
        List<Vector2Int> queue = new List<Vector2Int>{loc};
        while (queue.Count > 0)
        {
            loc= queue[0];
            queue.RemoveAt(0);
            for (int i = -1; i < 2; i++)
            {
                if (loc.x + i < 0 || loc.x + i >= MineManager.Instance.gridSize)
                {
                    continue;
                }
                for (int j = -1; j < 2; j++)
                {
                    if (loc.y + j >= 0 && loc.y + j < MineManager.Instance.gridSize)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }

                        Vector2Int curLoc = new Vector2Int(i, j) + loc;
                        if (MineManager.Instance.gridStates[loc.x+i,loc.y+j] == MineManager.State.Init)
                        {
                            if (MineManager.Instance.scoreGrid[loc.x+i,loc.y+j]==0 )
                            {
                                queue.Add(curLoc);
                            }
                            MineManager.Instance.ChangeState(curLoc,MineManager.State.Opened);
                            MineManager.Instance.changeList.Add((curLoc,MineManager.State.Opened));
                        }
                    }
                }
            }
        }
    }
    
    

}
