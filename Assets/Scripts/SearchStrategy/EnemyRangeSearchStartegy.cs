using System.Collections.Generic;

public class EnemyRangeSearchStartegy : SearchStrategy
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

        if (entitiesInRadio.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, entitiesInRadio.Count);
            searcher.RangeAttack(entitiesInRadio[randomIndex]);
            return true;
        }
        return false;

    }
}
