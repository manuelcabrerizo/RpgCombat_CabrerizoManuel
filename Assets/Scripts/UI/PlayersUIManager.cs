using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayersUIManager : MonoBehaviour
{
    private Dictionary<Player, PlayerUI> playerUIMap = new Dictionary<Player, PlayerUI>();
    private Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Player.onPlayerCreate += OnPlayerCreate;
        Player.onPlayerKill += OnPlayerKill;
    }

    private void OnDestroy()
    {
        Player.onPlayerCreate -= OnPlayerCreate;
        Player.onPlayerKill -= OnPlayerKill;
    }

    private void OnPlayerCreate(Player player, string name)
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
        PlayerUI playerUI = go.GetComponent<PlayerUI>();
        playerUI.SetPlayer(player);
        playerUIMap.Add(player, playerUI);
    }

    private void OnPlayerKill(Player player)
    {
        PlayerUI playerUI = playerUIMap[player];
        playerUI.gameObject.SetActive(false);
    }
}
