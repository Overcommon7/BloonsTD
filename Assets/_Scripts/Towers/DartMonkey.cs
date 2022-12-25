using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DartMonkey
{
    const int Pierce = 2;
    const float RangeIncrease = 4.5f;
    public const float Range = 3.5f;
    public const int Price = 250;
    const float AttackSpeed = 0.75f;

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Piercing Darts", 210), new Upgrade("Long Range Darts", 100) };

    public static void Awake(ref TowerVariables values)
    {
        values.sellPrice = (int)(Price * 0.8f);
        values.range = Range;
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
        if (upgradeIdx == 1)
        {
            values.range = RangeIncrease;
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[0] ? Upgrades[0].price : 0)) * 0.8f);
        }
        else
        {
            values.projectile.ProjectileValues.pierce = Pierce;
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[1] ? Upgrades[1].price : 0)) * 0.8f);
        }
        SoundManager.Instance.PlaySound(Sounds.LevelUp);
    }
}
