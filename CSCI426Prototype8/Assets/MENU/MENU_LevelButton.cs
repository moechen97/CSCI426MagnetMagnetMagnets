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
    private bool available;
    // Start is called before the first frame update
    void Start()
    {
        available = false;
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

        bool seen = false;
        if (number != -1)
        {
            if(vs.GetLevelState(number) == VariablesSaver.LevelState.Green)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.green;
                available = true;
            }
            else if (vs.GetLevelState(number) == VariablesSaver.LevelState.Red)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
            }
            else
            {
                available = true;
                Debug.Log("FIRST UNBEATEN LEVEL: " + number);
            }
        }
    }

    public void OnClick()
    {
        if (available)
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
