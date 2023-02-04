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
    [SerializeField] private GameObject trapSelectionMenu;

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
            if (gameManager.GetCurrentSacrifice().SacrificeState is SacrificeStates.IdleAtExit or SacrificeStates.IdleAtHole or SacrificeStates.WalkingToExit or SacrificeStates.WalkingToTree)
            {
                OnBuyCultist?.Invoke();
            }
        }
    }

    private void TryUpgradeCultists()
    {
        if (gameManager.CanAfford(BuyableObjectType.CultistUpgrade))
        {
            OnUpgradeCultists?.Invoke();
        }
    }

    public void ShowTrapSelection(Vector3 position)
    {
        trapSelectionMenu.SetActive(true);
        trapSelectionMenu.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
    }

    public void CloseTrapSelection()
    {
        trapSelectionMenu.SetActive(false);
    }
}
