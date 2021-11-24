using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MENU_LevelButton : MonoBehaviour
{
    private string levelScene;
    // Start is called before the first frame update
    void Start()
    {
        string[] nameInfo = gameObject.name.Split('_');
        if (nameInfo.Length == 2)
        {
            levelScene = nameInfo[0] + nameInfo[1];
        }
        else
        {
            levelScene = nameInfo[0];
        }
    }

    public void OnClick()
    {
        SceneManager.LoadScene(levelScene);
    }
}
