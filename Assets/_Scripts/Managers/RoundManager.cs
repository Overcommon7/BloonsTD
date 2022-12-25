using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] TextAsset roundsFile;
    static Vector3 start;
    List<List<string>> rounds = new List<List<string>>() { new List<string>() };
    int currentRound = 0;
    int instructionIndex = 0;
    bool roundPlaying;
    float timer = 0;
    float waitUntil = 0;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);

        int index = 0;
        foreach (var line in roundsFile.text.Split('\n')) 
        {
            if (line == "RoundEnd")   
                index++;
            else rounds[index].Add(line);
        }
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

    private void Update()
    {
        if (!roundPlaying) return;

        var instructions = rounds[currentRound][instructionIndex].Split(' ');

        switch (instructions[0])
        {
            case "s":
                SpawnBloon(instructions[1], int.Parse(instructions[2]), 0f); 
                break;
            case "w":
                break;
        }
    }

    void SpawnBloon(string type, int amount = 1, float spacing = 0)
    {
        switch (type)
        {
            case "red":
                break;
            case "blue":
                break;
            case "green":
                break;
            case "yellow":
                break;
            case "black":
                break;
            case "white":
                break;
        }

        while (amount > 0)
        {
            amount--;

        }
    }

    public void StartRound()
    {
        roundPlaying = true;
        instructionIndex = 0;
    }

    public void OnRoundEnd()
    {

    }
}
