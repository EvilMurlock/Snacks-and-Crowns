using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoldDisplay : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void CoupleToPlayer(GameObject player)
    {
        player.GetComponent<GoldTracker>().goldChange.AddListener(UpdateMoneyCounter);
    }
    void UpdateMoneyCounter(int money)
    {
        text.text = money.ToString();
    }
}
