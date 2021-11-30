using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        slider.value = AudioListener.volume;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeVolume(float x)
    {
        AudioListener.volume = x;
    }
}
