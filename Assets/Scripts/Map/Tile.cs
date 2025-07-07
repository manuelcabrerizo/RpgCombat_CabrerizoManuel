using UnityEngine;

public class Tile
{ 
    public GameObject Go;
    public Entity Entity;
    public int X;
    public int Y;

    public bool IsEmpty()
    {
        return (Entity == null) || (Entity != null && Entity.gameObject.activeSelf == false);
    }
}
