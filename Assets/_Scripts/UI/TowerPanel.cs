using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPanel : MonoBehaviour
{
    public static TowerPanel Instance;
    public Image description;
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

    public void EnableDescriptionPanel()
    {
        description.enabled = true;
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
