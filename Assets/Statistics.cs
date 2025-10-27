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
        StatTextValue[4].text = "x" + (IslandScript.GoldIncrease()).ToString("0.000");
        StatTextValue[5].text = "x" + (IslandScript.LumberIncrease()).ToString("0.000");
        StatTextValue[6].text = IslandScript.FoodPerTick().ToString("0") + "/s";
        StatTextValue[7].text = "x" + (IslandScript.FoodIncrease()).ToString("0.000");
        StatTextValue[8].text = (IslandScript.landEfficiency / 100f).ToString("0.00");
        StatTextValue[9].text = IslandScript.forestYield.ToString("0");
        StatTextValue[10].text = IslandScript.farmYield.ToString("0");
        StatTextValue[11].text = (IslandScript.taxEfficiency / 100f).ToString("0.00");
        StatTextValue[12].text = (IslandScript.lumberPercent * 100f).ToString("0.0") + "%";
        StatTextValue[13].text = (IslandScript.foodPercent * 100f).ToString("0.0") + "%";
    }
}
