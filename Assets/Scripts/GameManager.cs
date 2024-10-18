using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Remake to match skeleton pattern

    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Inventory inventory;

    private int coins = 0;

    void Start()
    {
        AddCoins(coins);
    }

    public void AddCoins(int amount)
    {
        if (amount > 0)
        {
            coins += amount;
            coinsText.text = coins.ToString();
        }
    }

    
}
