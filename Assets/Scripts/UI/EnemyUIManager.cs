using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    private Dictionary<Enemy, EnemyUI> enemyUIMap = new Dictionary<Enemy, EnemyUI>();
    private Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Enemy.onEnemyCreate += OnEnemyCreate;
        Enemy.onEnemyKill += OnEnemyKill;
    }

    private void OnDestroy()
    {
        Enemy.onEnemyCreate -= OnEnemyCreate;
        Enemy.onEnemyKill -= OnEnemyKill;
    }

    private void OnEnemyCreate(Enemy enemy, string name)
    {
        GameObject prefab = null;
        if (loadedPrefabs.ContainsKey(name))
        {
            prefab = loadedPrefabs[name];
        }
        else
        {
            prefab = Resources.Load<GameObject>(string.Format("{0}UI", name));
            loadedPrefabs.Add(name, prefab);
        }

        GameObject go = Instantiate(prefab, transform);
        EnemyUI enemyUI = go.GetComponent<EnemyUI>();
        enemyUI.SetEnemy(enemy);
        enemyUIMap.Add(enemy, enemyUI);
    }

    private void OnEnemyKill(Enemy enemy)
    {
        EnemyUI enemyUI = enemyUIMap[enemy];
        enemyUI.gameObject.SetActive(false);
    }

}
