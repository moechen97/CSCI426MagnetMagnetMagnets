using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource hit;
    public AudioSource chargeUp;
    public AudioSource magnetChangePosition;
    public AudioSource spikeKey;
    public AudioSource win;
    public AudioSource backgroundMusic;
    public AudioSource batteryCharge;
    public AudioSource electricDie;
    public AudioSource batteryUncharge;
    public AudioSource bombExplode;
    public AudioSource bombDefuse;

    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("MusicManager").Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PlayHit()
    {
        hit.Play();
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

    public void PlayBatteryCharge()
    {
        batteryCharge.Play();
    }

    public void PlayElectricDie()
    {
        electricDie.Play();
    }

    public void PlayBatteryUncharge()
    {
        batteryUncharge.Play();
    }

    public void PlayBombExplode()
    {
        bombExplode.Play();
    }

    public void PlayBombDefuse()
    {
        bombDefuse.Play();
    }
}
