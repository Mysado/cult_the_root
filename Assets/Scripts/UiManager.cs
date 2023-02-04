using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private TooltipsManager tooltipsManager;
    [SerializeField] private GameObject trapSelectionMenu;
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
