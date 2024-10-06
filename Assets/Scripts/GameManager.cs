using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    private int coins = 0;
    public void AddCoins(int amount)
    {
        if (amount > 0)
        {
            coins += amount;
            coinsText.text = coins.ToString();
        }
    }

    void Start()
    {
        AddCoins(coins);
    }
}
