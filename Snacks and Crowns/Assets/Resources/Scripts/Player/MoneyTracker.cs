using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MoneyTracker : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void CoupleToPlayer(GameObject player)
    {
        player.GetComponent<Player_Inventory>().moneyChange.AddListener(UpdateMoneyCounter);
    }
    void UpdateMoneyCounter(int money)
    {
        text.text = money.ToString();
    }
}
