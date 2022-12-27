using MyBox;
using MyBox.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float timer;
    public int currentWaypoint;
    public float distanceTraveled;
    public float freezeTime;

    public void SetSpawnVariables(ref BloonValues values, int offset = 0)
    {
        BloonType type = bloonType;
        if (values.bloonType == BloonType.Black || values.bloonType == BloonType.White)
            type = BloonType.Yellow;

        timer = values.timer * BloonManager.Instance.BloonSpeeds[values.bloonType].speed / BloonManager.Instance.BloonSpeeds[type].speed;
        currentWaypoint = values.currentWaypoint;

        if (offset == 0) return;
        else if (offset == 1) timer -= 0.05f;
        else if (offset == -1) timer += 0.05f;
    }

    public override string ToString()
    {
        return timer.ToString() + " " + distanceTraveled.ToString() + " " + currentWaypoint.ToString();
    }
}
public class Bloon : MonoBehaviour
{
    [SerializeField] BloonValues bloonValues = new BloonValues();
    public bool IsFrozen { get => bloonValues.isFrozen; }
    public float DistanceTraveled { get => bloonValues.distanceTraveled; }
    public BloonType Type { get => bloonValues.bloonType; }
    static readonly Color frozenColor = new Color(0.8f, 0.8f, 0.8f, 1);

    private void Start()
    {
        transform.position = transform.position.SetY(GameManager.Instance.Waypoints[0].position.y);
        RoundManager.Instance.Bloons++;
    }
    void Update()
    {
        if (bloonValues.isFrozen)
        {
            if (Time.time < bloonValues.freezeTime)
                return;
            bloonValues.isFrozen = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (transform.position.x < GameManager.Instance.Waypoints[0].position.x)
        {
            transform.Translate(BloonManager.Instance.BloonSpeeds[bloonValues.bloonType].speed * Time.deltaTime, 0, 0);
            return;
        }
           

        bloonValues.timer += Time.deltaTime;
        float v = bloonValues.timer * BloonManager.Instance.BloonSpeeds[bloonValues.bloonType].speed;
        float fraction = v / GameManager.Instance.Waypoints[bloonValues.currentWaypoint].distance;

        if (fraction >= 1f)
        {
            ++bloonValues.currentWaypoint;
            if (bloonValues.currentWaypoint >= GameManager.Instance.Waypoints.Count - 1)
            {
                int value = (int)bloonValues.bloonType - (bloonValues.bloonType == BloonType.White ? 1 : 0);
                Player.Instance.PlayerValues.Lives -= value;
                Destroy(gameObject);
            }
            bloonValues.timer = 0;
            return;
        }

        transform.position = Vector2.Lerp(GameManager.Instance.Waypoints[bloonValues.currentWaypoint].position,
                                          GameManager.Instance.Waypoints[bloonValues.currentWaypoint + 1].position,
                                          fraction);

        bloonValues.distanceTraveled = 
            GameManager.Instance.Waypoints[bloonValues.currentWaypoint].combinedDistance + 
            Vector2.Distance(transform.position, GameManager.Instance.Waypoints[bloonValues.currentWaypoint].position);
                                       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Projectiles")) return;

        var projectile = collision.transform.parent.GetComponentInChildren<Projectile>();

        switch (projectile.ProjectileType)
        {                                                                                              
            case ProjectileType.Dart:
                Dart.OnTriggerEnter(this, projectile);
                break;
            case ProjectileType.Bomb:
                Bomb.OnTriggerEnter(this, projectile);
                break;
            case ProjectileType.Tack:
                Tack.OnTriggerEnter(this, collision.transform, projectile);
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
                GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().sprite.bounds.size;
                var temp = bloonValues;
                bloonValues.bloonType -= 1;
                bloonValues.SetSpawnVariables(ref temp);               
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Popped].transform, transform);
                break;
            case BloonType.Black:
            case BloonType.White:
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Yellow].transform).GetComponent<Bloon>().bloonValues.SetSpawnVariables(ref bloonValues, 1);
                Instantiate(BloonManager.Instance.BloonSprites[BloonType.Yellow].transform).GetComponent<Bloon>().bloonValues.SetSpawnVariables(ref bloonValues, -1);
                Destroy(gameObject);
                break;
        }
        Player.Instance.PlayerValues.Money++;
        SoundManager.Instance.PlaySound(Sounds.Pop);
    }

    public void Freeze(bool upgraded)
    {
        bloonValues.isFrozen = true;
        GetComponent<SpriteRenderer>().color = frozenColor;
        bloonValues.freezeTime = Time.time + (upgraded ? IceTower.UpgradedFreezeTime : IceTower.FreezeTime);
    }

    private void OnDestroy()
    {
        RoundManager.Instance.Bloons--;
    }
}
