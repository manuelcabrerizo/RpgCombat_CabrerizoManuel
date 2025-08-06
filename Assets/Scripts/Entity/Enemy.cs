using System;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : Entity
{
    public static event Action<Enemy, string> onEnemyCreate;
    public static event Action<Enemy> onEnemyKill;

    private float time;
    private List<SearchStrategy> searchStrategies = new List<SearchStrategy>();

    protected override void OnStart()
    {
        onEnemyCreate?.Invoke(this, Name);
        searchStrategies.Add(new EnemyMeleeSearchStrategy());
        searchStrategies.Add(new EnemyRangeSearchStartegy());
    }

    public override void ProcessTurn()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            bool moveDone = false;
            while (!moveDone)
            {
                int dir = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    if (IsValidMove(dir, 0))
                    {
                        Move(dir, 0);
                        moveDone = true;
                    }
                }
                else
                {
                    if (IsValidMove(0, dir))
                    {
                        Move(0, dir);
                        moveDone = true;
                    }
                }
            }
            time = 0;
        }

        if (MoveLeft == 0)
        {
            RandomAttack(map);    
        }
    }

    private void RandomAttack(Map map)
    {
        bool success  = false;
        foreach (SearchStrategy srtategy in searchStrategies)
        {
            success |= srtategy.Search(this, map);
            if (success) 
            {
                break;
            }
        }
        if (!success)
        {
            PassTurn();
        }
    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (Life == 0)
        { 
            onEnemyKill?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
