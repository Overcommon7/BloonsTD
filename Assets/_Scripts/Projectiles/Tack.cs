using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public static class Tack
{
    public static void Start(Projectile projectile, ref TackValues values)
    {
        values = new TackValues();
        values.tacks = projectile.GetComponentsInChildren<Transform>(true).ToList();
        values.tacks.RemoveAt(0);
        values.isValid = new Dictionary<int, bool>();
        foreach (var tack in values.tacks)
            values.isValid.Add(tack.GetInstanceID(), true);

    }
    public static void Update(Projectile projectile, ref TackValues values)
    {
        if (values.tacks.Count == 0)
        {
            Object.Destroy(projectile.owner.gameObject);
            projectile.owner.gameObject.SetActive(false);
            return;
        }

        if (Vector2.Distance(values.tacks[0].position, projectile.transform.parent.position) > projectile.owner.TowerVariables.range * 0.5f)
             Object.Destroy(projectile.gameObject);

        foreach (var tack in values.tacks)
        {
            tack.Translate(projectile.ProjectileValues.speed * Time.deltaTime, 0, 0, Space.Self);
        }
    }

    public static void OnTriggerEnter(Bloon bloon, Transform tack, Projectile projectile)
    {
        if (tack == null) return;
        var id = tack.GetInstanceID();
        if (!projectile.TackValues.isValid[id]) return;
        projectile.TackValues.tacks.Remove(tack);

        projectile.TackValues.isValid[tack.GetInstanceID()] = false;
        Object.Destroy(tack.gameObject);
        if (bloon.IsFrozen) return;

        projectile.owner.PopCount++; 
        bloon.Pop();
    }
}
