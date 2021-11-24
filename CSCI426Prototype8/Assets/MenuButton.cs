using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    private VariablesSaver vs;
    private void Start()
    {
        vs = GameObject.FindGameObjectWithTag("VariablesSaver").GetComponent<VariablesSaver>();
    }
    public void ClickMenu()
    {
        vs.LoadLevel(0);
    }
}
