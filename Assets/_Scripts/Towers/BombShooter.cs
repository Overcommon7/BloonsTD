using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BombShooter
{
    const float BombScale = 1.4f;
    const float RangeIncrease = 4.25f;
    const float AttackSpeed = 1.5f;
    public const float Range = 3f;
    public const int Price = 900;

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Bigger Bombs", 650), new Upgrade("Extra Range Bombs", 250) };

    public static void Awake(ref TowerVariables values)
    {
        values.range = Range;
        values.sellPrice = (int)(Price * 0.8f);
    }
    public static void Update(ref TowerVariables values, Transform transform)
    {
        values.timeSinceLastShot += Time.deltaTime;
        if (AttackSpeed > values.timeSinceLastShot) return;

        var target = Tower.GetTargetBloon(ref values, transform);
        if (target == null) return;

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
            values.projectile.transform.SetLossyScale(values.projectile.transform.lossyScale * BombScale);
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[1] ? Upgrades[1].price : 0)) * 0.8f);
        }
        SoundManager.Instance.PlaySound(Sounds.LevelUp);
    }
}
