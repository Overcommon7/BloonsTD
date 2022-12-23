using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DartMonkey
{
    const int Pierce = 2;
    const float RangeIncrease = 2f;
    const int Price = 250;
    const float AttackSpeed = 0.5f;

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Piercing Darts", 210), new Upgrade("Long Range Darts", 100) };

    public static void Awake(ref TowerVariables values)
    {
        values.sellPrice = (int)(Price * 0.8f);
        values.range = 3.5f;
        values.timeSinceLastShot = 0;
    }
    public static void Update(ref TowerVariables values, Transform transform)
    {
        values.timeSinceLastShot += Time.deltaTime;
        if (AttackSpeed > values.timeSinceLastShot) return;

        var target = Tower.GetTargetBloon(ref values, transform);
        if (target == null)
        {
            return;
        }

        transform.right = target.transform.position - transform.position;
        var projectile = Object.Instantiate(values.projectile, transform);
        projectile.gameObject.SetActive(true);
        projectile.transform.right = transform.right;
        values.timeSinceLastShot = 0;
    }
    public static void ApplyUpgrade(int upgradeIdx, ref TowerVariables values)
    {
        if (!Upgrade.PreUpgrade(upgradeIdx)) return;
        if (upgradeIdx == 0)
        {
            values.range = RangeIncrease;
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price) * 0.8f);
        }
    }
}
