using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTimer : MonoBehaviour
{
    private float timeLimit;
    private float timer;
    // Start is called before the first frame update
    void Awake()
    {
        timeLimit = 2.5F;
        timer = 0F;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeLimit)
        {
            Destroy(this.gameObject);
        }
    }
}
