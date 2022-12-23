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
public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileType projectileType = ProjectileType.Dart;
    [MyBox.ReadOnly] public Tower owner;
    public ProjectileValues ProjectileValues = new ProjectileValues();
    

    public ProjectileType ProjectileType { get => projectileType; }

    public void Start()
    {
        owner = GetComponentInParent<Tower>();
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
                Bomb.Update(ref ProjectileValues);
                break;
            case ProjectileType.Tack:
                Tack.Update(this);
                break;
            case ProjectileType.Laser:
                Laser.Update(ref ProjectileValues);
                break;
        }
    }
}
