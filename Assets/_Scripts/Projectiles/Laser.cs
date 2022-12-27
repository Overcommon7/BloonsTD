using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Laser
{
    public static void Update(Projectile projectile)
    {
        projectile.transform.Translate(projectile.ProjectileValues.speed * Time.deltaTime, 0, 0, Space.Self);
    }
    public static void OnTriggerEnter(Bloon bloon, Projectile projectile)
    {
        if (!projectile.ProjectileValues.isValid) return;

        projectile.ProjectileValues.pierce--;
        if (projectile.ProjectileValues.pierce == 0)
        {
            Object.Destroy(projectile.gameObject);
            projectile.ProjectileValues.isValid = false;
        }
           
        bloon.Pop();
        projectile.owner.PopCount++;
    }
}
