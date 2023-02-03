using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public event Action<int> UpdateMoneyAmount; 
    
    private int _money;

    private void AddMoney(int addAmount)
    {
        _money += addAmount;
        UpdateMoneyAmount?.Invoke(_money);
    }

    private void SpendMoney(int spendAmount)
    {
        _money -= spendAmount;
        UpdateMoneyAmount?.Invoke(_money);
    }
}
