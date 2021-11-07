using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour
{
    public bool isLocked;
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
    private Music music;
    private void Awake()
    {
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
        blocks = new SpriteRenderer[transform.childCount];
        for(int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        }
        unlocking = false;
        unlockTimer = 1.0F;
        sr = GetComponent<SpriteRenderer>();
        srOriginalColor = sr.color;
        srGreen = new Color(36F / 256F, 123F / 256F, 18 / 256F);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerSr = player.gameObject.GetComponent<SpriteRenderer>();
        playerSrOriginalColor = playerSr.color;
    }

    public void ResetLock()
    {
        unlocking = false;
        isLocked = false;
    }

    private void Update()
    {
        if (unlocking) return;
        if (!isLocked)
        {
            sr.color = srOriginalColor;
        }
        else sr.color = srGreen;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (unlocking) return;
        if (!isLocked)
        {
            SetColor(srOriginalColor, true);
            if (collision.gameObject.CompareTag("Interactable"))
            {
                if (!isLocked)
                {
                    Contain(collision.gameObject.GetComponent<Interactable>());
                }
            }
            if(collision.gameObject.CompareTag("Player"))
            {
                player.Die();
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SetColor(Color.green, false);
            }
        }
    }


    public void Contain(Interactable i)
    {
        isLocked = true;
        music.PlaySpikeKey();
        unlocking = false;
        isLocked = true;
        SetColor(Color.green, false);
        i.SetLocked(index, transform);
    }

    public void Unlock()
    {
        unlocking = true;
        isLocked = false;
        StartCoroutine(UnlockRoutine());
    }

    private void SetColor(Color c, bool gateActive = true)
    {
        sr.color = c;
        if(gateActive)
        {
            foreach (SpriteRenderer s in blocks)
            {
                s.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (SpriteRenderer s in blocks)
            {
                s.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator UnlockRoutine()
    {
        SetColor(Color.green, false);
        yield return new WaitForSeconds(unlockTimer / 3F);
        SetColor(srOriginalColor, true);
        yield return new WaitForSeconds(unlockTimer / 3F);
        SetColor(srGreen, true);
        yield return new WaitForSeconds(unlockTimer / 3F);
        SetColor(srOriginalColor, true);
        unlocking = false;
    }
}
