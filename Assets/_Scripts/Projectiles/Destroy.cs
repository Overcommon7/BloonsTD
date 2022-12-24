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
        foreach (var hit in Physics2D.BoxCastAll(transform.position, GetComponent<SpriteRenderer>().sprite.bounds.size, 0, Vector2.zero))
            if (hit.transform.CompareTag("Bloons"))
            {
                Bloon bloon = hit.transform.GetComponent<Bloon>();
                if (bloon.Type != BloonType.Black)
                    bloon.Pop();
            }  
    }

    public void ResetSpeed()
    {
        GetComponent<Projectile>().ProjectileValues.speed = 0;
    }
}
