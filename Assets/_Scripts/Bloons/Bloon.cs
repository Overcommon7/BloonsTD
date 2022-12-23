using MyBox;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum BloonType
{
    Popped,
    Red,
    Blue,
    Green,
    Yellow,
    Black,
    White
}

public struct BloonVariables
{
    public BloonVariables(float time)
    {
        this.time = time;
        speed = GameManager.Instance.Distance / time;
    }
    public float speed;
    public float time;
}

[Serializable]
public struct BloonValues
{
    public BloonType bloonType;
    public bool isFrozen;
    public float begin;
    public float timeSinceSpawn;
    public int currentWaypoint;
    public float distanceTraveled;
}
public class Bloon : MonoBehaviour
{
    [SerializeField] BloonValues bloonValues = new BloonValues();
                    
    public bool IsFrozen { get => bloonValues.isFrozen; }
    public float DistanceTraveled { get => bloonValues.distanceTraveled; }

    void Start()
    {
        bloonValues.begin = Time.time;
        bloonValues.timeSinceSpawn = bloonValues.begin;
        transform.position = GameManager.Instance.Waypoints[bloonValues.currentWaypoint].position;
    }

    void Update()
    {
        float fraction = 
            (Time.time - bloonValues.begin) / 
            (GameManager.Instance.Waypoints[bloonValues.currentWaypoint].speed * BloonManager.Instance.BloonSpeeds[bloonValues.bloonType].time);

        if (fraction >= 1f)
        {
            ++bloonValues.currentWaypoint;
            if (bloonValues.currentWaypoint >= GameManager.Instance.Waypoints.Count - 1)
            {
                int value = (int)bloonValues.bloonType - (bloonValues.bloonType == BloonType.White ? 1 : 0);
                Player.Instance.PlayerValues.Lives -= value;
                Destroy(gameObject);
            }
            bloonValues.begin = Time.time;
            return;
        }

        transform.position = Vector2.Lerp(GameManager.Instance.Waypoints[bloonValues.currentWaypoint].position,
                                          GameManager.Instance.Waypoints[bloonValues.currentWaypoint + 1].position,
                                          fraction);
        bloonValues.distanceTraveled = BloonManager.Instance.BloonSpeeds[bloonValues.bloonType].speed * (Time.time - bloonValues.timeSinceSpawn);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Projectiles")) return;

        var projectile = collision.GetComponent<Projectile>();
        switch (projectile.ProjectileType)
        {
            case ProjectileType.Dart:
                Dart.OnTriggerEnter(this, projectile);
                break;
            case ProjectileType.Ice:
                Ice.OnTriggerEnter(this, projectile);
                break;
            case ProjectileType.Bomb:
                Bomb.OnTriggerEnter(this, projectile);
                break;
            case ProjectileType.Tack:
                Tack.OnTriggerEnter(this, projectile);
                break;
            case ProjectileType.Laser:
                Laser.OnTriggerEnter(this, projectile);
                break;
        }
    }

    public void Pop()
    {
        switch (bloonValues.bloonType)
        {
            case BloonType.Red:
                bloonValues.bloonType -= 1;
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Popped].transform).position = transform.position;
                Destroy(gameObject);
                break;
            case BloonType.Blue:
            case BloonType.Green:
            case BloonType.Yellow:
                GetComponent<SpriteRenderer>().sprite = BloonManager.Instance.BloonSprites[bloonValues.bloonType - 1].sprite;
                Vector2 S = GetComponent<SpriteRenderer>().sprite.bounds.size;
                GetComponent<BoxCollider2D>().size = S;
                bloonValues.bloonType -= 1;
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Popped].transform, transform).position = transform.position;
                break;
            case BloonType.Black:
            case BloonType.White:
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Yellow].transform).position = transform.position.OffsetX(0.1f);
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Yellow].transform).position = transform.position.OffsetY(0.1f);
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Popped].transform).position = transform.position;
                Destroy(gameObject);
                break;
        }
    }
}
