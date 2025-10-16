using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public Island IslandScript;
    public Button[] PlacementButton;
    public Button BuildingButton;
    public bool[] PlacementViable;
    public bool BuildingViable;
    public int blockID;
    public Vector3[] Positions;
    public SpriteRenderer BuildingSprite;
    public Sprite TentSprite, SawmillSprite, BarnSprite;

    public void DisplayPlacements()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlacementViable[i])
                PlacementButton[i].interactable = true;
        }
    }

    public void DisplayBuilding()
    {
        if (BuildingViable)
            BuildingButton.interactable = true;
    }

    public void HidePlacements()
    {
        for (int i = 0; i < 4; i++)
        {
            PlacementButton[i].interactable = false;
        }
        BuildingButton.interactable = false;
    }

    public void PlaceBlock(int position)
    {
        IslandScript.PlaceElement(blockID, position, true);
    }

    public void PlaceBuild()
    {
        BuildingSprite.enabled = true;
        IslandScript.PlaceElement(blockID, 0, false);
        BuildingViable = false;
        switch (IslandScript.placing)
        {
            case 1:
                BuildingSprite.sprite = TentSprite;
                break;
            case 3:
                BuildingSprite.sprite = SawmillSprite;
                break;
            case 5:
                BuildingSprite.sprite = BarnSprite;
                break;
        }
    }

    public void UpdateViability(Vector2 newBlockPosition)
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlacementViable[i])
            {
                if (transform.position.x + Positions[i][0] == newBlockPosition[0] && transform.position.y + Positions[i][1] == newBlockPosition[1])
                    PlacementViable[i] = false;
            }
        }
    }
}
