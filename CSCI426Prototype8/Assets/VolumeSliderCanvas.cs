using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeSliderCanvas : MonoBehaviour
{
    private TMPro.TextMeshProUGUI countdownText;
    private TMPro.TextMeshProUGUI deathCountText;
    private TMPro.TextMeshProUGUI deathCountTotalText;
    private TMPro.TextMeshProUGUI deathCountTitleText;
    private GameObject restartButton;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectsWithTag("VolumeSliderCanvas").Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            foreach(Transform child in transform)
            {
                if(child.name.Equals("Countdown_Text"))
                {
                    countdownText = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                }
                else if(child.name.Equals("DeathCountTotal_Text"))
                {
                    deathCountText = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                }
                else if (child.name.Equals("DeathCountTotalTotal_Text"))
                {
                    deathCountTotalText = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                }
                else if(child.name.Equals("DeathCountTitle_Text"))
                {
                    deathCountTitleText = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                }
                else if(child.name.Equals("RestartButton"))
                {
                    restartButton = child.gameObject;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name.Equals("New Menu"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            countdownText.gameObject.SetActive(false);
            restartButton.SetActive(false);
            deathCountText.gameObject.SetActive(false);
            deathCountTitleText.gameObject.SetActive(false);

        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            restartButton.SetActive(true);
        }
    }
}
