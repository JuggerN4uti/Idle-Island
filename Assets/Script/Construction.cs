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
    public TMPro.TextMeshProUGUI TownHallGold, TownHallWorkers, HouseWorkers, BarnGold, BarnLumber;

    [Header("Construction Costs")]
    public int[] TownHallCosts;
    public int[] HouseCosts;
    public int[] BarnCosts;

    public void CheckUpgrades()
    {
        for (int i = 0; i < upgradeCost.Length; i++)
        {
            if (IslandScript.lumber >= upgradeCost[i])
                UpgradeButton[i].interactable = true;
            else UpgradeButton[i].interactable = false;
        }
    }

    public void UpgradeBuilding(int which)
    {
        IslandScript.SpendLumber(upgradeCost[which]);
        upgradesBought[which]++;

        switch (which)
        {
            case 0:
                IslandScript.bonusGold += 4;
                IslandScript.workers += 4;
                upgradeCost[which] = TownHallCosts[upgradesBought[which]];
                TownHallGold.text = (4 * upgradesBought[which]).ToString("0");
                TownHallWorkers.text = (4 * upgradesBought[which]).ToString("0");
                break;
            case 1:
                IslandScript.workers += IslandScript.tents;
                upgradeCost[which] = HouseCosts[upgradesBought[which]];
                HouseWorkers.text = (2 + upgradesBought[which]).ToString("0");
                break;
            case 2:
                IslandScript.goldIncrease += 0.02f * IslandScript.barns;
                IslandScript.lumberIncrease += 0.01f * IslandScript.barns;
                upgradeCost[which] = BarnCosts[upgradesBought[which]];
                BarnGold.text = (4 + 2 * upgradesBought[which]).ToString("0") + "%";
                BarnLumber.text = (3 + upgradesBought[which]).ToString("0") + "%";
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
