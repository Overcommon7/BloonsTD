using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tack
{
    public static void Update(Projectile projectile)
    {
        if (Vector2.Distance(projectile.transform.position, projectile.transform.parent.position) > projectile.owner.TowerVariables.range)
            Object.Destroy(projectile.gameObject);
    }

    public static void OnTriggerEnter(Bloon bloon, Projectile projectile)
    {

    }
}
