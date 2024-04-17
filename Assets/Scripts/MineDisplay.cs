using UnityEngine;

public class MineDisplay : MonoBehaviour
{
    public Vector2Int boxLocation;
    public void ClickOnObject()
    {
        Vector2Int loc = boxLocation;
        if (Input.GetMouseButtonUp(1))
        {
            if (MineManager.Instance.gridStates[loc.x, loc.y] == MineManager.State.Init)
            {
                MineManager.Instance.ChangeState(loc,MineManager.State.Flagged);
            }
            else if (MineManager.Instance.gridStates[loc.x, loc.y] == MineManager.State.Flagged)
            {
                MineManager.Instance.ChangeState(loc,MineManager.State.Init);
            }
        }
        else
        {
            if (MineManager.Instance.gridStates[loc.x, loc.y] == MineManager.State.Init)
            {
                MineManager.Instance.ChangeState(loc,MineManager.State.Opened);
                if (MineManager.Instance.scoreGrid[loc.x, loc.y] == 0)
                {
                    MineManager.Instance.Expand(loc);
                }
            }
        }
        MineManager.Instance.UpdateGridDisplay();
    }

    
    
    

}
