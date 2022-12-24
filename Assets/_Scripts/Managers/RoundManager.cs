using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    [SerializeField] bool red;
    [SerializeField] bool blue;
    [SerializeField] bool green;
    [SerializeField] bool yellow;
    [SerializeField] bool black;
    [SerializeField] bool white;
    static Vector3 start;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        start = GameManager.Instance.Waypoints[0].position;
    }

    private void FixedUpdate()
    {
        if (red) Instantiate(BloonManager.BloonSprites[BloonType.Red].transform).position = start;
        if (blue) Instantiate(BloonManager.BloonSprites[BloonType.Blue].transform).position = start;
        if (green) Instantiate(BloonManager.BloonSprites[BloonType.Green].transform).position = start;
        if (yellow) Instantiate(BloonManager.BloonSprites[BloonType.Yellow].transform).position = start;
        if (white) Instantiate(BloonManager.BloonSprites[BloonType.White].transform).position = start;
        if (black) Instantiate(BloonManager.BloonSprites[BloonType.Black].transform).position = start;


        red = false;
        blue = false;
        green = false;
        yellow = false;
        black = false; 
        white = false;
    }
}
