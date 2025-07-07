using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Entity : MonoBehaviour
{
    public static event Action<Entity> onEntityCreate;
    public static event Action<Entity> onEntityLifeChange;
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    [field:SerializeField] public EntityData Data { get; private set; }
    public int Life { get; private set; }
    public int MoveLeft { get; private set; }
    public bool TurnInitialize { get; private set; }
    private bool actionPerformed;

    private void Awake()
    {
        Life = Data.MaxLife;
        MoveLeft = Data.Velocity;
        TurnInitialize = false;
    }

    private void Start()
    {
        onEntityCreate?.Invoke(this);
        OnStart();
    }

    protected abstract void OnStart();

    public void Initialize(Map map)
    {
        Tile tile = map.GetRandomEmptyTile();
        tile.Entity = this;
        transform.SetParent(tile.Go.transform);
        transform.localPosition = Vector3.zero;
        SetTile(tile.X, tile.Y);
    }

    public abstract void ProcessTurn(Map map);

    public void SetTile(int x, int y)
    {
        X = x;
        Y = y;
    }

    protected virtual void TakeDamage(int damage)
    {
        Life = Math.Max(Life - damage, 0);
        onEntityLifeChange?.Invoke(this);
    }

    public void Heal(Entity target)
    {
        Debug.Log(Name + " Heal " + target.Name);
        target.Life += Data.HealAmount;
        onEntityLifeChange?.Invoke(target);
        actionPerformed = true;
    }

    public void RangeAttack(Entity target)
    {
        Debug.Log(Name + " Range Attack " + target.Name);
        target.TakeDamage(Data.RangeAttack);
        actionPerformed = true;
    }

    public void MeleeAttack(Entity target)
    {
        Debug.Log(Name + " Melee Attack " + target.Name);
        target.TakeDamage(Data.MeleAttack);
        actionPerformed = true;
    }

    public void PassTurn()
    {
        Debug.Log(Name + " Pass Turn ");
        actionPerformed = true;
    }


    public virtual void OnTurnBegin(Map map)
    {
        TurnInitialize = true;
    }
    public virtual void OnTurnEnd()
    {
        MoveLeft = Data.Velocity;
        actionPerformed = false;
        TurnInitialize = false;
    }

    public bool IsValidMove(int xMovement, int yMovement, Map map)
    {
        int newX = X + xMovement;
        int newY = Y + yMovement;
        bool isInRange = (newX >= 0 && newX < map.GetWidth() && newY >= 0 && newY < map.GetHeight());
        if(!isInRange) { return false; }
        Tile newTile = map.GetTiles(newX, newY);
        return newTile.IsEmpty();
    }

    public bool Move(int xMovement, int yMovement, Map map)
    {
        int newX = X + xMovement;
        int newY = Y + yMovement;
        if (IsValidMove(xMovement, yMovement, map))
        {
            Tile newTile = map.GetTiles(newX, newY);
            Tile currentTile = map.GetTiles(X, Y);
            currentTile.Entity = null;
            newTile.Entity = this;

            X = newX;
            Y = newY;
            transform.SetParent(newTile.Go.transform);
            transform.localPosition = Vector3.zero;
            MoveLeft--;
            return true;
        }
        return false;
    }

    public bool HaveMovements()
    {
        return MoveLeft > 0;
    }

    public bool ActionPerformed()
    {
        return actionPerformed;
    }

    public bool IsAlive()
    {
        return Life > 0;
    }
}