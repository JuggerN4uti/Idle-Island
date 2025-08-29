using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Island : MonoBehaviour
{
    [Header("Scripts")]
    public Milestones MilestonesScript;

    [Header("Blocks")]
    public int blocks;
    public Block[] BlockScript;
    public Transform BlockPlacedPosition;
    public Vector2[] positionTaken;
    public int placing;
    public GameObject[] BlockPrefab;

    [Header("Resources")]
    public int gold;
    public int lumber, dirtBlocks, trees, tents, bonusGold;
    public float goldIncrease, lumberIncrease;

    [Header("Island Elements")]
    public int freeSpaces;
    public ElementCost[] ElementCostScript;
    public bool[] costsLumber;
    public int[] elementCost, elementCostIncrease;
    public Button[] elementBuyButton;
    public TMPro.TextMeshProUGUI[] elementCostText;
    //public char[] suffix;

    [Header("UI")]
    public TMPro.TextMeshProUGUI GoldText;
    public TMPro.TextMeshProUGUI LumberText;
    public GameObject[] DirtBlocksObject, TreeObject, TentObject;

    [Header("Windowns")]
    public GameObject[] WindowObject;
    public bool[] windowOpened;

    [Header("Unlocks")]
    public GameObject[] ObjectToRemove;
    public GameObject[] ObjectToUnlock;

    void Start()
    {
        MilestonesScript.ProgressMilestone(0, 1);
        CheckElements();
        Invoke("AutoClick", 1f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Click(1);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectScreen(0);
        if (Input.GetKeyDown(KeyCode.Tab))
            SelectScreen(1);
    }

    void AutoClick()
    {
        if (tents > 0)
            Click(tents);
        Invoke("AutoClick", 1f);
    }

    void Click(int clicks)
    {
        GainGold((dirtBlocks + bonusGold) * clicks);
        GainLumber(trees * clicks);

        CheckElements();
    }

    void GainGold(int amount)
    {
        amount = Mathf.RoundToInt(amount * goldIncrease);
        gold += amount;
        GoldText.text = gold.ToString("0");
        MilestonesScript.ProgressMilestone(1, amount);
    }

    void SpendGold(int amount)
    {
        gold -= amount;
        GoldText.text = gold.ToString("0");
    }

    void GainLumber(int amount)
    {
        amount = Mathf.RoundToInt(amount * lumberIncrease);
        lumber += amount;
        LumberText.text = lumber.ToString("0");
        MilestonesScript.ProgressMilestone(2, amount);
    }

    void SpendLumber(int amount)
    {
        lumber -= amount;
        LumberText.text = lumber.ToString("0");
    }

    void CheckElements()
    {
        for (int i = 0; i < elementCost.Length; i++)
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
    }

    public void BuyElement(int elementID)
    {
        if (elementID == 0 || freeSpaces > 0)
        {
            if (costsLumber[elementID])
                SpendLumber(elementCost[elementID]);
            else SpendGold(elementCost[elementID]);
            ElementCostScript[elementID].bought++;
            elementCost[elementID] = ElementCostScript[elementID].cost[ElementCostScript[elementID].bought];

            if (ElementCostScript[elementID].displayLevel[ElementCostScript[elementID].bought] == 0)
                elementCostText[elementID].text = elementCost[elementID].ToString("0");
            else if (ElementCostScript[elementID].displayLevel[ElementCostScript[elementID].bought] == 1)
                elementCostText[elementID].text = (elementCost[elementID] / 1000f).ToString("0.0") + "k";
            else
            {
                elementCostText[elementID].text = (elementCost[elementID] / 1000000f).ToString("0.0") + "m";
                //elementCostText[elementID].text = (elementCost[elementID] / 1000f).ToString("0.0") + suffix[];
                // potem lepsze
            }

            switch (elementID)
            {
                case 0:
                    //DirtBlocksObject[dirtBlocks].SetActive(true);
                    Build(0, true);
                    break;
                case 1:
                    //TreeObject[elementsPlaced].SetActive(true);
                    Build(1, true);
                    break;
                case 2:
                    Build(2, false);
                    break;
                case 3:
                    Build(3, true);
                    break;
            }

            CheckElements();
        }
    }

    void Build(int element, bool block)
    {
        placing = element;
        if (block)
        {
            for (int i = 0; i < blocks; i++)
            {
                BlockScript[i].DisplayPlacements();
            }
        }
        else
        {
            for (int i = 0; i < blocks; i++)
            {
                BlockScript[i].DisplayBuilding();
            }
        }
    }

    public void PlaceElement(int blockID, int position, bool block)
    {
        if (block)
        {
            for (int i = 0; i < blocks; i++)
            {
                BlockScript[i].HidePlacements();
            }
            BlockPlacedPosition.position = new Vector3(BlockScript[blockID].transform.position.x + BlockScript[blockID].Positions[position][0], 
                BlockScript[blockID].transform.position.y + BlockScript[blockID].Positions[position][1], BlockScript[blockID].transform.position.z + BlockScript[blockID].Positions[position][2]);
            GameObject blockPlaced = Instantiate(BlockPrefab[placing], BlockPlacedPosition.position, transform.rotation);
            positionTaken[blocks] = new Vector2(BlockPlacedPosition.position.x, BlockPlacedPosition.position.y);
            BlockScript[blocks] = blockPlaced.GetComponent(typeof(Block)) as Block;
            BlockScript[blocks].IslandScript = this;
            BlockScript[blocks].blockID = blocks;
            for (int i = 0; i < blocks; i++)
            {
                BlockScript[i].UpdateViability(positionTaken[blocks]);
                BlockScript[blocks].UpdateViability(positionTaken[i]);
            }
            blocks++;
        }
        else
        {
            for (int i = 0; i < blocks; i++)
            {
                BlockScript[i].HidePlacements();
            }
            freeSpaces--;
        }

        switch (placing)
        {
            case 0:
                freeSpaces++;
                break;
            case 1:
                trees++;
                if (trees == 3)
                    UnlockElement(1);
                break;
            case 2:
                tents++;
                break;
            case 3:
                goldIncrease += 0.05f;
                lumberIncrease += 0.05f;
                break;
        }
        if (block)
        {
            dirtBlocks++;
            MilestonesScript.ProgressMilestone(0, 1);
            if (dirtBlocks == 9)
                UnlockElement(0);
            if (dirtBlocks == 24)
                UnlockElement(2);
        }

        CheckElements();
    }

    void UnlockElement(int elementID)
    {
        ObjectToRemove[elementID].SetActive(false);
        ObjectToUnlock[elementID].SetActive(true);
    }

    public void SelectScreen(int screen)
    {
        if (windowOpened[screen])
        {
            windowOpened[screen] = false;
            WindowObject[screen].SetActive(false);
        }
        else
        {
            for (int i = 0; i < WindowObject.Length; i++)
            {
                windowOpened[i] = false;
                WindowObject[i].SetActive(false);
            }
            windowOpened[screen] = true;
            WindowObject[screen].SetActive(true);

            switch (screen)
            {
                case 0: // milestones
                    MilestonesScript.DisplayWindow();
                    break;
            }
        }
    }
}
