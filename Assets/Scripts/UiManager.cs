using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyAmountText;

    public void OnMoneyAmountChangedText(int moneyAmount)
    {
        moneyAmountText.text = moneyAmount.ToString();
    }
}
