using System.Linq;
using UnityEngine;

public static class Tack
{
    public static void Start(Projectile projectile, ref TackValues values)
    {
        values = new TackValues();
        values.tacks = projectile.GetComponentsInChildren<Transform>(true).ToList();
        values.tacks.RemoveAt(0);
    }
    public static void Update(Projectile projectile, ref TackValues values)
    {
        if (Vector2.Distance(values.tacks[0].position, projectile.transform.parent.position) > projectile.owner.TowerVariables.range * 0.5f)
             Object.Destroy(projectile.gameObject);

        foreach (var tack in values.tacks)
        {
            tack.Translate(projectile.ProjectileValues.speed * Time.deltaTime, 0, 0, Space.Self);
        }
    }

    public static void OnTriggerEnter(Bloon bloon, Transform tack, Projectile projectile)
    {
        projectile.TackValues.tacks.Remove(tack);
        
        Object.Destroy(tack.gameObject);
        if (!bloon.IsFrozen) bloon.Pop();
    }
}
