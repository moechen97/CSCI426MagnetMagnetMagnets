using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_MAGNETMANAGER : MonoBehaviour
{
    private enum CurrentMagnets { None, Left1, Left2, Right1, Right2 }
    public Menu_MAGNETBUTTON Left1;
    public Menu_MAGNETBUTTON Left2;
    public Menu_MAGNETBUTTON Right1;
    public Menu_MAGNETBUTTON Right2;

    private CurrentMagnets[] currentMagnets;

    private void Awake()
    {
        currentMagnets = new CurrentMagnets[2];
        currentMagnets[0] = CurrentMagnets.None;
        currentMagnets[1] = CurrentMagnets.None;
    }

    public void OnClick(Menu_MAGNETBUTTON click)
    {
        if(click == Left1)
        {
            if(currentMagnets[0] == CurrentMagnets.None)
            {
                foreach(Transform child in Left1.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
            else if(currentMagnets[0] == CurrentMagnets.Left1)
            {

            }
            else if(currentMagnets[0] == CurrentMagnets.Left2)
            {

            }
        }
        else if(click == Left2)
        {
            if (currentMagnets[0] == CurrentMagnets.None)
            {

            }
            else if (currentMagnets[0] == CurrentMagnets.Left1)
            {

            }
            else if (currentMagnets[0] == CurrentMagnets.Left2)
            {

            }
        }
        else if(click == Right1)
        {
            if (currentMagnets[0] == CurrentMagnets.None)
            {

            }
            else if (currentMagnets[0] == CurrentMagnets.Right1)
            {

            }
            else if (currentMagnets[0] == CurrentMagnets.Right2)
            {

            }
        }
        else if(click == Right2)
        {
            if (currentMagnets[0] == CurrentMagnets.None)
            {

            }
            else if (currentMagnets[0] == CurrentMagnets.Right1)
            {

            }
            else if (currentMagnets[0] == CurrentMagnets.Right2)
            {

            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
