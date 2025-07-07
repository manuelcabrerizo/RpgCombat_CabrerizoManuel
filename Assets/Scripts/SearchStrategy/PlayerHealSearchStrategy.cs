using System.Collections.Generic;

public class PlayerHealSearchStrategy : SearchStrategy
{
    public override bool Search(Entity searcher, Map map)
    {
        List<Entity> entitiesInRadio = SearchEntitiesInRange(searcher, searcher.Data.HealRadio, map);
        List<Entity> toRemove = new List<Entity>();
        foreach (Entity entity in entitiesInRadio)
        {
            if (entity as Enemy != null)
            { 
                toRemove.Add(entity);
            }
        }
        foreach (Entity entity in toRemove)
        { 
            entitiesInRadio.Remove(entity);
        }
        toRemove.Clear();

        onEntitiesToHealChange?.Invoke(searcher as Player, entitiesInRadio);
        return entitiesInRadio.Count > 0;
    }
}
