using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VariablesSaver : MonoBehaviour
{
    public int levelRange;
    [HideInInspector] public int level;
    [HideInInspector] public bool[] levelsCompleted;
    [HideInInspector] public int currentLevel;
    public enum LevelState { Gray, Green, Red }
    void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!sceneName.Equals("New Menu")) level = sceneName[sceneName.Length - 1] - '0';
        else level = 0; 
        Debug.Log("VARIABLE SAVER: Begin level " + level);
        DontDestroyOnLoad(this.gameObject);
        levelsCompleted = new bool[levelRange];
    }

    public LevelState GetLevelState(int num)
    {
        int i;
        for(i = 0; i < num; i++)
        {
            if(!levelsCompleted[i])
            {
                break;
            }
            if(i > num)
            {
                break;
            }
        }
        if(i == num)
        {
            return LevelState.Gray;
        }
        else if(i > num)
        {
            return LevelState.Green;
        }
        else
        {
            return LevelState.Red;
        }
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("New Menu");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void NextLevel()
    {
        levelsCompleted[level] = true;
        if(level < levelRange) level++;
        SceneManager.LoadScene("Level" + level);
    }

    public void PreviousLevel()
    {
        if (level > 1) level--;
        SceneManager.LoadScene("Level" + level);
    }
}
