using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public event Action<int> OnMoneyAmountChanged; 
    
    private int _money = 25;

    public void AddMoney(int addAmount)
    {
        _money += addAmount;
        OnMoneyAmountChanged?.Invoke(_money);
    }

    public void SpendMoney(int spendAmount)
    {
        _money -= spendAmount;
        OnMoneyAmountChanged?.Invoke(_money);
    }

    public bool CanAfford(int moneyAmount)
    {
        return _money >= moneyAmount;
    }
}
