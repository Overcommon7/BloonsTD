using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum TowerType
{
    DartTower,
    TackShooter,
    IceTower,
    BombTower,
    SuperMonkey
}

public struct Upgrade
{
    public Upgrade(string name, int price)
    {
        this.name = name;
        this.price = price;
    }

    public string name;
    public int price;

    public static bool PreUpgrade(int upgradeIdx)
    {
        int price = UpgradePanel.Instance.SelectedTower.Upgrades[upgradeIdx].price;
        if (Player.Instance.PlayerValues.Money < price) return false;
        Player.Instance.PlayerValues.Money -= price;
        UpgradePanel.Instance.SelectedTower.TowerVariables.upgraded[upgradeIdx] = true;
        return true; 
    }
}

[Serializable]
public struct TowerVariables
{
    [ReadOnly] public Projectile projectile;

    [ReadOnly] public int sellPrice;
    [ReadOnly] public int popCount;
    [ReadOnly] public float attackSpeed;
    [ReadOnly] public float range;

    public bool[] upgraded;

    [NonSerialized] public float timeSinceLastShot;
}

public class Tower : MonoBehaviour
{
    [SerializeField] TowerType towerType = TowerType.DartTower;
    [SerializeField] TowerVariables towerVariables = new TowerVariables();

    public TowerType TowerType { get => towerType; }
    public TowerVariables TowerVariables { get => towerVariables; }
    public Upgrade[] Upgrades
    {
        get
        {
            switch (towerType)
            {
                case TowerType.DartTower:
                    return DartMonkey.Upgrades;
                case TowerType.TackShooter:
                    return TackShooter.Upgrades;
                case TowerType.IceTower:
                    return IceTower.Upgrades;
                case TowerType.BombTower:
                    return BombShooter.Upgrades;
                case TowerType.SuperMonkey:
                    return SuperMonkey.Upgrades;
                default: 
                    return new Upgrade[0];
            }
        }
    }
    void Awake()
    {
        towerVariables.upgraded = new bool[2] { false, false };
        towerVariables.projectile = GetComponentInChildren<Projectile>();
        towerVariables.projectile.gameObject.SetActive(false);
        switch (towerType)
        {
            case TowerType.DartTower:
                DartMonkey.Awake(ref towerVariables);
                break;
            case TowerType.TackShooter:
                TackShooter.Awake(ref towerVariables);
                break;
            case TowerType.IceTower:
                IceTower.Awake(ref towerVariables);
                break;
            case TowerType.BombTower:
                BombShooter.Awake(ref towerVariables);
                break;
            case TowerType.SuperMonkey:
                SuperMonkey.Awake(ref towerVariables);
                break;
        }
    }

    void Update()
    {
        switch (towerType)
        {
            case TowerType.DartTower:
                DartMonkey.Update(ref towerVariables, transform);
                break;
            case TowerType.TackShooter:
                TackShooter.Update(ref towerVariables);
                break;
            case TowerType.IceTower:
                IceTower.Update(ref towerVariables);
                break;
            case TowerType.BombTower:
                BombShooter.Update(ref towerVariables);
                break;
            case TowerType.SuperMonkey:
                SuperMonkey.Update(ref towerVariables);
                break;
        }
    }

    public void ApplyUpgrade(int upgradeIdx)
    {
        switch (towerType)
        {
            case TowerType.DartTower:
                DartMonkey.ApplyUpgrade(upgradeIdx, ref towerVariables);
                break;
            case TowerType.TackShooter:
                TackShooter.ApplyUpgrade(upgradeIdx, ref towerVariables);
                break;
            case TowerType.IceTower:
                IceTower.ApplyUpgrade(upgradeIdx, ref towerVariables);
                break;
            case TowerType.BombTower:
                BombShooter.ApplyUpgrade(upgradeIdx, ref towerVariables);
                break;
            case TowerType.SuperMonkey:
                SuperMonkey.ApplyUpgrade(upgradeIdx, ref towerVariables);
                break;
        }
    }

#nullable enable
    public static Bloon? GetTargetBloon(ref TowerVariables values, Transform transform)
    {
        Bloon? target = null;
        float maxDistance = 0;
        foreach (var hit in Physics2D.CircleCastAll(transform.position, values.range * 0.5f, Vector2.zero))
        {
            Bloon? bloon = hit.transform.GetComponent<Bloon>();
            if (bloon == null) continue;
            if (maxDistance > bloon.DistanceTraveled) continue;
            target = bloon;
            maxDistance = bloon.DistanceTraveled;
        }
        return target;
    }
#nullable disable
}