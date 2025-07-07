using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private Button[] healButtons;
    [SerializeField] private Button[] rangeAttackButtons;
    [SerializeField] private Button[] meleeAttackButtons;
    [SerializeField] private GameObject border;
    private Player player = null;

    private void Awake()
    {
        Entity.onEntityLifeChange += OnEntityLifeChange;
        Player.onPlayerTurnBegin += OnTurnBegin;
        Player.onPlayerTurnEnd += OnTurnEnd;
        SearchStrategy.onEntitiesToHealChange += OnEntitiesToHealChange;
        SearchStrategy.onEntitiesToRangeAttackChange += OnEntitiesToRangeAttackChange;
        SearchStrategy.onEntitiesToMeleeAttackChange += OnEntitiesToMeleeAttackChange;
        SetAllButtonsActive(false);
    }

    private void OnDestroy()
    {
        Entity.onEntityLifeChange -= OnEntityLifeChange;
        Player.onPlayerTurnBegin -= OnTurnBegin;
        Player.onPlayerTurnEnd -= OnTurnEnd;
        SearchStrategy.onEntitiesToHealChange -= OnEntitiesToHealChange;
        SearchStrategy.onEntitiesToRangeAttackChange -= OnEntitiesToRangeAttackChange;
        SearchStrategy.onEntitiesToMeleeAttackChange -= OnEntitiesToMeleeAttackChange;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        lifeText.text = "HP: " + player.Life;
    }

    public void OnTurnBegin(Player player)
    {
        if (this.player != player)
        {
            return;
        }
        border.SetActive(true);
    }

    public void OnTurnEnd(Player player) 
    {
        if (this.player != player)
        {
            return;
        }
        RestartButtons(healButtons);
        RestartButtons(rangeAttackButtons);
        RestartButtons(meleeAttackButtons);
        SetAllButtonsActive(false);
        border.SetActive(false);
    }

    public void OnEntityLifeChange(Entity entity)
    {
        if (this.player != entity)
        {
            return;
        }
        lifeText.text = string.Format("HP: {0}", entity.Life);
    }

    public void OnEntitiesToHealChange(Player player, List<Entity> entities)
    {
        if (this.player != player)
        {
            return;
        }

        RestartButtons(healButtons);
        int healButtonsUsed = 0;
        foreach (Entity entity in entities)
        {
            Button button = healButtons[healButtonsUsed++];
            button.gameObject.SetActive(true);
            button.image.sprite = entity.Sprite;
            button.onClick.AddListener(() =>
            {
                player.Heal(entity);
            });
        }
    }

    public void OnEntitiesToRangeAttackChange(Player player, List<Entity> entities)
    {
        if (this.player != player)
        {
            return;
        }

        RestartButtons(rangeAttackButtons);
        int rangeAttackButtonsUsed = 0;
        foreach (Entity entity in entities)
        {
            Button button = rangeAttackButtons[rangeAttackButtonsUsed++];
            button.gameObject.SetActive(true);
            button.image.sprite = entity.Sprite;
            button.onClick.AddListener(() =>
            {
                player.RangeAttack(entity);
            });
        }
    }

    public void OnEntitiesToMeleeAttackChange(Player player, List<Entity> entities)
    {
        if (this.player != player)
        {
            return;
        }

        RestartButtons(meleeAttackButtons);
        int meleeAttackButtonsUsed = 0;
        foreach (Entity entity in entities)
        {
            Button button = meleeAttackButtons[meleeAttackButtonsUsed++];
            button.gameObject.SetActive(true);
            button.image.sprite = entity.Sprite;
            button.onClick.AddListener(() =>
            {
                player.MeleeAttack(entity);
            });
        }
    }

    private void RestartButtons(Button[] buttons)
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

    private void SetAllButtonsActive(bool value)
    {
        foreach (Button button in healButtons)
        { 
            button.gameObject.SetActive(value);
        }
        foreach (Button button in rangeAttackButtons)
        {
            button.gameObject.SetActive(value);
        }
        foreach (Button button in meleeAttackButtons)
        {
            button.gameObject.SetActive(value);
        }
    }
}
