using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public static class TackShooter
{
    const float rangeIncrease = 1.4f;
    const float speedIncrease = 1.5f;
    const int Price = 400;

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Faster Shooting", 250), new Upgrade("Extra Range Tacks", 150) };

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
