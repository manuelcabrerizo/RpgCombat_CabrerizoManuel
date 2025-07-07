using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lifeText;
    private Enemy enemy = null;

    private void Awake()
    {
        Entity.onEntityLifeChange += OnEntityLifeChange;
    }

    private void OnDestroy()
    {
        Entity.onEntityLifeChange -= OnEntityLifeChange;

    }

    private void OnEntityLifeChange(Entity entity)
    {
        if (this.enemy != entity)
        {
            return;
        }
        lifeText.text = string.Format("HP: {0}", entity.Life);
    }

    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        lifeText.text = string.Format("HP: {0}", enemy.Life);
    }
}
