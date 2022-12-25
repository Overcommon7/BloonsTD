using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bomb 
{
    public static void Update(Projectile projectile)
    {
        projectile.transform.Translate(projectile.ProjectileValues.speed * Time.deltaTime, 0, 0, Space.Self);
    }

    public static void OnTriggerEnter(Bloon bloon, Projectile projectile)
    {
        projectile.GetComponent<Animator>().enabled = true;
        if (bloon.Type != BloonType.Black)
            bloon.Pop();
        projectile.owner.PopCount++;
    }
}

