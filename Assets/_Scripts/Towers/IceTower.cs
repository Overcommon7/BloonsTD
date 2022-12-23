using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IceTower
{
    const float FreezeTime = 1.2f;
    const float RangeIncrease = 1.5f;
    const int Price = 850;

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Long Freeze Time", 450), new Upgrade("Wide Freeze Radius", 300) };

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
