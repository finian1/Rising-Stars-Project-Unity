using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatsController : MonoBehaviour
{
    public Text playerHealthText;
    public Text playerScoreText;
    public Text playerCurrencyText;


    public void Update()
    {
        playerHealthText.text = "Health: " + string.Format("{0:000.0}",PlayerStats.health) + "/" + PlayerStats.starterHealth;
        playerScoreText.text = "Score: " + string.Format("{0:000000}", PlayerStats.points);
        playerCurrencyText.text = "Crystals: " + PlayerStats.currency;
    }
}
