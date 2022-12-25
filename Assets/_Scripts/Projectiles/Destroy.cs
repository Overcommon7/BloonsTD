using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
   public void Remove()
   {
        Object.Destroy(gameObject);
   }

    public void RemoveCollider()
    {
        Object.Destroy(GetComponent<BoxCollider2D>());
    }

    public void ResizeCollider()
    {
        int popCount = 0;
        foreach (var hit in Physics2D.BoxCastAll(transform.position, GetComponent<SpriteRenderer>().sprite.bounds.size, 0, Vector2.zero))
            if (hit.transform.CompareTag("Bloons"))
            {
                Bloon bloon = hit.transform.GetComponent<Bloon>();
                if (bloon.Type != BloonType.Black)
                {
                    popCount++;
                    bloon.Pop();
                }    
            }
        transform.parent.GetComponent<Tower>().PopCount += popCount;
    }

    public void FreezeInRadius()
    {
        var tower = GetComponentInParent<Tower>();
        bool upgraded = tower.TowerVariables.upgraded[0];
        foreach (var hit in Physics2D.CircleCastAll(transform.position, tower.Range * 0.5f, Vector2.zero))
            if (hit.transform.CompareTag("Bloons"))
            {
                Bloon bloon = hit.transform.GetComponent<Bloon>();
                if (bloon.Type != BloonType.White)
                    bloon.Freeze(upgraded);                 
            }
    }

    public void ResetSpeed()
    {
        GetComponent<Projectile>().ProjectileValues.speed = 0;
    }
}
