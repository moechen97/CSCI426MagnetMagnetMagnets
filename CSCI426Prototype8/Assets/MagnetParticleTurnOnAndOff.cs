using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetParticleTurnOnAndOff : MonoBehaviour
{
    [SerializeField] private GameObject ParticleUp;

    [SerializeField] private GameObject ParticleDown;

    [SerializeField] private GameObject ParticleLeft;

    [SerializeField] private GameObject ParticleRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
        {
            ParticleRight.SetActive(true);
            ParticleDown.SetActive(false);
            ParticleLeft.SetActive(false);
            ParticleUp.SetActive(false);
        }
        else if (transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
        {
            ParticleRight.SetActive(false);
            ParticleDown.SetActive(false);
            ParticleLeft.SetActive(true);
            ParticleUp.SetActive(false);
        }
        else if (transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
        {
            ParticleRight.SetActive(false);
            ParticleDown.SetActive(true);
            ParticleLeft.SetActive(false);
            ParticleUp.SetActive(false);
        }
        else if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
        {
            ParticleRight.SetActive(false);
            ParticleDown.SetActive(false);
            ParticleLeft.SetActive(false);
            ParticleUp.SetActive(true);
        }
    }
}
