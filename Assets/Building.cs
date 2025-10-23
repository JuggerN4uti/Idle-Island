using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scripts")]
    public Block BlockScript;

    [Header("UI")]
    public Image BuildingSprite;
    public Sprite[] TentSprite, SawmillSprite, BarnSprite;
    public TMPro.TextMeshProUGUI CostText;
    public Button UpgradeButton;

    [Header("Stats")]
    public bool upgradeable;
    public int buildingPlaced, buildingLevel;

    [Header("Costs")]
    public int currentUpgradeCost;
    public int[] HouseCosts, SawmillCosts, BarnCosts;

    public void Build(int buildingID)
    {
        buildingPlaced = buildingID;

        switch (buildingID)
        {
            case 1:
                BuildingSprite.sprite = TentSprite[0];
                currentUpgradeCost = HouseCosts[0];
                break;
            case 3:
                BuildingSprite.sprite = SawmillSprite[0];
                currentUpgradeCost = SawmillCosts[0];
                break;
            case 5:
                BuildingSprite.sprite = BarnSprite[0];
                currentUpgradeCost = BarnCosts[0];
                break;
        }
    }

    public void ClickBuilding()
    {
        if (upgradeable)
        {
            if (BlockScript.IslandScript.lumber >= currentUpgradeCost)
            {
                BlockScript.IslandScript.SpendLumber(currentUpgradeCost);
                UpgradeBuilding();
            }
        }
    }

    void UpgradeBuilding()
    {
        buildingLevel++;
        switch (buildingPlaced)
        {
            case 1: // house
                BuildingSprite.sprite = TentSprite[buildingLevel];
                if (buildingLevel == HouseCosts.Length)
                {
                    upgradeable = false;
                    UpgradeButton.interactable = false;
                }
                else currentUpgradeCost = HouseCosts[buildingLevel];

                if (buildingLevel < 3)
                {
                    BlockScript.IslandScript.GainWorkers(1);
                    BlockScript.IslandScript.goldIncrease += 0.005f + 0.001f * buildingLevel;
                }
                else
                {
                    BlockScript.IslandScript.GainWorkers(2);
                    BlockScript.IslandScript.goldIncrease += 0.002f + 0.001f * buildingLevel;
                }
                break;
            case 3: // sawmill
                BuildingSprite.sprite = SawmillSprite[buildingLevel];
                if (buildingLevel == SawmillCosts.Length)
                {
                    upgradeable = false;
                    UpgradeButton.interactable = false;
                }
                else currentUpgradeCost = SawmillCosts[buildingLevel];

                if (buildingLevel < 3)
                {
                    BlockScript.IslandScript.GainWorkers(1);
                    BlockScript.IslandScript.forestYield += 1;
                    BlockScript.IslandScript.lumberPercent += 0.009f + 0.001f * buildingLevel;
                }
                else
                {
                    if (buildingLevel == 4)
                    {
                        BlockScript.IslandScript.GainWorkers(2);
                        BlockScript.IslandScript.forestYield += 2;
                        BlockScript.IslandScript.lumberPercent += 0.003f + 0.001f * buildingLevel;
                    }
                    else
                    {
                        BlockScript.IslandScript.GainWorkers(2);
                        BlockScript.IslandScript.forestYield += 1;
                        BlockScript.IslandScript.lumberPercent += 0.007f + 0.001f * buildingLevel;
                    }
                }
                break;
            case 5: // barn
                BuildingSprite.sprite = BarnSprite[buildingLevel];
                if (buildingLevel == BarnCosts.Length)
                {
                    upgradeable = false;
                    UpgradeButton.interactable = false;
                }
                else currentUpgradeCost = BarnCosts[buildingLevel];

                if (buildingLevel == 1)
                {
                    BlockScript.IslandScript.GainWorkers(1);
                    BlockScript.IslandScript.farmYield += 1;
                    BlockScript.IslandScript.foodPercent += 0.011f + 0.001f * buildingLevel;
                }
                else
                {
                    if (buildingLevel >= 3)
                    {
                        BlockScript.IslandScript.GainWorkers(2);
                        BlockScript.IslandScript.farmYield += 2;
                        BlockScript.IslandScript.foodPercent += 0.002f + 0.002f * buildingLevel;
                    }
                    else
                    {
                        BlockScript.IslandScript.GainWorkers(2);
                        BlockScript.IslandScript.farmYield += 1;
                        BlockScript.IslandScript.foodPercent += 0.01f + 0.001f * buildingLevel;
                    }
                }
                break;
        }
        CostText.text = currentUpgradeCost.ToString("0");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (upgradeable)
            CostText.text = currentUpgradeCost.ToString("0");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CostText.text = "";
    }
}
