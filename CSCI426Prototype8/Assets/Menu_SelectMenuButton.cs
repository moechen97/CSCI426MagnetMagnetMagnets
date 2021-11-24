using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_SelectMenuButton : MonoBehaviour
{
    public GameObject selectLevelMenu;
    public GameObject startGameMenu;
    public void OnClick()
    {
        selectLevelMenu.gameObject.SetActive(false);
        startGameMenu.gameObject.SetActive(true);
    }
}
