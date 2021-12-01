using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour
{
    [HideInInspector] public bool isLocked;
    private SpriteRenderer sr;
    private Color srOriginalColor;
    private Color srGreen;
    private Player player;
    private SpriteRenderer playerSr;
    private Color playerSrOriginalColor;
    public int index;
    private bool unlocking;
    private float unlockTimer;
    private SpriteRenderer[] blocks;
    private BoxCollider2D[] colliders;
    private Music music;
    private Interactable spike;
    private MagnetMove mm;
    [HideInInspector] public bool spikeContact;
    private void Awake()
    {
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
        unlocking = false;
        unlockTimer = 0.5F;
        sr = GetComponent<SpriteRenderer>();
        srOriginalColor = sr.color;
        srGreen = new Color(36F / 256F, 123F / 256F, 18 / 256F);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerSr = player.gameObject.GetComponent<SpriteRenderer>();
        playerSrOriginalColor = playerSr.color;
        mm = GameObject.FindGameObjectWithTag("Magnet").GetComponent<MagnetMove>();
        spike = null;
        spikeContact = false;
        blocks = new SpriteRenderer[transform.childCount];
        colliders = new BoxCollider2D[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            blocks[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            colliders[i] = transform.GetChild(i).GetComponent<BoxCollider2D>();
        }
    }

    private void Update()
    {
        if (isLocked)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].enabled = false;
                colliders[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].enabled = true;
                colliders[i].enabled = true;
            }
        }
    }
    public void ResetLock()
    {
        unlocking = false;
        isLocked = false;
        spikeContact = false;
        SetColor(srOriginalColor, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Interactable i = collision.gameObject.GetComponent<Interactable>();
            if (!spikeContact && !isLocked)
            {
                Contain(i);
            }
            spikeContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (spikeContact)
        {
            if (collision.gameObject.CompareTag("Interactable"))
            {
                Debug.Log(isLocked);
                spikeContact = false;
                SetColor(srOriginalColor, true);
            }
        }
    }


    public void Contain(Interactable i)
    {
        isLocked = true;
        music.PlaySpikeKey();
        unlocking = false;
        isLocked = true;
        spikeContact = true;
        SetColor(Color.green, false);
        i.SetLocked(this, index, transform);
        spike = i;
    }

    public void Unlock()
    {
        unlocking = true;
        isLocked = false;
        spikeContact = true;
        Debug.Log("UNLOCK");
        spike = null;
        StartCoroutine(UnlockRoutine());
    }

    private void SetColor(Color c, bool gateActive = true)
    {
        //sr.color = c;
        //if(gateActive)
        //{
        //    for (int i = 0; i < blocks.Length; i++)
        //    {
        //        blocks[i].enabled = true;
        //        colliders[i].enabled = true;
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < blocks.Length; i++)
        //    {
        //        blocks[i].enabled = false;
        //        colliders[i].enabled = false;
        //    }
        //}
    }

    private IEnumerator UnlockRoutine()
    {
        //SetColor(Color.green, false);
        SetColor(srOriginalColor, true);
        yield return new WaitForSeconds(unlockTimer);
        unlocking = false;
    }
}
