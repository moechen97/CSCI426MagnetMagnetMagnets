using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private MagnetMove mm;
    private Transform pullSource;
    private List<GameObject> pulls;
    private float pullSpeed = 3.0F;
    public enum Quadrant { Upper, Lower }
    private Quadrant currQuad;
    private LayerMask maskInteractable;
    private LayerMask maskBomb;
    private ParticleSystem ps;
    [HideInInspector] public bool snapBlocked;
    private float snapBlockedTimer;
    private Music music;
    private bool playingClink;
    public KeyLock[] KeyLocks;
    public 
    void Awake()
    {
        playingClink = false;
        snapBlocked = false;
        snapBlockedTimer = 0.0F;
        mm = GetComponent<MagnetMove>();
        pullSource = transform.GetChild(0);
        pulls = new List<GameObject>();
        currQuad = Quadrant.Upper;
        maskInteractable = LayerUtility.Only("Magnet", "Interactable");
        maskBomb = LayerUtility.Only("Magnet", "Bomb");
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
    }

    public void AddPull(GameObject i)
    {
        if (mm.currQuad == MagnetMove.Quadrant.None) return;
        if (i.CompareTag("Interactable"))
        {
            pulls.Add(i);
            if (pulls.Count > 1) Debug.Log("ADDPULL(): More than one pull");
            i.GetComponent<Interactable>().SetPull(pullSource, mm.currQuad);
        }
    }


    public void ResetPulls()
    {
        foreach(GameObject p in pulls)
        {
            if (p.CompareTag("Interactable")) p.GetComponent<Interactable>().ResetPull();
        }
        snapBlocked = false;
        pulls.Clear();
    }

    public void Update()
    {
        if(ps.gameObject.activeSelf)
        {
            if(snapBlocked && !playingClink)
            {
                if(ps.isPlaying) { playingClink = true;  StartCoroutine(TurnOffTractorBeam()); }
            }
        }
        else
        {
            if(!snapBlocked)
            {
                //Turn on tractor beam
                music.PlayUnSnapClink();
                ps.gameObject.SetActive(true);
                ps.Play();
            }
        }
    }

    private IEnumerator TurnOffTractorBeam()
    {
        if (pulls.Count == 0) { playingClink = false; yield break; }
        music.PlaySnapClink();
        ps.Stop();
        yield return new WaitForSeconds(0.35F);
        playingClink = false;
        if(snapBlocked) ps.gameObject.SetActive(false);
    }
    public void FixedUpdate()
    {
        if (snapBlocked || mm.currQuad == MagnetMove.Quadrant.None) return;
        Vector2 forward = Vector2.zero;
        if(mm.currQuad == MagnetMove.Quadrant.Up)
        {
           forward = -Vector2.up;
        }
        else if(mm.currQuad == MagnetMove.Quadrant.Down)
        {
            forward = Vector2.up;
        }
        else if(mm.currQuad == MagnetMove.Quadrant.Left)
        {
            forward = Vector2.right;
        }
        else if(mm.currQuad == MagnetMove.Quadrant.Right)
        {
            forward = Vector2.left;
        }
        else if(mm.currQuad == MagnetMove.Quadrant.DiagonalLeft)
        {
            forward = new Vector2(-1F, 1F);
        }
        else if (mm.currQuad == MagnetMove.Quadrant.DiagonalRight)
        {
            forward = new Vector2(1F, 1F);
        }

        //raycast for interactables
        RaycastHit2D[] raycasts = Physics2D.RaycastAll(transform.position, forward, 15F, maskInteractable);
        foreach (RaycastHit2D hit in raycasts)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Interactable"))
                {
                    Interactable i = hit.collider.gameObject.GetComponent<Interactable>();
                    if (i.pull == Interactable.PullDirection.Locked) break;
                    if (!pulls.Contains(hit.collider.gameObject)) AddPull(hit.collider.gameObject);
                    break;
                }
            }
        }
    }

    //Key Locks
    public void Unlock(int index)
    {
        Debug.Log("UNLOCK " + index);
        KeyLocks[index].Unlock();
    }
}
