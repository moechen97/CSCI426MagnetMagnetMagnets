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
    [HideInInspector] public int deathCount;
    public string[] Levels;
    public enum LevelState { Gray, Green, Red }
    [HideInInspector] public bool[] research;
    private MagnetMove magnetMove;
    [HideInInspector] public TMPro.TextMeshProUGUI deathCountTotalTitleText;
    [HideInInspector] public TMPro.TextMeshProUGUI deathCountTitleText;
    [HideInInspector] public TMPro.TextMeshProUGUI deathCountTotalText;
    [HideInInspector] public TMPro.TextMeshProUGUI deathCountText;
    [HideInInspector] public int currentLevelDeath;
    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("VariablesSaver").Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        //Screen.SetResolution(Mathf.RoundToInt((Screen.currentResolution.height / 1.6F) * 1.7777778F), Mathf.RoundToInt(Screen.currentResolution.height / 1.6F), FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
        //Screen.SetResolution(1280, 720, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
        deathCount = 0;
        currentLevelDeath = 0;
        GameObject vsc = GameObject.FindGameObjectWithTag("VolumeSliderCanvas");
        foreach(Transform child in vsc.transform)
        {
            if(child.gameObject.name.Equals("DeathCountTotal_Text"))
            {
                deathCountText = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (child.gameObject.name.Equals("DeathCountTotalTotal_Text"))
            {
                deathCountTotalText = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (child.gameObject.name.Equals("DeathCountTotalTotalTitle_Text"))
            {
                deathCountTotalTitleText = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (child.gameObject.name.Equals("DeathCountTitle_Text"))
            {
                deathCountTitleText = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            }
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

    public int GetLeveltoBeat()
    {
        for (int i = 1; i < levelsCompleted.Length; i++)
        {
            if (!levelsCompleted[i])
            {
                return i;
            }
        }
        return 0;
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name.Equals("New Menu"))
        {
            if(levelsCompleted[0])
            {
                deathCountTotalText.gameObject.SetActive(true);
                deathCountTotalTitleText.gameObject.SetActive(true);
            }
            else
            {
                deathCountTotalText.gameObject.SetActive(false);
                deathCountTotalTitleText.gameObject.SetActive(false);
            }
        }
        else
        {
            deathCountTotalText.gameObject.SetActive(false);
            deathCountTotalTitleText.gameObject.SetActive(false);
        }
    }
    public void RecordDeath()
    {
        deathCount++;
        currentLevelDeath++;
        deathCountText.text = currentLevelDeath.ToString();
        deathCountTotalText.text = deathCount.ToString();
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
        levelsCompleted[0] = true;
        currentLevelDeath = 0;
        gameStarted = true;
        level = index;
        SceneManager.LoadScene(Levels[level]);
    }

    public void ReloadLevel()
    {
        currentLevelDeath = 0;
        SceneManager.LoadScene("Level" + level);
    }

    public void NextLevel()
    {
        currentLevelDeath = 0;
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
        currentLevelDeath = 0;
        if (level > 0)
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
