using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Island : MonoBehaviour
{
    [Header("Scripts")]
    public Construction ConstructionScript;
    public Milestones MilestonesScript;
    public Statistics StatisticsScript;

    [Header("Blocks")]
    public int blocks;
    public Block[] BlockScript;
    public Transform BlockPlacedPosition;
    public Vector2[] positionTaken;
    public int placing;
    public GameObject[] BlockPrefab;

    [Header("Resources")]
    public int workers;
    public int gold, lumber, food, bonusGold;
    public int dirtBlocks, tents, trees, barns, farmlands, sawmills, sawmillLumber, glades;
    public float goldIncrease, lumberIncrease, foodIncrease;
    int workHours;

    [Header("Island Elements")]
    public int freeSpaces;
    public ElementCost[] ElementCostScript;
    public bool[] elementMaxxed;
    public int[] elementCost;
    public Button[] elementBuyButton;
    public TMPro.TextMeshProUGUI[] elementCostText;
    //public char[] suffix;

    [Header("Hiring")]
    public int hireCost;
    public int hireIncrease, suffix;
    public Button HireButton;
    public TMPro.TextMeshProUGUI HireCostText;

    [Header("UI")]
    public TMPro.TextMeshProUGUI GoldText;
    public TMPro.TextMeshProUGUI LumberText, FoodText;
    public GameObject[] DirtBlocksObject, TreeObject, TentObject;

    [Header("Windowns")]
    public GameObject[] WindowObject;
    public bool[] windowOpened;

    [Header("Unlocks")]
    public int blocksUnlocked;
    public int buildingsUnlocked, nextBlockUnlockReq, nextBuildingUnlockReq;
    public GameObject[] BlockToRemove, BlockToUnlock, BuildingToRemove, BuildingToUnlock;

    void Start()
    {
        MilestonesScript.ProgressMilestone(0, 1);
        CheckElements();
        Invoke("Tick", 1f);
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
            //Click(1);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectScreen(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectScreen(1);
        if (Input.GetKeyDown(KeyCode.Tab))
            SelectScreen(2);
        if (Input.GetKeyDown(KeyCode.T))
            TimeSkip(30);
    }

    void Tick()
    {
        /*workHours += workers;
        if (workHours > 15)
        {
            Click(workHours / 15);
            workHours = workHours % 15;
        }*/
        GainGold(GoldPerTick());
        GainLumber(LumberPerTick());
        GainFood(FoodPerTick());
        Invoke("Tick", 1f);
    }

    void TimeSkip(int seconds)
    {
        GainGold(GoldPerTick() * seconds);
        GainLumber(LumberPerTick() * seconds);
        GainFood(FoodPerTick() * seconds);
    }

    void Click(int clicks)
    {
        GainGold(GoldPerTick() * clicks);
        GainLumber(LumberPerTick() * clicks);
    }

    public int GoldPerTick()
    {
        return dirtBlocks + bonusGold + workers + (dirtBlocks * glades / 40);
    }

    void GainGold(int amount)
    {
        amount = Mathf.RoundToInt(amount * goldIncrease);
        gold += amount;
        GoldText.text = gold.ToString("0");
        MilestonesScript.ProgressMilestone(1, amount);

        CheckElements();
    }

    void SpendGold(int amount)
    {
        gold -= amount;
        GoldText.text = gold.ToString("0");
    }

    public int LumberPerTick()
    {
        return trees + (sawmills * (sawmillLumber + workers / 8));
    }

    void GainLumber(int amount)
    {
        amount = Mathf.RoundToInt(amount * lumberIncrease);
        lumber += amount;
        LumberText.text = lumber.ToString("0");
        MilestonesScript.ProgressMilestone(2, amount);

        if (windowOpened[1])
            ConstructionScript.CheckUpgrades();

        if (sawmills > 0)
            GainGold(amount * sawmills / 10);
    }

    public void SpendLumber(int amount)
    {
        lumber -= amount;
        LumberText.text = lumber.ToString("0");
    }

    public int FoodPerTick()
    {
        return farmlands;
    }

    void GainFood(int amount)
    {
        amount = Mathf.RoundToInt(amount * foodIncrease);
        food += amount;
        FoodText.text = food.ToString("0");
        //MilestonesScript.ProgressMilestone(1, amount);

        if (food >= hireCost)
            HireButton.interactable = true;
        else HireButton.interactable = false;
    }

    public void SpendFood(int amount)
    {
        food -= amount;
        FoodText.text = food.ToString("0");

        if (food >= hireCost)
            HireButton.interactable = true;
        else HireButton.interactable = false;
    }

    void CheckElements()
    {
        for (int i = 0; i < elementCost.Length; i++)
        {
            if (!elementMaxxed[i])
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
            SpendGold(elementCost[elementID]);
            ElementCostScript[elementID].bought++;
            if (ElementCostScript[elementID].bought >= ElementCostScript[elementID].cost.Length)
            {
                elementMaxxed[elementID] = true;
                elementCostText[elementID].text = "";
                elementBuyButton[elementID].interactable = false;
            }
            else
            {
                elementCost[elementID] = ElementCostScript[elementID].cost[ElementCostScript[elementID].bought];

                if (ElementCostScript[elementID].displayLevel[ElementCostScript[elementID].bought] == 0)
                    elementCostText[elementID].text = elementCost[elementID].ToString("0");
                else if (ElementCostScript[elementID].displayLevel[ElementCostScript[elementID].bought] == 1)
                    elementCostText[elementID].text = (elementCost[elementID] / 1000f).ToString() + "k";
                else
                {
                    elementCostText[elementID].text = (elementCost[elementID] / 1000000f).ToString() + "m";
                    //elementCostText[elementID].text = (elementCost[elementID] / 1000f).ToString("0.0") + suffix[];
                    // potem lepsze
                }
            }

            switch (elementID)
            {
                case 0: // grass block
                    //DirtBlocksObject[dirtBlocks].SetActive(true);
                    Build(0, true);
                    break;
                case 1: // house
                    //TreeObject[elementsPlaced].SetActive(true);
                    Build(1, false);
                    break;
                case 2: // forest block
                    Build(2, true);
                    break;
                case 3: // barn
                    Build(3, false);
                    break;
                case 4: // farmland block
                    Build(4, true);
                    break;
                case 5: // sawmill
                    Build(5, false);
                    break;
                case 6: // glade block
                    Build(6, true);
                    break;
            }

            CheckElements();
        }
    }

    public void HireWorker()
    {
        SpendFood(hireCost);
        GainWorkers(1);
        hireCost += hireIncrease;
        if (hireCost == 1000)
        {
            hireIncrease = 1000;
            suffix++;
        }
        else if (hireCost == 10000)
            hireIncrease = 10000;
        else if (hireCost == 100000)
            hireIncrease = 100000;
        if (suffix > 0)
            HireCostText.text = (hireCost / 1000).ToString("0") + "k";
        else HireCostText.text = hireCost.ToString("0");
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
                tents++;
                GainWorkers(2 + ConstructionScript.upgradesBought[1]);
                break;
            case 2:
                trees++;
                break;
            case 3:
                sawmills++;
                GainWorkers(1 + ConstructionScript.upgradesBought[2]);
                break;
            case 4:
                farmlands++;
                //GainBlock();
                break;
            case 5:
                barns++;
                //GainBlock();
                break;
            case 6:
                glades++;
                freeSpaces++;
                //GainBlock();
                break;
        }
        if (block)
            GainBlock();

        CheckElements();
    }

    void GainBlock()
    {
        dirtBlocks++;
        //if (barns > 0)
            //goldIncrease += (0.002f + 0.001f * ConstructionScript.upgradesBought[2]) * barns;
        MilestonesScript.ProgressMilestone(0, 1);
        if (dirtBlocks >= nextBlockUnlockReq)
            UnlockBlock();
    }

    public void GainWorkers(int amount)
    {
        workers += amount;
        if (workers >= nextBuildingUnlockReq)
            UnlockBuilding();
    }

    void UnlockBlock()
    {
        BlockToRemove[blocksUnlocked].SetActive(false);
        BlockToUnlock[blocksUnlocked].SetActive(true);
        blocksUnlocked++;
        nextBlockUnlockReq = (3 + blocksUnlocked * 2);
        nextBlockUnlockReq *= nextBlockUnlockReq;
    }

    void UnlockBuilding()
    {
        BuildingToRemove[buildingsUnlocked].SetActive(false);
        BuildingToUnlock[buildingsUnlocked].SetActive(true);
        buildingsUnlocked++;
        nextBuildingUnlockReq = (30 + 5 * buildingsUnlocked) * (1 + buildingsUnlocked);
    }

    void UnlockElement(int elementID)
    {
        //ObjectToRemove[elementID].SetActive(false);
        //ObjectToUnlock[elementID].SetActive(true);
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
                case 1: // construction
                    ConstructionScript.CheckUpgrades();
                    break;
                case 2: // stats
                    StatisticsScript.DisplayStats();
                    break;
            }
        }
    }
}
