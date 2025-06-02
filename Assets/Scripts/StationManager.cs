using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    public GameTimer stationTimer;

    public void OnItemBroughtIn()
    {
        if (gameObject.name == "FryingStation")
        {
            stationTimer.StartPrepTime(20f);
        }
        else
        {
            stationTimer.StartPrepTime(10f);
        }
    }

    public void OnItemTakenOut()
    {
        if (stationTimer.HasCompletedPrepTime() && stationTimer.GetElapsedTime() <= 8f)
        {
            Debug.Log("Item taken out in time!");
        }
    }
}
