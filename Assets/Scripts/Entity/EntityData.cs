using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EntityData", menuName = "EntityData/Data", order = 1)]

public class EntityData : ScriptableObject
{
    public int MaxLife;
    public int Velocity;
    public int MeleAttack;

    public int RangeAttack;
    public int RangeAttackRadio;

    public int HealAmount;
    public int HealRadio;
}