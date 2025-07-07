using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static event Action<Player, string> onPlayerCreate;
    public static event Action<Player> onPlayerTurnBegin;
    public static event Action<Player> onPlayerTurnEnd;
    public static event Action<Player> onPlayerKill;

    private List<SearchStrategy> searchStrategies = new List<SearchStrategy>();

    protected override void OnStart()
    {
        onPlayerCreate.Invoke(this, Name);
        searchStrategies.Add(new PlayerHealSearchStrategy());
        searchStrategies.Add(new PlayerMeleeSearchStrategy());
        searchStrategies.Add(new PlayerRangeSearchStrategy());
    }

    public override void OnTurnBegin(Map map)
    {
        base.OnTurnBegin(map);
        foreach (var strategy in searchStrategies)
        {
            strategy.Search(this, map);
        }
        onPlayerTurnBegin.Invoke(this);
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        onPlayerTurnEnd?.Invoke(this);
    }

    public override void ProcessTurn(Map map)
    {
        bool moveWasValid = false;
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveWasValid |= Move(0, 1, map);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            moveWasValid |= Move(0, -1, map);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            moveWasValid |= Move(-1, 0, map);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            moveWasValid |= Move(1, 0, map);
        }

        if (moveWasValid)
        {
            foreach (var strategy in searchStrategies)
            {
                strategy.Search(this, map);
            }
        }
    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (Life == 0)
        {
            onPlayerKill?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}