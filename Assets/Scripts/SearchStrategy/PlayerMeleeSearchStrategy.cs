using System.Collections.Generic;

public class PlayerMeleeSearchStrategy : SearchStrategy
{
    public override bool Search(Entity searcher, Map map)
    {
        List<Entity> entitiesInRadio = SearchEntitiesInRange(searcher, 1, map);
        entitiesInRadio.Remove(searcher);
        onEntitiesToMeleeAttackChange?.Invoke(searcher as Player, entitiesInRadio);
        return entitiesInRadio.Count > 0;
    }
}
