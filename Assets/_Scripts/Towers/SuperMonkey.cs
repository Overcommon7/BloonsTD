using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SuperMonkey
{
    static int ProjectileIndex = 0;
    const float RangeIncrease = 2.5f;
    const int Price = 4000;

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Epic Range", 2400), new Upgrade("Laser Vision", 3500) };

    public static void Awake(ref TowerVariables values)
    {

    }
    public static void Update(ref TowerVariables values)
    {

    }
    public static void ApplyUpgrade(int upgradeNum, ref TowerVariables values)
    {

    }
}
