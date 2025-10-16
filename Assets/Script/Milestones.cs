using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Milestones : MonoBehaviour
{
    [Header("Scripts")]
    public Island IslandScript;

    [Header("Milestones")]
    public bool[] milestoneComplete; // 0 - total island score, 1 - gold collected, lumber collected
    public int[] milestoneProgress, milestoneGoal, milestonesReached;
    public string[] milestoneGoalText, suffix;

    [Header("UI")]
    public Image[] MilestoneBarFill;
    public Button[] MilestoneBarButton;
    public TMPro.TextMeshProUGUI[] MilestoneProgressText, BonusFromMilestone;

    [Header("Milestones Goals")]
    public int[] dirtToPlace;
    public int[] goldToCollect;
    public int[] lumberToCollect;

    public void ProgressMilestone(int ID, int amount)
    {
        milestoneProgress[ID] += amount;
        if (milestoneProgress[ID] >= milestoneGoal[ID])
            milestoneComplete[ID] = true;

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
                IslandScript.bonusGold += 5;
                milestoneGoal[0] = dirtToPlace[milestonesReached[0]];
                milestoneGoalText[0] = SetMilestoneGoalText(milestoneGoal[0]);
                BonusFromMilestone[ID].text = "+" + (milestonesReached[ID] * 5).ToString("0") + "\nGold per Second";
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
        while (amount >= 10000)
        {
            amount /= 1000;
            tempi++;
        }
        return amount.ToString() + suffix[tempi];
    }
}
