using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource snapClink;
    public AudioSource unSnapClink;
    public AudioSource chargeUp;
    public AudioSource magnetChangePosition;
    public AudioSource spikeKey;
    public AudioSource win;

    private void Awake()
    {
        //if(GameObject.FindGameObjectWithTag("MusicManager") == null)
        //{
        //    DontDestroyOnLoad(this.gameObject);
        //    foreach(Transform child in transform)
        //    {
        //        DontDestroyOnLoad(this.gameObject);
        //    }
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //}
    }
    public void PlaySnapClink()
    {
        snapClink.Play();
    }

    public void PlayUnSnapClink()
    {
        unSnapClink.Play();
    }

    public void PlayChargeUp()
    {
        chargeUp.Play();
    }

    public void PlayChangeMagnetPosition()
    {
        magnetChangePosition.Play();
    }

    public void PlaySpikeKey()
    {
        spikeKey.Play();
    }

    public void PlayWin()
    {
        win.Play();
    }
}
