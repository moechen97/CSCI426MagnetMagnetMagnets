using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Menu_DefaultSelected : MonoBehaviour
{
    private VariablesSaver vs;
    [SerializeField] private Button StartButtom;

    //[SerializeField] private Button Level1;
    //private MENU_LevelButton L1;
    //private Image L1Highlight;
    //[SerializeField] private Button Level11;
    //private MENU_LevelButton L11;
    //private Image L11Highlight;
    [SerializeField] private Button QuitSearchButton1;
    [SerializeField] private Button QuitSearchButton2;
    // Start is called before the first frame update
    void Start()
    {
        vs = GameObject.FindGameObjectWithTag("VariablesSaver").GetComponent<VariablesSaver>();
        //L1 = Level1.GetComponent<MENU_LevelButton>();
        //L11 = Level11.GetComponent<MENU_LevelButton>();
        StartButtom.Select();
    }

    public void SetLevel1Selected()
    {
        //Level1.Select();
    }

    public void SetLevel11Selected()
    {
        //Level11.Select();
    }

    public void SetQuit1Button()
    {
        QuitSearchButton1.Select();
    }
    
    public void SetQuit2Button()
    {
        QuitSearchButton2.Select();
    }

    public void QuitGameButton()
    {
        
    }
}
