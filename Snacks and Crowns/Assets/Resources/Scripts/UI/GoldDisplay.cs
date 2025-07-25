using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Displays the gold in the UI
/// </summary>
public class GoldDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        CoupleToPlayer(player);
    }
    public void CoupleToPlayer(GameObject player)
    {
        //Debug.Log("Yep, attaching to player");
        GoldTracker goldTracker = player.GetComponent<GoldTracker>();
        goldTracker.goldChange.AddListener(UpdateMoneyCounter);
        UpdateMoneyCounter(goldTracker.Gold);
    }
    void UpdateMoneyCounter(int gold)
    {
        //Debug.Log("Changing gold display to: " + gold);
        text.text = gold.ToString();
    }
}
