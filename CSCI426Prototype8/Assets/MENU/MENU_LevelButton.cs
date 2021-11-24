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
    // Start is called before the first frame update
    void Start()
    {
        unavailable = false;
        vs = GameObject.FindGameObjectWithTag("VariablesSaver").GetComponent<VariablesSaver>();
        string[] nameInfo = gameObject.name.Split('_');
        if (nameInfo.Length == 2)
        {
            levelScene = nameInfo[0] + nameInfo[1];
            number = int.Parse(nameInfo[1]) - 1;
        }
        else
        {
            levelScene = nameInfo[0];
            number = -1;
        }

        if(number > vs.levelRange - 1)
        {
            unavailable = true;
            transform.GetChild(0).GetComponent<Image>().color = Color.red;
        }
    }

    public void OnClick()
    {
        if (!unavailable)
        {
            if (number > 0 && number <= vs.levelRange)
            {
                SceneManager.LoadScene(levelScene);
            }
            else if (number == 0)
            {
                SceneManager.LoadScene(levelScene);
            }
        }
    }
}
