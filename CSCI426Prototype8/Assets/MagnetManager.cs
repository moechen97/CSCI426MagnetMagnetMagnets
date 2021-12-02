using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    [HideInInspector] public Transform[] magnets;
    // Start is called before the first frame update
    void Awake()
    {
        magnets = new Transform[GameObject.FindGameObjectWithTag("MagnetPositions").transform.childCount + 1];
    }
    public void RecordMagnetPosition(int magnetIndex, Transform magnetTransform)
    {
        magnets[magnetIndex] = magnetTransform;
    }

    public Transform ReturnMagnetPosition(int magnetIndex)
    {
        return magnets[magnetIndex];
    }
}
