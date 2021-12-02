using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MENU_LevelButton : MonoBehaviour,  IPointerEnterHandler,
                                   IPointerExitHandler
{
    private VariablesSaver vs;
    private string levelScene;
    private int number;
    private bool unavailable;
    private Button button;
    private Color green;
    [HideInInspector] public bool selected = false;
    private float timer;
    private Color defaultColor;
    private Color selectColor;
    private int currentColor;
    private Image image;
    private Coroutine cd;
    // Start is called before the first frame update
    void Start()
    {
        cd = null;
        image = GetComponent<Image>();
        currentColor = 0;
        defaultColor = new Color(163F / 255F, 154F / 255F, 157F / 255F);
        selectColor = new Color(4F / 255F, 57F / 255F, 255F / 255F);
        timer = 0F;
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
            image.color = Color.black;
            transform.GetChild(0).GetComponent<Image>().color = new Color(52F / 255F, 65F / 255F, 50F / 255F);
        }
    }

    private void Update()
    {
        if(number == vs.GetLeveltoBeat())
        {
            timer += Time.deltaTime;
            if (!selected)
            {
                if (timer >= 1.0F)
                {
                    timer = 0F;
                    if (currentColor == 0)
                    {
                        image.color = selectColor;
                        currentColor = 1;
                    }
                    else
                    {
                        image.color = defaultColor;
                        currentColor = 0;
                    }
                }
            }
            else
            {
                timer = 0F;
                image.color = selectColor;
            }
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
        if (number == vs.GetLeveltoBeat())
        {
            cd = StartCoroutine(PointerLeaveRoutine());
        }
    }

    private IEnumerator PointerLeaveRoutine(int ticks = 0)
    {
        if(ticks == 7)
        {
            cd = null;
            yield break;
        }
        if(!selected)
        {
            if (currentColor == 0)
            {
                image.color = defaultColor;
            }
            else
            {
                image.color = selectColor;
            }
        }
        else
        {
            cd = null;
            yield break;
        }
        ticks++;
        yield return new WaitForSeconds(0.125F);
        if (currentColor == 0)
        {
            currentColor = 1;
            StartCoroutine(PointerLeaveRoutine(ticks));
        }
        else if (currentColor == 1)
        {
            currentColor = 0;
            StartCoroutine(PointerLeaveRoutine(ticks));
        }
    }
}
