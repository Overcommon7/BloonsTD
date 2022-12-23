using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
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
        DisableUpgradePanel();
    }

    private void OnApplicationQuit()
    {
        Instance = null;
    }

    public void EnableUpgradePanel()
    {
        TowerPanel.Instance.Description.enabled = false;
        UpgradePanel.Instance.gameObject.SetActive(true);
    }

    public void DisableUpgradePanel()
    {
        TowerPanel.Instance.Description.enabled = true;
        TowerPanel.Instance.Description.sprite = null;
        UpgradePanel.Instance.gameObject.SetActive(false);
        GameManager.Instance.RangeCircle.gameObject.SetActive(false);
    }
}
