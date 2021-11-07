using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    private CinemachineBasicMultiChannelPerlin _noiseControl;

    private float counter;
    private bool CountDownState;
    private float ShakeExtent;

    [SerializeField] private float ShakeStrength;
    [SerializeField] private float colddownTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _noiseControl = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _noiseControl.m_AmplitudeGain = 0;
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CountDownState)
        {
            _noiseControl.m_AmplitudeGain = ShakeStrength;
            counter += Time.deltaTime;
            if (counter>=colddownTime)
            {
                _noiseControl.m_AmplitudeGain = 0f;
                counter = 0;
                CountDownState = false;
            }
        }
    }

    public void ShakeScreen()
    {
        CountDownState = true;
    }
}
