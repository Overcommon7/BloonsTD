using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Waypoint
{
    public Waypoint(Vector2 position, float speed)
    {
        this.position = position;
        this.speed = speed;
    }

    public Vector2 position;
    public float speed;
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

        distance = Vector2.Distance(waypoint.GetChild(0).position, waypoint.GetChild(1).position);
        for (int i = 0; i < waypoint.childCount - 1; i++)
            waypoints.Add(new Waypoint(waypoint.GetChild(i).position, Vector2.Distance(waypoint.GetChild(i + 1).position, waypoint.GetChild(i).position) / distance));
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
