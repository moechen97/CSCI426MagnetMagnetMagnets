using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VariablesSaver : MonoBehaviour
{
    [HideInInspector] public bool gameStarted;
    [HideInInspector] public int levelRange;
    [HideInInspector] public int level;
    [HideInInspector] public bool[] levelsCompleted;
    [HideInInspector] public int currentLevel;
    public string[] Levels;
    public enum LevelState { Gray, Green, Red }
    [HideInInspector] public bool[] research;
    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("VariablesSaver").Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        gameStarted = false;
        string sceneName = SceneManager.GetActiveScene().name;
        level = 0;
        levelRange = Levels.Length;
        Debug.Log("VARIABLE SAVER: Begin level " + level);
        DontDestroyOnLoad(this.gameObject);
        levelsCompleted = new bool[levelRange];
        research = new bool[3];
        for(int r = 0; r < 3; r++)
        {
            research[r] = false;
        }
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
        gameStarted = true;
        level = index;
        SceneManager.LoadScene(Levels[level]);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void NextLevel()
    {
        levelsCompleted[level] = true;
        //Check research
        if(level == 5)
        {
            research[0] = true;
        }
        else if(level == 7)
        {
            research[1] = true;
        }
        else if(level == 10)
        {
            research[2] = true;
        }

        //Move on to next level
        if (level < levelRange - 1)
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
