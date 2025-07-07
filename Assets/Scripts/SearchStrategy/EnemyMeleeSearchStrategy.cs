using System.Collections.Generic;

public class EnemyMeleeSearchStrategy : SearchStrategy
{
    public override bool Search(Entity searcher, Map map)
    {
        List<Entity> entitiesInRadio = SearchEntitiesInRange(searcher, 1, map);
        entitiesInRadio.Remove(searcher);

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
            searcher.MeleeAttack(entitiesInRadio[randomIndex]);
            return true;
        }
        return false;
    }
}
