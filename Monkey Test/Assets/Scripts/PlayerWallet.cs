using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour , Iinitialize
{
    public int CoinsAmount { get => _coinsAmount; }
    public event System.Action OnCoinsValueChanged;

    [SerializeField] private TMP_Text _coinsText;

    private int _coinsAmount;

    public void Init()
    {
        OnCoinsValueChanged += UpdateCoinsText;
        OnCoinsValueChanged += SaveCoins;    // MAY BE TOO COMPLEX !!!

        PlayerCoins playerCoins = SaveSystem.LoadCoins();
        if (playerCoins == null)
        {
            _coinsAmount = 0;
            SaveSystem.SaveCoins(_coinsAmount);
        }
        else
            _coinsAmount = playerCoins.coins;

        OnCoinsValueChanged?.Invoke();
    }

    public void AddCoins(int amount)
    {
        _coinsAmount += amount;
        OnCoinsValueChanged?.Invoke();
        // animation
    }

    public bool CanAfford(int cost)
    {
        return _coinsAmount - cost > 0;
    }

    public void MinusCoins(int amount)
    {
        _coinsAmount -= amount;
        OnCoinsValueChanged?.Invoke();
        // animation
    }

    private void UpdateCoinsText()
    {
        _coinsText.text = _coinsAmount.ToString();
    }

    public void AnimateCoins (int coins)
    {
        StartCoroutine(AnimateCoinsCorutine(coins));
    }

    private IEnumerator AnimateCoinsCorutine(int coins)
    {
        int currentCoins = _coinsAmount;
        int start = currentCoins;
        _coinsAmount += coins; // add coins without animation
        //SaveCoins();

        yield return new WaitForSeconds(0.4f);

        float duration = 1f;
        gameObject.LeanScale(new Vector3(1.2f, 1.2f, 1f), duration).setEaseOutSine().setOnComplete(SetScoreTextToNormal);

        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            currentCoins = (int)Mathf.Lerp(start, _coinsAmount, progress);
            _coinsText.text = currentCoins.ToString();
            yield return null;
        }
        OnCoinsValueChanged();
    }

    private void SetScoreTextToNormal()
    {
        gameObject.LeanScale(Vector3.one, 0.3f).setEaseInOutCubic();
    }

    private void SaveCoins()
    {
        SaveSystem.SaveCoins(_coinsAmount);
    }

    private void OnDisable()
    {
        OnCoinsValueChanged -= UpdateCoinsText;
    }
}
