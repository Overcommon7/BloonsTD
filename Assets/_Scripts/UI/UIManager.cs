using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject gameoverPanel;
    List<TextMeshProUGUI> Texts;

    public void StartButtonActive(bool active)
    {
        startButton.SetActive(active);
    }

    public void OngameOver()
    {
        startButton.SetActive(false);
        gameoverPanel.SetActive(true);
        Texts[1].enabled = true;
    }

    public void OnWin()
    {
        Debug.Log("UI");
        startButton.SetActive(false);
        gameoverPanel.SetActive(true);
        Texts[0].enabled = true;
    }
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
        Texts = gameoverPanel.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        Texts.Remove(Texts.Find(x => x.name.Contains("TMP")));
        Texts.Sort((TextMeshProUGUI l, TextMeshProUGUI r) => l.name.CompareTo(r.name));
        foreach (var text in Texts)
            text.enabled = false;
        gameoverPanel.SetActive(false);

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

    public void StartRound()
    {
        RoundManager.Instance.StartRound();
    }
}
