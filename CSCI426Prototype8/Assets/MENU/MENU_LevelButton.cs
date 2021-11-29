using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MENU_LevelButton : MonoBehaviour
{
    private VariablesSaver vs;
    private string levelScene;
    private int number;
    private bool unavailable;
    private Button button;
    private Color green;
    // Start is called before the first frame update
    void Start()
    {
        green = new Color(58F / 255F, 215F / 255F, 65F / 255F);
        button = GetComponent<Button>();
        unavailable = false;
        vs = GameObject.FindGameObjectWithTag("VariablesSaver").GetComponent<VariablesSaver>();
        string[] nameInfo = gameObject.name.Split('_');
        if (nameInfo.Length == 2)
        {
            levelScene = nameInfo[0] + nameInfo[1];
            number = int.Parse(nameInfo[1]);
        }

        if(number == 1)
        {
            if(vs.levelsCompleted[1])
            {
                GetComponent<Image>().color = green;
            }
        }
        else  if(number >= 2 && number < vs.levelRange)
        {
            if (vs.levelsCompleted[number])
            {
                GetComponent<Image>().color = green;
            }
            else
            {
                if(!vs.levelsCompleted[number - 1])
                {
                    unavailable = true;
                    GetComponent<Image>().color = Color.red;
                }
            }
        }
        else
        {
            unavailable = true;
            GetComponent<Image>().color = Color.red;
        }
    }

    public void OnClick()
    {
        if (!unavailable)
        {
            if (number >= 0 && number <= vs.levelRange)
            {
                vs.LoadLevel(number);
            }
        }
    }
}
