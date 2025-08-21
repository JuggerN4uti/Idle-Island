using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Island : MonoBehaviour
{
    [Header("Resources")]
    public int gold;
    public int lumber, dirtBlocks, trees, tents;

    [Header("Island Elements")]
    public int elements;
    public bool[] costsLumber;
    public int[] elementCost, elementCostIncrease;
    public Button[] elementBuyButton;
    public TMPro.TextMeshProUGUI[] elementCostText;
    public int elementsPlaced;

    [Header("UI")]
    public TMPro.TextMeshProUGUI GoldText;
    public TMPro.TextMeshProUGUI LumberText;
    public GameObject[] DirtBlocksObject, TreeObject, TentObject;

    [Header("Unlocks")]
    public GameObject[] ObjectToRemove;
    public GameObject[] ObjectToUnlock;

    void Start()
    {
        CheckElements();
        Invoke("AutoClick", 1f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Click(1);
    }

    void AutoClick()
    {
        if (tents > 0)
            Click(tents);
        Invoke("AutoClick", 1f);
    }

    void Click(int clicks)
    {
        GainGold(dirtBlocks * clicks);
        GainLumber(trees * clicks);

        CheckElements();
    }

    void GainGold(int amount)
    {
        gold += amount;
        GoldText.text = gold.ToString("0");
    }

    void SpendGold(int amount)
    {
        gold -= amount;
        GoldText.text = gold.ToString("0");
    }

    void GainLumber(int amount)
    {
        lumber += amount;
        LumberText.text = lumber.ToString("0");
    }

    void SpendLumber(int amount)
    {
        lumber -= amount;
        LumberText.text = lumber.ToString("0");
    }

    void CheckElements()
    {
        for (int i = 0; i < elements; i++)
        {
            if (costsLumber[i])
            {
                if (lumber >= elementCost[i])
                    elementBuyButton[i].interactable = true;
                else elementBuyButton[i].interactable = false;
            }
            else
            {
                if (gold >= elementCost[i])
                    elementBuyButton[i].interactable = true;
                else elementBuyButton[i].interactable = false;
            }
        }
        CheckElements();
    }

    public void BuyElement(int elementID)
    {
        if (costsLumber[elementID])
            SpendLumber(elementCost[elementID]);
        else SpendGold(elementCost[elementID]);
        elementCost[elementID] += elementCostIncrease[elementID];
        elementCostText[elementID].text = elementCost[elementID].ToString("0");

        switch (elementID)
        {
            case 0:
                DirtBlocksObject[dirtBlocks].SetActive(true);
                elementCostIncrease[0] += 5;
                elementCostIncrease[0] += (elementCostIncrease[0] / 75) * 5;
                dirtBlocks++;
                if (dirtBlocks == 9)
                    UnlockElement(0);
                break;
            case 1:
                trees++;
                TreeObject[elementsPlaced].SetActive(true);
                elementsPlaced++;
                elementCostIncrease[1] += 10;
                elementCostIncrease[1] += (elementCostIncrease[0] / 125) * 10;
                if (trees == 1)
                    UnlockElement(1);
                break;
            case 2:
                tents++;
                TentObject[elementsPlaced].SetActive(true);
                elementsPlaced++;
                elementCostIncrease[2] += 20;
                elementCostIncrease[2] += (elementCostIncrease[0] / 107) * 10;
                break;
        }
    }

    void UnlockElement(int elementID)
    {
        ObjectToRemove[elementID].SetActive(false);
        ObjectToUnlock[elementID].SetActive(true);
    }
}
