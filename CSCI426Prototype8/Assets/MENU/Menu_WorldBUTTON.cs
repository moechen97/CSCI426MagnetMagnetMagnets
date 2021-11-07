using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu_WorldBUTTON : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
                                                                     IPointerExitHandler
{
    public string World;
    private bool selected;
    void Start()
    {
        Debug.Log("START");
        //Attach Physics2DRaycaster to the Camera
        Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        addEventSystem();
    }

    void Awake()
    {
        selected = false;
    }

    private IEnumerator OnMouseHover()
    {
        selected = true;
        yield return new WaitForSeconds(0.5F);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("HI");
        StartCoroutine(OnMouseHover());
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("YO");
        SceneManager.LoadScene(World);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
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
