using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[Serializable]
public struct PlayerValues
{
    [SerializeField] int money;
    [SerializeField] int lives;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI livesText;

    public int Money { get => money; set { money = value; moneyText.text = money.ToString(); } }
    public int Lives { get => lives; set { lives = value; livesText.text = lives.ToString(); if (lives <= 0) GameManager.Instance.OnGameOver(); } }

}
public class Player : MonoBehaviour
{
    public static Player Instance;
    public PlayerValues PlayerValues;
    public bool HasTower = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    private void OnApplicationQuit()
    {
        Instance = null;
    }
    void Start()
    {
        PlayerValues.Money = PlayerValues.Money; 
        PlayerValues.Lives = 40; 
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();  
    }

    void HandleMouseInput()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        if (HasTower)
        {
            TowerPanel.Instance.CreateTower(ref hit);
        }
        else
        {
            UIManager.Instance.EnableUpgradePanel();
            UpgradePanel.Instance.OnMouseButtonDown(ref hit);
        }
    }
}
