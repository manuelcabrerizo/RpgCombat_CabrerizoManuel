using System;
using System.Collections.Generic;

public abstract class SearchStrategy
{
    public static Action<Player, List<Entity>> onEntitiesToHealChange;
    public static Action<Player, List<Entity>> onEntitiesToRangeAttackChange;
    public static Action<Player, List<Entity>> onEntitiesToMeleeAttackChange;

    public abstract bool Search(Entity searcher, Map map);

    protected List<Entity> SearchEntitiesInRange(Entity searcher, int range, Map map)
    {
        List<Entity> entitiesInRadio = new List<Entity>();
        int minX = searcher.X - range;
        int maxX = searcher.X + range;
        int minY = searcher.Y - range;
        int maxY = searcher.Y + range;
        if (minX < 0) minX = 0;
        if (maxX >= map.GetWidth()) maxX = map.GetWidth() - 1;
        if (minY < 0) minY = 0;
        if (maxY >= map.GetHeight()) maxY = map.GetHeight() - 1;
        for (int y = minY; y <= maxY; ++y)
        {
            for (int x = minX; x <= maxX; ++x)
            {
                Tile tile = map.GetTiles(x, y);
                if (tile.Entity != null && tile.Entity.IsAlive())
                {
                    entitiesInRadio.Add(tile.Entity);
                }
            }
        }
        return entitiesInRadio;
    }
}
