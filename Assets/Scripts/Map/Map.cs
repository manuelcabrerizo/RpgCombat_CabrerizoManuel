using UnityEngine;
public class Map
{
    private int width;
    private int height;
    private Tile[] tiles = null;

    public Map(int width, int height, string prefabName, Transform parent)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width * height];
        GameObject tilePrefab = Resources.Load<GameObject>(prefabName);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tile = new Tile();
                tile.Go = GameObject.Instantiate(tilePrefab, parent);
                float positionX = x * tile.Go.transform.localScale.x * 0.5f;
                float positionY = y * tile.Go.transform.localScale.y * 0.5f;
                tile.Go.transform.position = new Vector2(positionX, positionY);
                tile.X = x;
                tile.Y = y;
                tile.Entity = null;
                int tileIndex = y * width + x;
                tiles[tileIndex] = tile;
            }
        }
    }

    public int GetWidth()
    { 
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public Tile GetTiles(int x, int y)
    { 
        return tiles[y * width + x];
    }

    public Tile GetRandomEmptyTile()
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, height);
        int tileIndex = y * width + x;
        while (tiles[tileIndex].Entity != null)
        {
            x = Random.Range(0, width);
            y = Random.Range(0, height);
            tileIndex = y * width + x;
        }
        return tiles[tileIndex];
    }
}
