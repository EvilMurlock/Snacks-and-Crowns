using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MoneyTracker : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void CoupleToPlayer(GameObject player, int money)
    {
        player.GetComponent<Player_Inventory>().moneyChange.AddListener(UpdateMoneyCounter);
        UpdateMoneyCounter(money);
    }
    void UpdateMoneyCounter(int money)
    {
        text.text = money.ToString();
    }
}
