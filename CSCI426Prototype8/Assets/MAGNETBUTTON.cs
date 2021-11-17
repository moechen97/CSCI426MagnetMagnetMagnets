using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MAGNETBUTTON : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
                                   IPointerExitHandler
{
    private int index;
    SpriteRenderer sr;
    Color regularColor;
    Color hoverColor;
    private MagnetMove mm;
    private bool selected;
    private float flashTime;
    private float selectTimer;
    private float selectTimeLimit;

    void Start()
    {
        //Attach Physics2DRaycaster to the Camera
        Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        addEventSystem();
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        regularColor = sr.color;
        hoverColor = Color.gray;
        mm = GameObject.FindGameObjectWithTag("Magnet").GetComponent<MagnetMove>();
        flashTime = 0.85F;
        selectTimer = 0F;
        selected = false;
        selectTimeLimit = 3.675F;
        string[] magnetInfo = gameObject.name.Split('_');
        if(magnetInfo[0].Equals("MagnetButtonUp"))
        {
            gameObject.tag = "Magnet_Up";
        }
        else if (magnetInfo[0].Equals("MagnetButtonDown"))
        {
            gameObject.tag = "Magnet_Down";
        }
        else if (magnetInfo[0].Equals("MagnetButtonRight"))
        {
            gameObject.tag = "Magnet_Right";
        }
        else if (magnetInfo[0].Equals("MagnetButtonLeft"))
        {
            gameObject.tag = "Magnet_Left";
        }

        index = int.Parse(magnetInfo[1]);
    }

    private IEnumerator OnMouseHover()
    {
        if (!selected)
        {
            sr.color = regularColor;
            yield break;
        }
        sr.color = hoverColor;
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        sr.color = regularColor;
        transform.GetChild(0).gameObject.SetActive(false);
        if (!selected) yield break;
        yield return new WaitForSeconds(flashTime);
        if (!selected) yield break;
        sr.color = hoverColor;
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(OnMouseHover());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mm.magnetIndex != index) mm.HideCurrentMagnetHint();
        selected = true;
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(OnMouseHover());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (mm.magnetIndex != index)
        {
            mm.SetMagnetPos(index);
        }
        else
        {
            mm.SetMagnetPos(0);
        }
        selected = false;
        mm.UnhideCurrentMagnetHint();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mm.magnetIndex != index) mm.UnhideCurrentMagnetHint();
        sr.color = regularColor;
        selected = false;
        transform.GetChild(0).gameObject.SetActive(false);
        selectTimer = 0F;
    }


    //Add Event System to the Camera
    void addEventSystem()
    {
        GameObject eventSystem = null;
        GameObject tempObj = GameObject.Find("EventSystem");
        if (tempObj == null)
        {
            eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        else
        {
            if ((tempObj.GetComponent<EventSystem>()) == null)
            {
                tempObj.AddComponent<EventSystem>();
            }

            if ((tempObj.GetComponent<StandaloneInputModule>()) == null)
            {
                tempObj.AddComponent<StandaloneInputModule>();
            }
        }
    }
    //IPointerClickHandler,
    //IPointerDownHandler, IPointerEnterHandler,
    //IPointerUpHandler, IPointerExitHandler
    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    Debug.Log("Mouse Up!");
    //}


    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("Mouse Down!");
    //}
}