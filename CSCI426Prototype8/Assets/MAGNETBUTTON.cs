using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MAGNETBUTTON : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
                                   IPointerExitHandler
{
    private MagnetManager magnetManager;
    private int index;
    SpriteRenderer sr;
    Color regularColor;
    Color hoverColor;
    private MagnetMove mm;
    private bool selected;
    private float flashTime;
    private float selectTimer;
    private float selectTimeLimit;
    private GameObject cursor;
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        string[] magnetInfo = gameObject.name.Split('_');
        if (magnetInfo[0].Equals("MagnetButtonUp"))
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
        ExpandClickRange();
        index = int.Parse(magnetInfo[1]);
        magnetManager = GameObject.FindGameObjectWithTag("MagnetManager").GetComponent<MagnetManager>();
        magnetManager.RecordMagnetPosition(index, transform);
        //Attach Physics2DRaycaster to the Camera
        Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        addEventSystem();
        cursor = GameObject.FindWithTag("Cursor");
    }

    private void ExpandClickRange()
    {
        if(gameObject.tag.Equals("Magnet_Up") || gameObject.tag.Equals("Magnet_Down"))
        {
            
        }
        else if(gameObject.tag.Equals("Magnet_Left") || gameObject.tag.Equals("Magnet_Right"))
        {
        }
        boxCollider.offset = new Vector2(0F, 1F);
        boxCollider.size = new Vector2(5.12F, 11.125F);
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
        yield return new WaitForSeconds(flashTime * 4F);
        sr.color = regularColor;
        transform.GetChild(0).gameObject.SetActive(false);
        if (!selected) yield break;
        yield return new WaitForSeconds(flashTime * 0.65F);
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
        MouseCursor.isPointer = true;
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
        MouseCursor.isPointer = false;

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
}