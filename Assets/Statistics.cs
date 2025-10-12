using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [Header("Scripts")]
    public Island IslandScript;

    [Header("UI")]
    public TMPro.TextMeshProUGUI[] StatTextValue;

    void Start()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        DisplayStats();
        Invoke("UpdateStats", 1.2f);
    }

    public void DisplayStats()
    {
        /*for (int i = 0; i < StatTextValue.Lengh; i++)
        {

        }*/
        StatTextValue[0].text = IslandScript.dirtBlocks.ToString("0");
        StatTextValue[1].text = IslandScript.workers.ToString("0");
        StatTextValue[2].text = IslandScript.GoldPerTick().ToString("0") + "/s";
        StatTextValue[3].text = IslandScript.LumberPerTick().ToString("0") + "/s";
        StatTextValue[4].text = "x" + (IslandScript.goldIncrease).ToString("0.00");
        StatTextValue[5].text = "x" + (IslandScript.lumberIncrease).ToString("0.00");
    }
}
