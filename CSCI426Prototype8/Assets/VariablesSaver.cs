using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VariablesSaver : MonoBehaviour
{
    [HideInInspector] public int levelRange;
    [HideInInspector] public int level;
    [HideInInspector] public bool[] levelsCompleted;
    [HideInInspector] public int currentLevel;

    public string[] Levels;
    public enum LevelState { Gray, Green, Red }
    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("VariablesSaver").Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        string sceneName = SceneManager.GetActiveScene().name;
        level = 0;
        levelRange = Levels.Length - 1;
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

    public void LoadLevel(int index)
    {
        level = index;
        SceneManager.LoadScene(Levels[level]);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void NextLevel()
    {
        if (level < levelRange)
        {
            level++;
        }
        else
        {
            level = 0;
        }
        SceneManager.LoadScene(Levels[level]);
    }

    public void PreviousLevel()
    {
        if(level > 0)
        {
            level--;
        }
        else
        {
            level = levelRange;
        }
        SceneManager.LoadScene(Levels[level]);
    }
}
