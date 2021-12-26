using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    public int CoinsAmount { get => _coinsAmount; }
    public event System.Action OnCoinsValueChanged;

    [SerializeField] private TMP_Text _coinsText;

    private int _coinsAmount;

    public void Awake()
    {
        OnCoinsValueChanged += UpdateCoinsText;

        PlayerCoins playerCoins = SaveSystem.LoadCoins();
        if (playerCoins == null)
        {
            _coinsAmount = 0;
            SaveSystem.SaveCoins(_coinsAmount);
        }
        else
            _coinsAmount = playerCoins.coins;

        OnCoinsValueChanged();
    }

    public void AddCoins(int amount)
    {
        _coinsAmount += amount;
        OnCoinsValueChanged();
        // animation
    }

    public bool CanAfford(int cost)
    {
        return _coinsAmount - cost > 0;
    }

    public void MinusCoins(int amount)
    {
        _coinsAmount -= amount;
        OnCoinsValueChanged();
        // animation
    }

    private void UpdateCoinsText()
    {
        _coinsText.text = _coinsAmount.ToString();
    }

    private void OnDisable()
    {
        OnCoinsValueChanged -= UpdateCoinsText;
    }
}
