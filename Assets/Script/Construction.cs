using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Construction : MonoBehaviour
{
    [Header("Scripts")]
    public Island IslandScript;

    [Header("Upgrades")]
    public int[] upgradeCost;
    public int[] upgradesBought;
    public string[] suffix;

    [Header("UI")]
    public Button[] UpgradeButton;
    public TMPro.TextMeshProUGUI[] UpgradeCostText;
    public TMPro.TextMeshProUGUI TownHallGold, TownHallEfficiency, TownHallWorkers, HouseGold, HouseWorkers, SawmillWorkers, SawmillLumber, SawmillEfficiency, BarnWorkers, BarnFood, BarnEfficiency;
    public GameObject PingObject;

    [Header("Construction Costs")]
    public int[] TownHallCosts;
    public int[] HouseCosts, SawmillCosts, BarnCosts;

    public void CheckUpgrades()
    {
        for (int i = 0; i < upgradeCost.Length; i++)
        {
            if (IslandScript.lumber >= upgradeCost[i])
                UpgradeButton[i].interactable = true;
            else UpgradeButton[i].interactable = false;
        }
        PingObject.SetActive(false);
    }

    public void UpgradeBuilding(int which)
    {
        IslandScript.SpendLumber(upgradeCost[which]);
        upgradesBought[which]++;

        switch (which)
        {
            case 0:
                IslandScript.goldIncrease += 0.04f;
                IslandScript.GainWorkers(5);
                IslandScript.goldPercent += 0.01f;
                upgradeCost[which] = TownHallCosts[upgradesBought[which]];
                TownHallGold.text = (4 * upgradesBought[which]).ToString("0") + "%";
                TownHallWorkers.text = (5 * upgradesBought[which]).ToString("0");
                TownHallEfficiency.text = (1 * upgradesBought[which]).ToString("0") + "%";
                break;
            case 1:
                IslandScript.goldIncrease += IslandScript.tents * 0.006f;
                IslandScript.GainWorkers(IslandScript.tents);
                upgradeCost[which] = HouseCosts[upgradesBought[which]];
                HouseGold.text = (0.8f + 0.6f * upgradesBought[which]).ToString("0.0") + "%";
                HouseWorkers.text = (2 + upgradesBought[which]).ToString("0");
                break;
            case 2:
                IslandScript.GainWorkers(IslandScript.sawmills);
                IslandScript.forestYield += IslandScript.sawmills;
                IslandScript.lumberPercent += IslandScript.sawmills * 0.01f;
                upgradeCost[which] = SawmillCosts[upgradesBought[which]];
                SawmillWorkers.text = (3 + upgradesBought[which]).ToString("0");
                SawmillLumber.text = (1 + upgradesBought[which]).ToString("0");
                SawmillEfficiency.text = (1 + upgradesBought[which]).ToString("0") + "%";
                break;
            case 3:
                IslandScript.GainWorkers(IslandScript.barns);
                IslandScript.farmYield += IslandScript.barns;
                IslandScript.foodPercent += IslandScript.barns * 0.01f;
                upgradeCost[which] = BarnCosts[upgradesBought[which]];
                BarnWorkers.text = (4 + upgradesBought[which]).ToString("0");
                BarnFood.text = (1 + upgradesBought[which]).ToString("0");
                BarnEfficiency.text = (1 + upgradesBought[which]).ToString("0") + "%";
                break;
        }

        UpgradeCostText[which].text = SetCostText(upgradeCost[which]);

        CheckUpgrades();
    }

    string SetCostText(int amount)
    {
        int tempi = 0;
        while (amount >= 10000)
        {
            amount /= 1000;
            tempi++;
        }
        return amount.ToString("0") + suffix[tempi];
    }
}
