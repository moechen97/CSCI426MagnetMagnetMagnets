using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Menu_QuitGame : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private GameObject noticeText;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    

    public void quitGame()
    {
        Application.Quit();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        noticeText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        noticeText.SetActive(false);
    }
}
