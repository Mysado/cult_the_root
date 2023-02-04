using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private TooltipsManager tooltipsManager;
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
}
