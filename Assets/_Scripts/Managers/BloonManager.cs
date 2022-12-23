using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonManager : MonoBehaviour
{
    public static BloonManager Instance;
    public Dictionary<BloonType, BloonVariables> BloonSpeeds = null;
    public Dictionary<BloonType, SpriteRenderer> BloonSprites = null; 
    [SerializeField] SpriteRenderer BlackBloon;
    [SerializeField] SpriteRenderer WhiteBloon;
    [SerializeField] SpriteRenderer YellowBloon;
    [SerializeField] SpriteRenderer GreenBloon;
    [SerializeField] SpriteRenderer BlueBloon;
    [SerializeField] SpriteRenderer RedBloon;
    [SerializeField] SpriteRenderer PoppedBloon;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        BloonSpeeds = new Dictionary<BloonType, BloonVariables>()
        {
            {BloonType.Popped, new BloonVariables(float.PositiveInfinity) },
            {BloonType.Red,    new BloonVariables(1.35f)},
            {BloonType.Blue,   new BloonVariables(1.1f)},
            {BloonType.Green,  new BloonVariables(0.85f)},
            {BloonType.Yellow, new BloonVariables(0.62f)},
            {BloonType.Black,  new BloonVariables(0.75f)},
            {BloonType.White,  new BloonVariables(0.75f)}
        };

        BloonSprites = new Dictionary<BloonType, SpriteRenderer>()
        {
            {BloonType.Popped, PoppedBloon },
            {BloonType.Red,    RedBloon},
            {BloonType.Blue,   BlueBloon},
            {BloonType.Green,  GreenBloon},
            {BloonType.Yellow, YellowBloon},
            {BloonType.Black,  BlackBloon},
            {BloonType.White,  WhiteBloon}
        };
    }
}
