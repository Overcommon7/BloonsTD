using MyBox;
using MyBox.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

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

    public void SetCustomSpawnValues(ref BloonValues values)
    {
        //float value = (BloonManager.Instance.BloonSpeeds[bloonType].speed / BloonManager.Instance.BloonSpeeds[values.bloonType].speed);
       // Debug.Log(value);
        Debug.Log(values.begin);
        begin = values.begin;
        timeSinceSpawn = values.timeSinceSpawn;
        currentWaypoint = values.currentWaypoint;
        distanceTraveled = values.distanceTraveled;
        distanceTraveled -= values.distanceTraveled * (BloonManager.Instance.BloonSpeeds[bloonType].speed / BloonManager.Instance.BloonSpeeds[values.bloonType].speed);

        Debug.Log((Time.time - begin) /
           (GameManager.Instance.Waypoints[currentWaypoint].speed * BloonManager.Instance.BloonSpeeds[bloonType].time));
    }

    public override string ToString()
    {
        return begin.ToString() + " " + timeSinceSpawn.ToString() + " " + distanceTraveled.ToString() + " " + currentWaypoint.ToString();
    }
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
                print(bloonValues.distanceTraveled);
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
            case BloonType.Popped:
                return;
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
                bloonValues.distanceTraveled -=
                    bloonValues.distanceTraveled * (BloonManager.Instance.BloonSpeeds[bloonValues.bloonType].speed / BloonManager.Instance.BloonSpeeds[bloonValues.bloonType - 1].speed);
                bloonValues.bloonType -= 1;
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Popped].transform, transform).position = transform.position;
                break;
            case BloonType.Black:
            case BloonType.White:
                var t = Instantiate(BloonManager.Instance.BloonSprites[BloonType.Yellow].transform);
                t.GetComponent<Bloon>().bloonValues.SetCustomSpawnValues(ref bloonValues);
                t = Instantiate(BloonManager.Instance.BloonSprites[BloonType.Yellow].transform);
                t.GetComponent<Bloon>().bloonValues.SetCustomSpawnValues(ref bloonValues);
                ////print(t.GetComponent<Bloon>().bloonValues.ToString() +  " - " + bloonValues.ToString());
                //print((Time.time - bloonValues.begin) / (GameManager.Instance.Waypoints[bloonValues.currentWaypoint].speed * BloonManager.Instance.BloonSpeeds[bloonValues.bloonType].time));
                //print((Time.time - t.GetComponent<Bloon>().bloonValues.begin) / (GameManager.Instance.Waypoints[t.GetComponent<Bloon>().bloonValues.currentWaypoint].speed * BloonManager.Instance.BloonSpeeds[t.GetComponent<Bloon>().bloonValues.bloonType].time));


                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Popped].transform).position = transform.position;
                Destroy(gameObject);
                break;
        }
        Player.Instance.PlayerValues.Money++;
    }
}
