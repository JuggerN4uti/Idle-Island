using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Milestones : MonoBehaviour
{
    [Header("Scripts")]
    public Island IslandScript;
    public Scrollbar sliderScript;

    [Header("Milestones")]
    public bool[] milestoneComplete; // 0 - total island score, 1 - gold collected, lumber collected
    public int[] milestoneProgress, milestoneGoal, milestonesReached;
    public string[] milestoneGoalText, suffix;

    [Header("UI")]
    public Image[] MilestoneBarFill;
    public Button[] MilestoneBarButton;
    public TMPro.TextMeshProUGUI[] MilestoneProgressText, BonusFromMilestone;
    public GameObject PingObject;
    public RectTransform ContentTransform;

    [Header("Milestones Goals")]
    public int[] dirtToPlace;
    public int[] goldToCollect;
    public int[] lumberToCollect;
    public int[] foodToCollect;
    public int[] ticksToWork;

    public void ProgressMilestone(int ID, int amount)
    {
        milestoneProgress[ID] += amount;
        if (!milestoneComplete[ID])
        {
            if (milestoneProgress[ID] >= milestoneGoal[ID])
            {
                milestoneComplete[ID] = true;
                if (!IslandScript.windowOpened[0])
                    PingObject.SetActive(true);
            }
        }

        if (IslandScript.windowOpened[0])
            DisplayMilestone(ID);
    }

    public void CompleteMilestone(int ID)
    {
        milestonesReached[ID]++;
        //BonusFromMilestone[ID].text = "+" + (milestonesReached[ID] * 5).ToString("0") + "%";

        switch (ID)
        {
            case 0:
                IslandScript.landEfficiency += 30;
                milestoneGoal[0] = dirtToPlace[milestonesReached[0]];
                milestoneGoalText[0] = SetMilestoneGoalText(milestoneGoal[0]);
                BonusFromMilestone[ID].text = "+" + (milestonesReached[ID] * 0.3f).ToString("0.0") + "\nGold per Block";
                break;
            case 1:
                IslandScript.goldIncrease += 0.05f;
                milestoneGoal[1] = goldToCollect[milestonesReached[1]];
                milestoneGoalText[1] = SetMilestoneGoalText(milestoneGoal[1]);
                BonusFromMilestone[ID].text = "+" + (milestonesReached[ID] * 5).ToString("0") + "%\nGold";
                break;
            case 2:
                IslandScript.lumberIncrease += 0.05f;
                milestoneGoal[2] = lumberToCollect[milestonesReached[2]];
                milestoneGoalText[2] = SetMilestoneGoalText(milestoneGoal[2]);
                BonusFromMilestone[ID].text = "+" + (milestonesReached[ID] * 5).ToString("0") + "%\nLumber";
                break;
            case 3:
                IslandScript.foodIncrease += 0.05f;
                milestoneGoal[3] = foodToCollect[milestonesReached[3]];
                milestoneGoalText[3] = SetMilestoneGoalText(milestoneGoal[3]);
                BonusFromMilestone[ID].text = "+" + (milestonesReached[ID] * 5).ToString("0") + "%\nFood";
                break;
            case 4:
                IslandScript.taxEfficiency += 20;
                milestoneGoal[4] = ticksToWork[milestonesReached[4]];
                milestoneGoalText[4] = SetMilestoneGoalText(milestoneGoal[4]);
                BonusFromMilestone[ID].text = "+" + (milestonesReached[ID] * 0.2f).ToString("0.0") + "\nGold per Worker";
                break;
        }

        milestoneComplete[ID] = false;
        MilestoneBarButton[ID].interactable = false;
        ProgressMilestone(ID, 0);
    }

    public void DisplayWindow()
    {
        for (int i = 0; i < milestoneComplete.Length; i++)
        {
            DisplayMilestone(i);
        }
        PingObject.SetActive(false);
    }

    void DisplayMilestone(int ID)
    {
        MilestoneBarFill[ID].fillAmount = (milestoneProgress[ID] * 1f) / (milestoneGoal[ID] * 1f);
        MilestoneProgressText[ID].text = milestoneProgress[ID].ToString() + "/" + milestoneGoalText[ID];
        if (milestoneComplete[ID])
            MilestoneBarButton[ID].interactable = true;
    }

    string SetMilestoneGoalText(int amount)
    {
        int tempi = 0;
        while (amount >= 1000)
        {
            amount /= 1000;
            tempi++;
        }
        return amount.ToString() + suffix[tempi];
    }

    public void moveUI()
    {
        //ContentTransform.transform.position = new Vector3(0f, sliderScript.value * 125f, 0f);
    }
}
