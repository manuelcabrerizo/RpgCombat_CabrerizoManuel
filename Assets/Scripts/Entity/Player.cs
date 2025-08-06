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

    public override void OnTurnBegin()
    {
        base.OnTurnBegin();

        InputManager.onMoveUp += OnMoveUp;
        InputManager.onMoveDown += OnMoveDown;
        InputManager.onMoveLeft += OnMoveLeft;
        InputManager.onMoveRight += OnMoveRight;

        UpdateTargets();
        
        onPlayerTurnBegin.Invoke(this);
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        InputManager.onMoveUp -= OnMoveUp;
        InputManager.onMoveDown -= OnMoveDown;
        InputManager.onMoveLeft -= OnMoveLeft;
        InputManager.onMoveRight -= OnMoveRight;

        onPlayerTurnEnd?.Invoke(this);
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

    private void OnMoveUp()
    {
        MoveTo(0, 1);
    }

    private void OnMoveDown()
    {
        MoveTo(0, -1);
    }

    private void OnMoveLeft()
    {
        MoveTo(-1, 0);
    }

    private void OnMoveRight() 
    {
        MoveTo(1, 0);
    }

    private void MoveTo(int x, int y)
    {
        if (HaveMovements() && Move(x, y))
        {
            UpdateTargets();
        }
    }

    private void UpdateTargets()
    {
        foreach (var strategy in searchStrategies)
        {
            strategy.Search(this, map);
        }
    }
}