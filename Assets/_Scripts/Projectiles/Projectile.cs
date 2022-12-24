using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ProjectileType
{
    Dart,
    Ice,
    Bomb,
    Tack,
    Laser
}

[Serializable]
public struct ProjectileValues
{
    public int pierce;
    public int speed;
}

public struct TackValues
{
    public List<Transform> tacks;
}
public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileType projectileType = ProjectileType.Dart;
    [MyBox.ReadOnly] public Tower owner;
    public ProjectileValues ProjectileValues = new ProjectileValues();
    TackValues tackValues;
    public ProjectileType ProjectileType { get => projectileType; }
    public TackValues TackValues { get => tackValues; }

    public void Awake()
    {
        if (projectileType.Equals(ProjectileType.Bomb))
            GetComponent<Animator>().enabled = false;
    }
    public void Start()
    {
        owner = GetComponentInParent<Tower>();
        switch (projectileType)
        {
            case ProjectileType.Ice:
                break;
            case ProjectileType.Tack:
                Tack.Start(this, ref tackValues); 
                break;
        }
    }
    public void Update()
    {
        switch (projectileType)
        {
            case ProjectileType.Dart:
                Dart.Update(this);
                break;
            case ProjectileType.Ice:
                Ice.Update(this);
                break;
            case ProjectileType.Bomb:
                Bomb.Update(this);
                break;
            case ProjectileType.Tack:
                Tack.Update(this, ref tackValues);
                break;
            case ProjectileType.Laser:
                Laser.Update(this);
                break;
        }

        if (Mathf.Abs(transform.position.x) > 10 || Mathf.Abs(transform.position.y) > 10)
            Destroy(gameObject);
    }
}
