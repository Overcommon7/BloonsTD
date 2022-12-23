using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct UpgradeButtonComponents
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI price;
    public Image icon;
    public int index;
}
public class UpgradePanel : MonoBehaviour
{
    public static UpgradePanel Instance;
    [SerializeField] List<Button> upgradeButtons = new List<Button>();
    [SerializeField] Button sellButton;
    [SerializeField] TextMeshProUGUI popCount;
    [ReadOnly] public Tower SelectedTower = null;
    [SerializeField] Sprite[] dartMonkeyIcons = new Sprite[2];
    [SerializeField] Sprite[] tackShooterIcons = new Sprite[2];
    [SerializeField] Sprite[] iceTowerIcons = new Sprite[2];
    [SerializeField] Sprite[] bombTowerIcons = new Sprite[2];
    [SerializeField] Sprite[] SuperMonkeyIcons = new Sprite[2];

    List<UpgradeButtonComponents> components = new List<UpgradeButtonComponents>();
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
        int i = 0;
        foreach (var button in upgradeButtons)
        {
            UpgradeButtonComponents component = new UpgradeButtonComponents();
            var meshes = button.GetComponentsInChildren<TextMeshProUGUI>();
            component.name = Array.Find(meshes, x => x.name.Contains("Name"));
            component.price = Array.Find(meshes, x => x.name.Contains("Price"));
            component.icon = button.transform.GetChildsWhere(x => x.GetComponent<Image>() != null)[0].GetComponent<Image>();
            component.index = i;
            components.Add(component);
            ++i;
        }


    }
    private void OnApplicationQuit()
    {
        Instance = null;
    }

    public void OnMouseButtonDown(ref RaycastHit2D hit)
    {
        if(hit.transform) SelectedTower = hit.transform.GetComponent<Tower>();
        else SelectedTower = null;

        if (SelectedTower == null)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                foreach (var button in upgradeButtons)
                    button.gameObject.SetActive(false);
                sellButton.gameObject.SetActive(false);
                GameManager.Instance.RangeCircle.gameObject.SetActive(false);
                popCount.text = string.Empty;
                TowerPanel.Instance.Description.gameObject.SetActive(true);
            }
            return;
        }

        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(true);
            components[i].name.text = SelectedTower.Upgrades[i].name;
            components[i].price.text = "Buy For: " + SelectedTower.Upgrades[i].price.ToString();
            components[i].icon.sprite = GetSprite(SelectedTower.TowerType, i);
            if (Player.Instance.PlayerValues.Money < SelectedTower.Upgrades[i].price) upgradeButtons[i].GetComponent<Image>().color = Defines.RED;
            else upgradeButtons[i].GetComponent<Image>().color = Defines.GREEN;
        }

        sellButton.gameObject.SetActive(true);
        sellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sell For: " + SelectedTower.TowerVariables.sellPrice.ToString();
        popCount.text = "Pop Count: " + SelectedTower.TowerVariables.popCount.ToString();
        float range = SelectedTower.TowerVariables.range;
        TowerPanel.Instance.Description.gameObject.SetActive(false);
        if (range <= 0) return;
        GameManager.Instance.RangeCircle.gameObject.SetActive(true);
        GameManager.Instance.RangeCircle.transform.position = SelectedTower.transform.position;
        GameManager.Instance.RangeCircle.transform.SetLossyScale(new Vector3(range, range, range));
    }
    private Sprite GetSprite(TowerType type, int idx)
    {                                                                  
        switch (type)
        {
            case TowerType.DartTower:
                return dartMonkeyIcons[idx];
            case TowerType.TackShooter:
                return tackShooterIcons[idx];
            case TowerType.IceTower:
                return iceTowerIcons[idx];
            case TowerType.BombTower:
                return bombTowerIcons[idx];
            case TowerType.SuperMonkey:
                return SuperMonkeyIcons[idx];   
            default:
                return null;
        }
    }

    public void ApplyUpgrade()
    {
        
    }
}
