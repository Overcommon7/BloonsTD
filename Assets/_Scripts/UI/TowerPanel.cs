using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.InputSystem;
using MyBox;
using TMPro;

struct PlaceHolderTower
{
    public GameObject obj;
    public TowerType type;
}
public class TowerPanel : MonoBehaviour
{
    PlaceHolderTower placeHolderTower = new PlaceHolderTower();
    public static TowerPanel Instance;
    [SerializeField] Image description;
    public Image Description { get => description; }
    static Dictionary<TowerType, Tuple<Tower, Sprite>> towers = null;

    [SerializeField] Sprite dartMonkeyDescription;
    [SerializeField] Sprite tackShooterDescription;
    [SerializeField] Sprite iceTowerDescription;
    [SerializeField] Sprite bombShooterDescription;
    [SerializeField] Sprite superMonkeyDescription;
    [SerializeField] Tower dartMonkey;
    [SerializeField] Tower tackShooter;
    [SerializeField] Tower iceTower;
    [SerializeField] Tower bombShooter;
    [SerializeField] Tower superMonkey;

    [SerializeField] SpriteRenderer rangeCircle;
    Color colorRed;
    Color colorWhite;
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
        placeHolderTower.obj = new GameObject("PlaceHolder");
        placeHolderTower.obj.AddComponent<SpriteRenderer>().sortingLayerName = "Towers";
        placeHolderTower.obj.SetActive(false);
        towers = new Dictionary<TowerType, Tuple<Tower, Sprite>>()
        {
            { TowerType.DartTower, new Tuple<Tower, Sprite>(dartMonkey, dartMonkeyDescription) },
            { TowerType.TackShooter, new Tuple<Tower, Sprite>(tackShooter, tackShooterDescription) },
            { TowerType.IceTower, new Tuple<Tower, Sprite>(iceTower, iceTowerDescription) },
            { TowerType.BombTower, new Tuple<Tower, Sprite>(bombShooter, bombShooterDescription) },
            { TowerType.SuperMonkey, new Tuple<Tower, Sprite>(superMonkey, superMonkeyDescription) },
        };
        colorRed = GameManager.Instance.RangeCircle.GetComponent<SpriteRenderer>().color;
        colorWhite = new Color(1, 0, 0, colorRed.a);
        rangeCircle.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!placeHolderTower.obj.activeSelf) return;
        Vector3 vec = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var hit = Physics2D.Raycast(vec, Vector2.zero);
        vec.z = 0;
        placeHolderTower.obj.transform.position = vec;
        rangeCircle.transform.position = vec;

        if (hit.transform == null && !EventSystem.current.IsPointerOverGameObject()) rangeCircle.color = colorRed;
        else rangeCircle.color = colorWhite;

    }
    public void EnableDescriptionPanel(string tower)
    {
        if (!Enum.TryParse(tower, true, out TowerType type)) return;
        description.enabled = true;
        description.sprite = towers[type].Item2;
    }

    public void CreatePlaceHolderTower(string tower)
    {
        if (!Enum.TryParse(tower, true, out TowerType type)) return;
        if (Player.Instance.PlayerValues.Money < towers[type].Item1.Price) return;

        Player.Instance.HasTower = true;
        placeHolderTower.obj.SetActive(true);
        placeHolderTower.type = type;
        placeHolderTower.obj.GetComponent<SpriteRenderer>().sprite = towers[type].Item1.GetComponent<SpriteRenderer>().sprite;
        float range = towers[type].Item1.Range;
        rangeCircle.transform.SetLossyScale(new Vector3(range, range, range));
        rangeCircle.gameObject.SetActive(true);
    }

    public void CreateTower(ref RaycastHit2D hit)
    {
        if (hit.transform != null) return;
        if (!EventSystem.current.IsPointerOverGameObject() && Player.Instance.PlayerValues.Money >= towers[placeHolderTower.type].Item1.Price)
        {
            var t = Instantiate(towers[placeHolderTower.type].Item1.transform);
            t.transform.position = placeHolderTower.obj.transform.position;
            t.name = towers[placeHolderTower.type].Item1.name;
            Player.Instance.PlayerValues.Money -= towers[placeHolderTower.type].Item1.Price;
        }
            
        DeletePlaceHolderTower();
    }

    public void DeletePlaceHolderTower()
    {
        placeHolderTower.obj.GetComponent<SpriteRenderer>().sprite = null;
        placeHolderTower.obj.SetActive(false);
        rangeCircle.gameObject.SetActive(false);
        Player.Instance.HasTower = false;
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
