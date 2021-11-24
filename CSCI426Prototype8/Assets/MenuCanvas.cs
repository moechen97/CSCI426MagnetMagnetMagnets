using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    private VariablesSaver vs;
    [SerializeField] GameObject startGameMenu;
    [SerializeField] GameObject selectLevelMenu;
    // Start is called before the first frame update
    void Start()
    {
        vs = GameObject.FindGameObjectWithTag("VariablesSaver").GetComponent<VariablesSaver>();
        if(vs.gameStarted)
        {
            startGameMenu.SetActive(false);
            selectLevelMenu.SetActive(true);
        }
    }
}
