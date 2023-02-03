using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private MoneyManager moneyManager;

    private void Awake()
    {
        moneyManager.UpdateMoneyAmount += UpdateMoneyAmountText;
    }

    private void UpdateMoneyAmountText(int moneyAmount)
    {
        moneyAmountText.text = moneyAmount.ToString();
    }
}
