using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<string> onGameEnd;

    [SerializeField] private Transform mapTransform;
    [SerializeField] private AdsManager adsManager;

    private Map map;
    private List<Entity> entities = new List<Entity>();
    private int currentTurn = 0;
    private int playerAliveCount = 0;
    private int playerKillCount = 0;
    private int enemyAliveCount = 0;
    private int enemyKillCount = 0;

    private bool isRunning = false;
    private bool isPaused = false;


    private void Awake()
    {
        Entity.onEntityCreate += OnEntityCreate;
        Player.onPlayerCreate += OnPlayerCreate;
        Player.onPlayerKill += OnPlayerKill;
        Enemy.onEnemyCreate += OnEnemyCreate;
        Enemy.onEnemyKill += OnEnemyKill;
        UIManager.onCreditsPanelShow += OnCreditsPanelShow;

        // Create the new map
        map = new Map(6, 4, "grid_cell_0", mapTransform);
        // Create the players
        Instantiate(Resources.Load<GameObject>("player1"));
        Instantiate(Resources.Load<GameObject>("player2"));
        Instantiate(Resources.Load<GameObject>("player3"));
        // Create the enemies
        Instantiate(Resources.Load<GameObject>("enemy1"));
        Instantiate(Resources.Load<GameObject>("enemy2"));
        // reset stats
        currentTurn = 0;
        playerAliveCount = 0;
        playerKillCount = 0;
        enemyAliveCount = 0;
        enemyKillCount = 0;
        isRunning = true;
        isPaused = false;
    }

    private void OnDestroy()
    {
        Entity.onEntityCreate -= OnEntityCreate;
        Player.onPlayerCreate -= OnPlayerCreate;
        Player.onPlayerKill -= OnPlayerKill;
        Enemy.onEnemyCreate -= OnEnemyCreate;
        Enemy.onEnemyKill -= OnEnemyKill;
        UIManager.onCreditsPanelShow -= OnCreditsPanelShow;
    }

    private void Update()
    {
        if (!isRunning || isPaused)
        {
            return;
        }

        Entity entity = entities[currentTurn];
        while (!entity.IsAlive())
        {
            currentTurn = (currentTurn + 1) % entities.Count;
            entity = entities[currentTurn];
        }

        if (!entity.TurnInitialize)
        {
            entity.OnTurnBegin();
        }

        if (entity.HaveMovements() && !entity.ActionPerformed())
        {
            entity.ProcessTurn();
        }
        else if (entity.ActionPerformed())
        {
            entity.OnTurnEnd();
            currentTurn = (currentTurn + 1) % entities.Count;
        }

        if (playerKillCount > 0 && enemyKillCount == 0)
        {
            OnGameEnd();
            onGameEnd?.Invoke("Game Over");
        }
        if ((enemyAliveCount - enemyKillCount) == 0 &&
            (playerAliveCount - playerKillCount) == 1)
        {
            OnGameEnd();
            foreach (Entity winner in entities)
            {
                if (winner.IsAlive())
                {
                    onGameEnd?.Invoke(string.Format("{0} Fue el ganador", winner.Name));
                }
            }
        }
    }

    private void OnGameEnd()
    {
        adsManager.ShowInterstitial();
        isRunning = false;
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

    private void OnCreditsPanelShow(bool show)
    {
        if (show)
        {
            isPaused = true;
            Time.timeScale = 0.0f;
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1.0f;
        }
    }
}
