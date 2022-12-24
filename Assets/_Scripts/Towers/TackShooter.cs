using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class TackShooter
{
    const float rangeIncrease = 2.9f;
    const float attackSpeed = 1.5f;
    const float upgradedAttackSpeed = 0.6f;
    public const float Range = 2f;
    public const int Price = 400;
     

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Faster Shooting", 250), new Upgrade("Extra Range Tacks", 150) };

    public static void Awake(ref TowerVariables values)
    {
        values.range = Range;
        values.sellPrice = (int)(Price * 0.8f);
    }
    public static void Update(ref TowerVariables values, Transform transform)
    {
        values.timeSinceLastShot += Time.deltaTime;
        if ((values.upgraded[0] ? upgradedAttackSpeed : attackSpeed) > values.timeSinceLastShot) return;

        foreach (var hit in Physics2D.CircleCastAll(transform.position, values.range * 0.5f, Vector2.zero))
        {
            if (hit.transform.CompareTag("Bloons"))
            {
                Object.Instantiate(values.projectile, transform).gameObject.SetActive(true);
                values.timeSinceLastShot = 0;
                return;
            }
        }
    }
    public static void ApplyUpgrade(int upgradeIdx, ref TowerVariables values)
    {
        if (!Upgrade.PreUpgrade(upgradeIdx)) return;
        if (upgradeIdx == 1)
        {
            values.range = rangeIncrease;
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[0] ? Upgrades[0].price : 0)) * 0.8f);
        }
        else
        {
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[1] ? Upgrades[1].price : 0)) * 0.8f);
        }
    }
}
