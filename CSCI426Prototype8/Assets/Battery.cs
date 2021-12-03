using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private Color chargedColor;
    private Color unchargedColor;
    [SerializeField] private GameObject batteryAnimationP;
    [HideInInspector] public bool charged;
    private SpriteRenderer[] sr;
    private Animator _batteryAnimator;
    private float animationSpeed;
    private float chargeTimer;
    private float chargeTimeExpire;
    private Music music;
    void Awake()
    {
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
        chargedColor = batteryAnimationP.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        unchargedColor = Color.red;
        charged = false;
        sr = new SpriteRenderer[batteryAnimationP.transform.childCount];
        int i = 0;
        foreach(Transform child in batteryAnimationP.transform)
        {
            sr[i] = child.GetComponent<SpriteRenderer>();
            i++;
        }

        _batteryAnimator = batteryAnimationP.GetComponent<Animator>();
        animationSpeed = _batteryAnimator.speed;
        _batteryAnimator.speed=0;

        chargeTimer = 0F;
        chargeTimeExpire = 6.25F;
    }

    public void Charge()
    {
        charged = true;
        chargeTimer = 0F;
        music.PlayBatteryCharge();
    }

    // Update is called once per frame
    void Update()
    {
        if(!charged)
        {
            ColorUncharged();
        }
        else
        {
            ColorCharged();
            chargeTimer += Time.deltaTime;
            if(chargeTimer >= chargeTimeExpire)
            {
                UnCharge();
            }
        }
    }

    public void UnCharge()
    {
        charged = false;
        chargeTimer = 0F;
        music.PlayBatteryUncharge();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spark"))
        {
            Interactable i = collision.transform.parent.gameObject.GetComponent<Interactable>();
            if (i.Charged)
            {
                Debug.Log("CHARGE BATTERY");
                Charge();
                i.RemoveCharge();
            }
        }
    }

    void ColorUncharged()
    {
        foreach(SpriteRenderer s in sr)
        {
            s.color = unchargedColor;
        }
       
        _batteryAnimator.Rebind();
        _batteryAnimator.speed=0f;
    }

    void ColorCharged()
    {
        foreach(SpriteRenderer s in sr)
        {
            s.color = chargedColor;
        }

        _batteryAnimator.speed=animationSpeed;
    }
}
