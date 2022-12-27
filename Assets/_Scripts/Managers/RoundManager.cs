using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    //[SerializeField] bool red;
    //[SerializeField] bool blue;
    //[SerializeField] bool green;
    //[SerializeField] bool yellow;
    //[SerializeField] bool black;
    //[SerializeField] bool white;
    //[SerializeField] bool reloadRounds = false;
    //[SerializeField] bool endRound = false;
    //[SerializeField] bool startRound = false;

    [SerializeField] TextAsset roundsFile;
    [SerializeField] TextMeshProUGUI roundNumber;
    static Vector3 start;
    static Vector3 pos;
    List<List<string>> rounds = null;
    Dictionary<BloonType, float> bloonsSpacings = new Dictionary<BloonType, float>();

    [SerializeField] int currentRound = -1;
    [SerializeField] int instructionIndex = 0;
    bool roundPlaying = false;
    float waitUntil = 0;
    int numOfRounds = 0;
    
    [SerializeField] int numOfInstructions = 0;
    public int Bloons = 0;

    int CurrentRound { get => currentRound;  set { currentRound = value; roundNumber.text = (currentRound + 1).ToString(); } }
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

        bloonsSpacings = new Dictionary<BloonType, float>()
        {
            { BloonType.Red, 0.28f },
            { BloonType.Blue, 0.32f },
            { BloonType.Green, 0.36f },
            { BloonType.Yellow, 0.35f },
            { BloonType.Black, 0.231f },
            { BloonType.White, 0.231f }
        };

        ReloadRounds();
    }

    public void ReloadRounds()
    {
        int index = 0;
        rounds = new List<List<string>>() { new List<string>() };
        foreach (var instruction in roundsFile.text.Split('\n'))
        {
            var line = instruction.Replace("\r", "");
            if (line.Contains("//")) break;
            if (line.Contains("RoundEnd"))
            {
                index++;
                rounds.Add(new List<string>());
            }
            else rounds[index].Add(line);
        }
        numOfRounds = rounds.Count;
    }
    void Start()
    {
        start = GameManager.Instance.Waypoints[0].position;
        pos = start;
    }

    //private void FixedUpdate()
    //{
    //    if (red) Instantiate(BloonManager.BloonSprites[BloonType.Red].transform).position = start;
    //    if (blue) Instantiate(BloonManager.BloonSprites[BloonType.Blue].transform).position = start;
    //    if (green) Instantiate(BloonManager.BloonSprites[BloonType.Green].transform).position = start;
    //    if (yellow) Instantiate(BloonManager.BloonSprites[BloonType.Yellow].transform).position = start;
    //    if (white) Instantiate(BloonManager.BloonSprites[BloonType.White].transform).position = start;
    //    if (black) Instantiate(BloonManager.BloonSprites[BloonType.Black].transform).position = start;
    //    if (endRound) ForceRoundEnd();
    //    if (startRound) DebugStartRound();
    //    if (reloadRounds)
    //    {
    //        ReloadRounds();
    //        --CurrentRound;
    //    }

    //    startRound = false;
    //    reloadRounds = false;
    //    endRound = false;
    //    red = false;
    //    blue = false;
    //    green = false;
    //    yellow = false;
    //    black = false; 
    //    white = false;
    //}

    private void Update()
    {
        if (!roundPlaying) return;
        if (Time.time < waitUntil) return;
        if (instructionIndex >= numOfInstructions)
        {
            if (Bloons == 0) OnRoundEnd();
            return;
        }
        var instructions = rounds[currentRound][instructionIndex].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        switch (instructions[0])
        {
            case "s":
                int size = instructions.Length;
                if (size == 2) SpawnBloon(instructions[1]); 
                else if (size == 3) SpawnBloon(instructions[1], int.Parse(instructions[2])); 
                else if (size == 4) SpawnBloon(instructions[1], int.Parse(instructions[2]), float.Parse(instructions[3])); 
                break;
            case "w":
                waitUntil = Time.time + float.Parse(instructions[1]);
                break;
        }

        instructionIndex++;
    }

    void SpawnBloon(string type, int amount = 1, float spacing = 0)
    {
        switch (type[0])
        {
            case 'r':
                type = "Red";
                break;
            case 'g':
                type = "Green";
                break;
            case 'b':
                type = "Blue";
                break;
            case 'y':
                type = "Yellow";
                break;
            case 'w':
                type = "White";
                break;
            case 'B':
                type = "Black";
                break;
        }

        if (!System.Enum.TryParse(type, false, out BloonType result)) return;
        //if (spacing < bloonsSpacings[result])
        if (spacing == 0)
            spacing = bloonsSpacings[result];

        while (amount > 0)
        {
            amount--;
            Instantiate(BloonManager.Instance.BloonSprites[result].transform).position = pos;
            pos.x -= spacing;
        }
    }

    public void StartRound()
    {
        roundPlaying = true;
        instructionIndex = 0;
        ++CurrentRound;
        pos = start;
        if (currentRound >= numOfRounds)
        {
            GameManager.Instance.OnWin();
            return;
        }
           
        numOfInstructions = rounds[currentRound].Count;
        UIManager.Instance.StartButtonActive(false);
    }

    public void OnRoundEnd()
    {
        roundPlaying = false;
        UIManager.Instance.StartButtonActive(true);
        Player.Instance.PlayerValues.Money += 100 - currentRound;
        Debug.ClearDeveloperConsole();
    }

    public void ForceRoundEnd()
    {
        if (!roundPlaying) return;
        foreach (var obj in GameObject.FindGameObjectsWithTag("Bloons"))
            Destroy(obj);
        roundPlaying = false;
        instructionIndex = 0;
        Bloons = 0;
    }

    //void DebugStartRound()
    //{
    //    roundPlaying = true;
    //    instructionIndex = 0;
    //    pos = start;
    //    if (currentRound >= numOfRounds)
    //    {
    //        Debug.Log("You Win");
    //        return;
    //    }

    //    numOfInstructions = rounds[currentRound].Count;
    //    UIManager.Instance.StartButtonActive(false);
    //}
}
