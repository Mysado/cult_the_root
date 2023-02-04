using System;
using UnityEditor.Rendering;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int currentMoney = 25;
    public event Action<int> OnMoneyAmountChanged;

    public void Initialize()
    {
        OnMoneyAmountChanged?.Invoke(currentMoney);
    }

    public void AddMoney(int addAmount)
    {
        currentMoney += addAmount;
        OnMoneyAmountChanged?.Invoke(currentMoney);
    }

    public void SpendMoney(int spendAmount)
    {
        currentMoney -= spendAmount;
        OnMoneyAmountChanged?.Invoke(currentMoney);
    }

    public bool CanAfford(int moneyAmount)
    {
        return currentMoney >= moneyAmount;
    }
}
