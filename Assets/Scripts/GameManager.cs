using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<string> onGameEnd;

    [SerializeField] private Transform mapTransform;

    private Map map;
    private int currentTurn = 0;
    private List<Entity> entities = new List<Entity>();
    private int playerAliveCount = 0;
    private int playerKillCount = 0;
    private int enemyAliveCount = 0;
    private int enemyKillCount = 0;


    private void Awake()
    {
        Entity.onEntityCreate += OnEntityCreate;
        Player.onPlayerCreate += OnPlayerCreate;
        Player.onPlayerKill += OnPlayerKill;
        Enemy.onEnemyCreate += OnEnemyCreate;
        Enemy.onEnemyKill += OnEnemyKill;

        currentTurn = 0;
        playerAliveCount = 0;
        enemyAliveCount = 0;

        // Create the map
        map = new Map(6, 4, "grid_cell_0", mapTransform);
        // Create the players
        Instantiate(Resources.Load<GameObject>("player1"));
        Instantiate(Resources.Load<GameObject>("player2"));
        Instantiate(Resources.Load<GameObject>("player3"));
        // Create the enemies
        Instantiate(Resources.Load<GameObject>("enemy1"));
        Instantiate(Resources.Load<GameObject>("enemy2"));
    }

    private void OnDestroy()
    {
        Entity.onEntityCreate -= OnEntityCreate;
        Player.onPlayerKill -= OnPlayerKill;
        Enemy.onEnemyKill -= OnEnemyKill;
    }

    private void Update()
    {
        Entity entity = entities[currentTurn];
        while (!entity.IsAlive())
        {
            currentTurn = (currentTurn + 1) % entities.Count;
            entity = entities[currentTurn];
        }

        if (!entity.TurnInitialize)
        {
            entity.OnTurnBegin(map);
        }

        if (entity.HaveMovements() && !entity.ActionPerformed())
        {
            entity.ProcessTurn(map);
        }
        else if (entity.ActionPerformed())
        {
            entity.OnTurnEnd();
            currentTurn = (currentTurn + 1) % entities.Count;
        }

        if (playerKillCount > 0 && enemyKillCount == 0)
        {
            onGameEnd?.Invoke("Game Over");
        }
        if ((enemyAliveCount - enemyKillCount) == 0 &&
            (playerAliveCount - playerKillCount) == 1)
        {
            foreach (Entity winner in entities)
            {
                if (winner.IsAlive())
                {
                    onGameEnd?.Invoke(string.Format("{0} Fue el ganador", winner.Name));
                }
            }
        }
    }

    private void OnEntityCreate(Entity entity)
    {
        entity.Initialize(map);
        entities.Add(entity);
    }

    private void OnPlayerCreate(Player player, string name)
    {
        playerAliveCount++;
    }

    private void OnEnemyCreate(Enemy enemy, string name)
    {
        enemyAliveCount++;
    }

    private void OnPlayerKill(Player player)
    {
        playerKillCount++;
    }

    private void OnEnemyKill(Enemy enemy)
    { 
        enemyKillCount++;
    }
}
