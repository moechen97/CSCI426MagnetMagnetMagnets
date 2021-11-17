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
        slider.value = AudioListener.volume;
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
