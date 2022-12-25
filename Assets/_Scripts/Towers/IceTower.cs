using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IceTower
{
    
    const float RangeIncrease = 2.6f;
    const float AttackSpeed = 1.9f;
    public const float FreezeTime = 1.2f;
    public const float UpgradedFreezeTime = 1.2f;
    public const float Range = 1.9f;
    public const int Price = 850;

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Long Freeze Time", 450), new Upgrade("Wide Freeze Radius", 300) };

    public static void Awake(ref TowerVariables values)
    {
        values.range = Range;
        values.sellPrice = (int)(Price * 0.8f);
    }
    public static void Update(ref TowerVariables values, Transform transform)
    {
        values.timeSinceLastShot += Time.deltaTime;
        if (AttackSpeed > values.timeSinceLastShot) return;

        foreach (var hit in Physics2D.CircleCastAll(transform.position, values.range * 0.5f, Vector2.zero))
        {
            if (hit.transform.CompareTag("Bloons"))
            {
                Object.Instantiate(values.projectile, transform).gameObject.SetActive(true);
                values.timeSinceLastShot = 0;
                SoundManager.Instance.PlaySound(Sounds.Ice);
                return;
            }
        }
    }
    public static void ApplyUpgrade(int upgradeIdx, ref TowerVariables values)
    {
        if (!Upgrade.PreUpgrade(upgradeIdx)) return;
        if (upgradeIdx == 1)
        {
            values.range = RangeIncrease;
            values.projectile.transform.SetLossyScale(new Vector3(values.range, values.range, 1));
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[0] ? Upgrades[0].price : 0)) * 0.8f);
        }
        else
        {
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[1] ? Upgrades[1].price : 0)) * 0.8f);
        }
        SoundManager.Instance.PlaySound(Sounds.LevelUp);
    }
}
