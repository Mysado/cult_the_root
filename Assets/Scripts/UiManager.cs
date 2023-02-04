using System;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private TextMeshProUGUI cultistsAmountText;

    [SerializeField] private Button buyCultistButton;
    [SerializeField] private Button upgradeCultistsButton;
        
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TooltipsManager tooltipsManager;

    public event Action OnBuyCultist;
    public event Action OnUpgradeCultists;

    private void Awake()
    {
        buyCultistButton.onClick.AddListener(TryBuyCultist);
        upgradeCultistsButton.onClick.AddListener(TryUpgradeCultists);
    }

    public void OnMoneyAmountChangedText(int moneyAmount)
    {
        moneyAmountText.text = moneyAmount.ToString();
    }

    public void ShowTooltip(Vector3 position, string text)
    {
        tooltipsManager.ShowTooltip(position, text);
    }

    public void CloseTooltip()
    {
        tooltipsManager.HideTooltip();
    }

    public void OnCultistsAmountChanged(int cultistsAmount)
    {
        cultistsAmountText.text = cultistsAmount.ToString();
    }

    private void TryBuyCultist()
    {
        if (gameManager.CanAfford(BuyableObjectType.Cultist))
        {
            OnBuyCultist?.Invoke();
        }
    }

    private void TryUpgradeCultists()
    {
        if (gameManager.CanAfford(BuyableObjectType.CultistUpgrade))
        {
            OnUpgradeCultists?.Invoke();
        }
    }
}
