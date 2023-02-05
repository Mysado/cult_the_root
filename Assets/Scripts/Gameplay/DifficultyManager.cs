using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private int startingDifficulty;
    [SerializeField] private int maxDifficulty;
    private int currentDifficulty;

    public int CurrentDifficulty => currentDifficulty;
    void Start()
    {
        currentDifficulty = 1;
    }

    public void IncreaseDifficulty()
    {
        if (currentDifficulty < maxDifficulty)
            currentDifficulty++;
    }
}
