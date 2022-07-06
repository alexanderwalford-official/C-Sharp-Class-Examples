using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public int CheckPointCounter = 0;
    public string Levelname = "00";
    public List<GameObject> CheckPoints;
    int NumberOfCheckPoints = 0;

    void Start()
    {
        // get the number of checkpoints in the current scene
        NumberOfCheckPoints = CheckPoints.Count;    
    }

    public void IncementCheckPoint()
    {
        CheckPointCounter++;
    }
}
