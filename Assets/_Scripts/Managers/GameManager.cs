using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct Waypoint
{
    public Waypoint(Vector2 position, float distance, float combinedDistance)
    {
        this.position = position;
        this.distance = distance;
        this.combinedDistance = combinedDistance;
    }

    public Vector2 position;
    public float distance;
    public float combinedDistance;
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

        float combinedDistance = 0;
        distance = Vector2.Distance(waypoint.GetChild(1).position, waypoint.GetChild(0).position);
        for (int i = 0; i < waypoint.childCount - 1; i++)
        {
            var d = Vector2.Distance(waypoint.GetChild(i + 1).position, waypoint.GetChild(i).position);
            waypoints.Add(new Waypoint(waypoint.GetChild(i).position, d, combinedDistance));
            combinedDistance += d;
        }
           
    }

    public void Start()
    {
        RangeCircle.gameObject.SetActive(false);
        foreach (var obj in GameObject.FindGameObjectsWithTag("Map"))
            obj.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ReloadScene()
    {
        UIManager.Instance = null;
        BloonManager.Instance = null;
        Player.Instance = null;
        RoundManager.Instance = null;
        SoundManager.Instance = null;

        StatsPanel.Instance = null;
        TowerPanel.Instance = null;
        UpgradePanel.Instance = null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void OnGameOver()
    {
        RoundManager.Instance.ForceRoundEnd();
        UIManager.Instance.OngameOver();
    }

    public void OnWin()
    {
        UIManager.Instance.OnWin();
    }

    private void OnApplicationQuit()
    {
        Instance = null;
    }
}
