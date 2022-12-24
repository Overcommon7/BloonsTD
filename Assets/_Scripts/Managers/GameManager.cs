using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Waypoint
{
    public Waypoint(Vector2 position, float distance)
    {
        this.position = position;
        this.distance = distance;
    }

    public Vector2 position;
    public float distance;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Transform waypoint;
    public Transform RangeCircle;
    List<Waypoint> waypoints = new List<Waypoint>();
    float distance = 0; 
    public float Distance { get => distance; }
    public List<Waypoint> Waypoints { get => waypoints; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        distance = Vector2.Distance(waypoint.GetChild(1).position, waypoint.GetChild(0).position);
        for (int i = 0; i < waypoint.childCount - 1; i++)
            waypoints.Add(new Waypoint(waypoint.GetChild(i).position, Vector2.Distance(waypoint.GetChild(i + 1).position, waypoint.GetChild(i).position)));
    }

    public void Start()
    {
        RangeCircle.gameObject.SetActive(false);
        foreach (var obj in GameObject.FindGameObjectsWithTag("EditorOnly"))
            obj.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnApplicationQuit()
    {
        Instance = null;
    }
}
