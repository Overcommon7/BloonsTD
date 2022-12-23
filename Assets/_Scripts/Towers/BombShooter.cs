using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BombShooter
{
    const float BombScale = 1.4f;
    const float speedIncrease = 1.5f;
    const int Price = 900;

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Bigger Bombs", 650), new Upgrade("Extra Range Bombs", 250) };

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
