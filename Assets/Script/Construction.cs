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
    public TMPro.TextMeshProUGUI TownHallGold, TownHallGoldIncrease, TownHallWorkers, HouseWorkers, SawmillWorkers, SawmillLumber;
    public GameObject PingObject;

    [Header("Construction Costs")]
    public int[] TownHallCosts;
    public int[] HouseCosts, SawmillCosts;

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
                IslandScript.bonusGold += 3 + upgradesBought[which];
                IslandScript.GainWorkers(4);
                upgradeCost[which] = TownHallCosts[upgradesBought[which]];
                TownHallGold.text = (4 * upgradesBought[which]).ToString("0");
                TownHallGoldIncrease.text = "+" + (4 + upgradesBought[which]).ToString("0");
                TownHallWorkers.text = (4 * upgradesBought[which]).ToString("0");
                break;
            case 1:
                IslandScript.GainWorkers(IslandScript.tents);
                upgradeCost[which] = HouseCosts[upgradesBought[which]];
                HouseWorkers.text = (2 + upgradesBought[which]).ToString("0");
                break;
            case 2:
                IslandScript.GainWorkers(IslandScript.sawmills);
                IslandScript.sawmillLumber += 2;
                upgradeCost[which] = SawmillCosts[upgradesBought[which]];
                SawmillWorkers.text = (1 + upgradesBought[which]).ToString("0");
                SawmillLumber.text = (2 + 2 * upgradesBought[which]).ToString("0");
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
