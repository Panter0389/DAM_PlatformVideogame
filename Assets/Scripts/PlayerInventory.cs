using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public CoinManager coinManager;
    bool hasKey = false;

    int coins = 0;

    public void AddCoins(int amount)
    {
        coins += amount;
        coinManager.UpdateCoinUI(coins);
    }

    public void TakeKey()
    {
        hasKey = true;
        Debug.Log("CHIAVE PRESA!");
    }

    public bool HasKey()
    {
        return hasKey;
    }

}
