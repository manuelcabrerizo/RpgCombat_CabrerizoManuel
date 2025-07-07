using System.Collections.Generic;

public class PlayerRangeSearchStrategy : SearchStrategy
{
    public override bool Search(Entity searcher, Map map)
    {
        List<Entity> entitiesInRadio = SearchEntitiesInRange(searcher, searcher.Data.RangeAttackRadio, map);
        List<Entity> entitiesToClose = SearchEntitiesInRange(searcher, 1, map);
        entitiesInRadio.Remove(searcher);
        foreach (Entity entity in entitiesToClose)
        {
            entitiesInRadio.Remove(entity);
        }
        onEntitiesToRangeAttackChange?.Invoke(searcher as Player, entitiesInRadio);
        return entitiesInRadio.Count > 0;
    }
}
