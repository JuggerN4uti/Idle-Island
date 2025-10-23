using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public Island IslandScript;
    public Building BuildingScript;
    public Button[] PlacementButton;
    public Button BuildingButton;
    public bool[] PlacementViable;
    public bool BuildingViable;
    public int blockID;
    public Vector3[] Positions;
    public GameObject BuildingObject;

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
        BuildingObject.SetActive(true);
        IslandScript.PlaceElement(blockID, 0, false);
        BuildingScript.Build(IslandScript.placing);
        BuildingViable = false;
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
