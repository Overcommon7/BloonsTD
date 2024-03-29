using System.IO;
using UnityEngine;

public static class Defines
{
    public readonly static Color RED = new Color(143f / 255f, 0f, 0f, 1f);
    public readonly static Color GREEN = new Color(6f / 255f, 1f, 9f / 255f, 1f);
    public static readonly string RoundPath = Path.Combine(Application.persistentDataPath, "Rounds.txt");
}
