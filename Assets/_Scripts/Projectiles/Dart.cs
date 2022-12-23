using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dart
{
    public static void Update(Projectile projectile)
    {
        projectile.transform.Translate(projectile.ProjectileValues.speed * Time.deltaTime, 0, 0, Space.Self);
    }
    public static void OnTriggerEnter(Bloon bloon, Projectile projectile)
    { 
        if (bloon.IsFrozen)
        {
            Object.Destroy(projectile.gameObject);
            return;
        }

        projectile.ProjectileValues.pierce--;
        if (projectile.ProjectileValues.pierce == 0) 
            Object.Destroy(projectile.gameObject);
        bloon.Pop();
    }
}
