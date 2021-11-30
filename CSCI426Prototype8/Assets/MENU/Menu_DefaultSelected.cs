using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Menu_DefaultSelected : MonoBehaviour
{
    [SerializeField] private Button StartButtom;

    [SerializeField] private Button Level1;

    [SerializeField] private Button Level11;

    [SerializeField] private Button QuitSearchButton1;
    [SerializeField] private Button QuitSearchButton2;
    // Start is called before the first frame update
    void Start()
    {
        StartButtom.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel1Selected()
    {
        Level1.Select();
    }

    public void SetLevel11Selected()
    {
        Level11.Select();
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
