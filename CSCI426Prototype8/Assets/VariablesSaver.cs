using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VariablesSaver : MonoBehaviour
{
    public int levelRange;
    [HideInInspector] public int level;
    void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!sceneName.Equals("Menu")) level = sceneName[sceneName.Length - 1] - '0';
        else level = 0;
        Debug.Log("VARIABLE SAVER: Begin level " + level);
        DontDestroyOnLoad(this.gameObject);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void NextLevel()
    {
        if(level < levelRange) level++;
        SceneManager.LoadScene("Level" + level);
    }

    public void PreviousLevel()
    {
        if (level > 1) level--;
        SceneManager.LoadScene("Level" + level);
    }
}
