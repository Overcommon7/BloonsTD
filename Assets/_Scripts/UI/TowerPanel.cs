using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerPanel : MonoBehaviour
{
    public static TowerPanel Instance;
    [SerializeField] Image description;
    public Image Description { get => description; }

    [SerializeField] Sprite dartMonkey;
    [SerializeField] Sprite tackShooter;
    [SerializeField] Sprite iceTower;
    [SerializeField] Sprite bombShooter;
    [SerializeField] Sprite superMonkey;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
         DisableDescriptionPanel();
    }

    public void EnableDescriptionPanel(string tower)
    {
        if (!System.Enum.TryParse(tower, true, out TowerType type)) return;
        description.enabled = true;

        switch (type)
        {
            case TowerType.DartTower:
                description.sprite = dartMonkey;
                break;
            case TowerType.TackShooter:
                description.sprite = tackShooter;
                break;
            case TowerType.IceTower:
                description.sprite = iceTower;
                break;
            case TowerType.BombTower:
                description.sprite = bombShooter;
                break;
            case TowerType.SuperMonkey:
                description.sprite = superMonkey;
                break;
        };
    }

    public void DisableDescriptionPanel()
    {
        description.enabled = false;
    }

    private void OnApplicationQuit()
    {
        Instance = null;
    }
}
